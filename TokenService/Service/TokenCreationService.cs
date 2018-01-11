using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TokenService.Model.Entity;
using TokenService.Model.Rest;
using TokenService.Repository;

namespace TokenService.Service
{
    /// <summary>
    /// Token creation and managment service implementation
    /// </summary>
    public class TokenCreationService : ITokenCreationService
    {
        /// <summary>
        /// injected
        /// </summary>
        private readonly ILogger<TokenCreationService> _logger;
        /// <summary>
        /// injected
        /// </summary>
        private readonly IRepository<TokenEntity> _repository;

        /// <summary>
        /// horrible temporary algorithm. Cryptographically unsound
        /// </summary>
        /// <returns></returns>
        private string SecretKey()
        {
            // 68+68 characters
            return Guid.NewGuid().ToString("X") + Guid.NewGuid().ToString("X");
        }

        /// <summary>
        /// Constructor for Dependency injection
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        public TokenCreationService(ILogger<TokenCreationService> logger, IRepository<TokenEntity> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Creates a token and returns it in the response. Throws an exception wrapping the response if there is an error.
        /// BadArgumentException if the request is bad
        /// FailedException if there was some other problem
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
                jwtToken = newJwt,
                Version = "1.0",
            };
            return response;
        }

        /// <summary>
        /// Creates an entity from a submitted requests.  The entity is not complete until a JWT has been added to it.
        /// This means the entity must be mutable :-(
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal TokenEntity CreateTokenEntity(TokenCreateRequest request)
        {
            DateTime now = DateTime.Now;
            // TODO create initiator for this constructor. We set obo as property below
            // TODO get initiator from request or security context
            // Consumed by is initially empty and updated by Validate()
            TokenEntity entity = new TokenEntity(new TokenIdentityEntity(request.OnBehalfOf.ProviderName, request.OnBehalfOf.UserName), null)
            {
                Context = request.Context,
                EffectiveTime = request.EffectiveTime,
                ExpirationIntervalSec = request.ExpirationIntervalSeconds,
                ExpirationTime = now.AddSeconds(request.ExpirationIntervalSeconds),
                InitiationTime = now,
                JwtUniqueIdentifier = Guid.NewGuid().ToString(),
                JwtSecret = SecretKey(),
                ProtectedUrl = request.ProtectedUrl,
                Version = "1.0",
            };
            return entity;
        }

#pragma warning disable CA1822
        /// <summary>
        /// throws BadArgumentException if there is a problem with the token.
        /// </summary>
        /// <param name="request"></param>
        internal void ValidateRequest(IValidatableObject request)
        {
            ValidationContext context = new ValidationContext(request, null, null);
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, context, validationResults, true);
            this.RaiseValidationErrors(validationResults);
        }

        internal string CreateJwt(TokenEntity entity)
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

    }
}
