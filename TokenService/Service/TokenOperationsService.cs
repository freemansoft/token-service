using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.RegularExpressions;
using TokenService.Exception;
using TokenService.Model.Dto;
using TokenService.Model.Entity;
using TokenService.Repository;

namespace TokenService.Service
{
    /// <summary>
    /// Token creation and managment service implementation
    /// </summary>
    public class TokenOperationsService : ITokenOperationsService
    {
        /// <summary>
        /// injected
        /// </summary>
        private readonly ILogger<TokenOperationsService> _logger;
        /// <summary>
        /// injected
        /// </summary>
        private readonly IRepository<TokenEntity> _repository;

        /// <summary>
        /// Constructor injected properties from appsettings.json
        /// </summary>
        private readonly IOptions<CryptographySettings> _cryptoSettings;

        /// <summary>
        /// horrible temporary algorithm. Cryptographically unsound
        /// </summary>
        /// <returns></returns>
        private string SecretKey()
        {
            return _cryptoSettings.Value.JwtSecret;
        }

        /// <summary>
        /// Constructor for Dependency injection
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        /// <param name="cryptoSettings">the JST Token secret</param>
        public TokenOperationsService(ILogger<TokenOperationsService> logger, IRepository<TokenEntity> repository, IOptions<CryptographySettings> cryptoSettings)
        {
#pragma warning disable IDE0016 
            if (logger == null) { throw new ConfigurationException("logger null when creating TokenCreationService"); }
            if (repository == null) { throw new ConfigurationException("repository null when creating TokenCreationService"); }
#pragma warning restore IDE0016 
            if (cryptoSettings == null) { throw new ConfigurationException("cryptoSettings null when creating TokenCreationService"); }
            if (cryptoSettings.Value.JwtSecret == null) { throw new ConfigurationException("cryptosettings is missing the JWT secret seed value"); }
            _logger = logger;
            _repository = repository;
            _cryptoSettings = cryptoSettings;
        }

        /// <summary>
        /// Creates a token and returns it in the response. 
        /// Throws an exception wrapping the response if there is an error.
        /// Throws BadArgumentException if the request is bad
        /// FailedException if there was some other problem
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A response containing the token</returns>
        public TokenCreateResponse CreateToken(TokenCreateRequest request)
        {
            ValidateRequest(request);
            // yeah its circular because we store the JWT in the entity and JWT creation uses the entity fields.
            TokenEntity entity = CreateTokenEntity(request);
            string newJwt = CreateJwt(entity);
            entity.JwtToken = newJwt;
            // save the entity, create a response and get out of here
            _repository.Create(entity);
            TokenCreateResponse response = new TokenCreateResponse()
            {
                JwtToken = newJwt,
            };
            return response;
        }

        /// <summary>
        /// Validates the passed in token should be honored
        /// Returns the response.  
        /// Throws an exception containing a response if it fails
        /// Throws BadArgumentException if the request is bad
        /// FailedException if there was some other problem
        /// </summary>
        /// <param name="request"></param>
        /// <returns>context information from the token</returns>
        public TokenValidateResponse ValidateToken(TokenValidateRequest request)
        {
            ValidateRequest(request);
            string jwtEncodedString = request.JwtToken;
            string protectedUrl = request.ProtectedUrl;
            // convert string to POCO
            JwtSecurityToken jwtToken = new JwtSecurityToken(jwtEncodedString);
            TokenEntity jwtTokenEntity = _repository.GetById(jwtToken.Id);
            if (jwtTokenEntity == null)
            {
                throw new NotFoundException(String.Format(
                    "JWT TokenId={0} Token not found", jwtToken.Id));
            }
            ValidateTokenSignature(jwtEncodedString, jwtTokenEntity);
            // validate the basic token and the URL
            ValidateEncodedJwt(jwtToken, jwtTokenEntity, request.ProtectedUrl);
            TokenEntity postValidationEntity = ValidateExpirationPolicy(jwtTokenEntity);
            // assuming we validated and found it
            TokenValidateResponse response = new TokenValidateResponse()
            {
                Context = jwtTokenEntity.Context
            };
            // save the updated usage count and any audit information
            _logger.LogDebug("Saving updated entity {0}", postValidationEntity);
            _repository.Update(postValidationEntity);
            return response;
        }

        /// <summary>
        /// Creates an entity from a submitted requests.  The entity is not complete until a JWT has been added to it.
        /// This means the entity must be muta)ble :-(
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal TokenEntity CreateTokenEntity(TokenCreateRequest request)
        {
            DateTime now = DateTime.Now;
            // TODO create initiator for this constructor. Get initiator from request or security context. We set obo as property below.
            // Consumed by is initially empty and updated by Validate()
            TokenEntity entity = new TokenEntity(new TokenIdentityEntity(request.OnBehalfOf.ProviderName, request.OnBehalfOf.UserName), null)
            {
                Context = request.Context,
                MaxUseCount = request.MaxUsageCount,
                EffectiveTime = request.EffectiveTime,
                ExpirationIntervalSec = request.ExpirationIntervalSeconds,
                ExpirationTime = request.EffectiveTime.AddSeconds(request.ExpirationIntervalSeconds),
                InitiationTime = now,
                JwtUniqueIdentifier = Guid.NewGuid().ToString(),
                JwtSecret = SecretKey(),
                ProtectedUrl = request.ProtectedUrl,
            };
            return entity;
        }

        /// <summary>
        /// throws BadArgumentException if there is a problem with the token.
        /// </summary>
        /// <param name="request"></param>
#pragma warning disable CA1822
        internal void ValidateRequest(IValidatableObject request)
        {
#pragma warning restore CA1822
            ValidationContext context = new ValidationContext(request, null, null);
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, context, validationResults, true);
            this.RaiseValidationErrors(validationResults);
        }

#pragma warning disable CA1822
        internal string CreateJwt(TokenEntity entity)
#pragma warning restore CA1822
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(entity.JwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            // create the header
            JwtHeader header = new JwtHeader(credentials);
            // create payload portion of the token
            JwtPayload payload = new JwtPayload()
            {
                {"jti", entity.JwtUniqueIdentifier },
                {"sub", entity.ProtectedUrl },
                {"aud", entity.OnBehalfOf.UserName },
                {"iss", entity.Initiator.UserName },
                {"iat", entity.InitiationTime },
                {"nbf", entity.EffectiveTime },
                {"exp", entity.ExpirationTime }
            };
            // assemble the token and convert to string
            JwtSecurityToken newToken = new JwtSecurityToken(header, payload);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string tokenString = handler.WriteToken(newToken);
            entity.JwtToken = tokenString;
            return tokenString;
        }

        /// <summary>
        /// Validate the signing part of this
        /// </summary>
        /// <param name="signedToken"></param>
        /// <param name="jwtToken"></param>
        internal void ValidateTokenSignature(string signedToken, TokenEntity jwtToken)
        {
            // kind of a simple hack
            string tokenFromEntity = CreateJwt(jwtToken);
            if (!tokenFromEntity.Equals(signedToken))
            {
                throw new ConsistencyException(String.Format(
                    "JWT TokenId={0} CalculatedSignature={1} does not match ProvidedSignature={2}",
                    jwtToken.Id, tokenFromEntity, signedToken));
            }
        }

        /// <summary>
        /// This thows an exception on validation failure. 
        /// Should it return something instead?
        /// </summary>
        /// <param name="jwtToken">token submitted as part of validation request</param>
        /// <param name="jwtTokenEntity">token retrieved from the token store</param>
        /// <param name="targetUrl">url validation request is being made for</param>
        internal void ValidateEncodedJwt(JwtSecurityToken jwtToken, TokenEntity jwtTokenEntity, string targetUrl)
        {
            // they should always match since we use the jwtToken "id" to retrieve the jwtTokenEntity from the store
            if (!jwtTokenEntity.Id.Equals(jwtToken.Id))
            {
                _logger.LogWarning(
                    "JWT TokenId={0} does not match stored TokenId={1}", jwtToken.Id, jwtTokenEntity.Id);
                throw new ConsistencyException(String.Format(
                    "JWT TokenId={0} does not match stored TokenId={1}", jwtToken.Id, jwtTokenEntity.Id));
            }
            // Validate the requested URL against the URL the token was created for
            // Note that this is a SOFT match anchored at the start
            // todo add support for real regex or hard/soft.
            Regex ourRegex = new Regex("^" + jwtTokenEntity.ProtectedUrl, RegexOptions.IgnoreCase);
            Match urlRegexResults = ourRegex.Match(targetUrl);
            if (!urlRegexResults.Success)
            {
                _logger.LogWarning(
                    "JWT TokenId={0} RequestedUrl='{1}' does not match store token ProtectedUrl='{2}'", jwtToken.Id, targetUrl, jwtTokenEntity.ProtectedUrl);
                throw new ViolationException(String.Format(
                    "JWT TokenId={0} RequestedUrl='{1}' does not match store token ProtectedUrl='{2}'", jwtToken.Id, targetUrl, jwtTokenEntity.ProtectedUrl));
            }
            // victory!
        }

        /// <summary>
        /// Verifies time and usage count limits.
        /// <para></para>
        /// This thows an exception on validation failure. 
        /// Should it return something instead?
        /// <para></para>
        /// Mutates the passed in TokenEntity!  This is a bad idea.
        /// </summary>
        /// <param name="jwtTokenEntity">token retrieved from the token store</param>
        /// <returns>Updated TokenEntity that has to be re-saved to DB to update statistics</returns>
        internal TokenEntity ValidateExpirationPolicy(TokenEntity jwtTokenEntity)
        {
            DateTime rightNow = DateTime.Now;
            // Comparison returns <1 if rightNow is prior to jwtTokenEntity.EffectiveTime
            // Comparison returns >1 if rightNow is later than jwtTokenEntity.EffectiveTime
            if (DateTime.Compare(rightNow, jwtTokenEntity.EffectiveTime) < 0)
            {
                // token not yet effective
                throw new ViolationException("JWT TokenId=" + jwtTokenEntity.Id + " not yet effective now=" + rightNow + " EffectiveTime=" + jwtTokenEntity.ExpirationTime);
            }

            if (jwtTokenEntity.MaxUseCount <= jwtTokenEntity.CurrentUseCount)
            {
                // one more puts us over the limit
                throw new ViolationException("JWT TokenId=" + jwtTokenEntity.Id + " exceeded MaxUseCount=" + jwtTokenEntity.MaxUseCount);
            }
            else
            {
                // under max use count - assume entity will get saved later
                jwtTokenEntity.CurrentUseCount++;
                _logger.LogDebug("JWT TokenId={0} CurrentUseCount={1} MaxUseCount={2}", jwtTokenEntity.Id, jwtTokenEntity.CurrentUseCount, jwtTokenEntity.MaxUseCount);
            }

            // Comparison returns <1 if rightNow is prior to jwtTokenEntity.ExpirationTime
            // Comparison returns >1 if rightNow is later than jwtTokenEntity.ExpirationTime
            if (DateTime.Compare(rightNow, jwtTokenEntity.ExpirationTime) > 0)
            {
                // token expired
                throw new ViolationException("JWT TokenId=" + jwtTokenEntity.Id + " expired now=" + rightNow + " ExpirationTime=" + jwtTokenEntity.ExpirationTime);
            }
            // TODO make TokenEntity immutable and return copy
            return jwtTokenEntity;
        }

    }
}
