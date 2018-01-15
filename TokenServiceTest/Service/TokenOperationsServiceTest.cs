using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using TokenService.Exception;
using TokenService.Model.Entity;
using TokenService.Model.Rest;
using TokenService.Repository;
using TokenService.Service;
using Xunit;
using Xunit.Abstractions;

namespace TokenServiceTest
{
    public class TokenOperationsServiceTest
    {
        /// <summary>
        /// logging support 
        /// </summary>
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Note that this is anchored at the beginning of a line
        /// </summary>
        private string _protectedUrl = "http://myurl.com";

        private ILogger<TokenOperationsService> logger;
        private TokenInMemRepository inMemoryRepo;
        private TokenOperationsService underTest;


        public TokenOperationsServiceTest(ITestOutputHelper output)
        {
            this._output = output;
            logger = Mock.Of<ILogger<TokenOperationsService>>();
            inMemoryRepo = new TokenInMemRepository();
            underTest = new TokenOperationsService(logger, inMemoryRepo, BuildCryptographySettings());

        }

        /// <summary>
        /// Empty objects should fail input validation
        /// </summary>
        [Fact]
        public void CreateTokenInvalidObject()
        {
            // set no mandatory properties
            TokenCreateRequest request = new TokenCreateRequest(null);
            try
            {
                TokenCreateResponse response = underTest.CreateToken(request);
                Assert.True(false, "Should have thrown an exception");
            }
            catch (BadArgumentException e)
            {
                Assert.NotNull(e.ServiceResponse);
                _output.WriteLine("validation messages included " + e.ServiceResponse);
            }
        }

        [Fact]
        public void CreateTokenForceValidationError()
        {
            TokenCreateRequest request = BuildTokenCreateRequest();
            request.Version = null;
            try
            {
                TokenCreateResponse createResult = underTest.CreateToken(request);
                Assert.False(true, "Create Token should have failed model validation");
            }
            catch (BadArgumentException e)
            {
                _output.WriteLine("Received expected BadArgumentException when mandatory aTribute not set: " + e.ServiceResponse);
            }
        }

        [Fact]
        public void CreateTokenSuccess()
        {
            TokenCreateRequest request = BuildTokenCreateRequest();
            TokenCreateResponse createResult = underTest.CreateToken(request);
            Assert.NotNull(createResult);
            Assert.NotNull(createResult.jwtToken);
            _output.WriteLine("Calculated Token Encoded : " + createResult.jwtToken);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken foundToken = tokenHandler.ReadJwtToken(createResult.jwtToken);
            Assert.NotNull(foundToken);
            Assert.Equal(_protectedUrl, foundToken.Payload.Sub);
        }

        /// <summary>
        /// Test key needs to be >= 128 characters
        /// </summary>
        /// <returns></returns>
        internal IOptions<CryptographySettings> BuildCryptographySettings()
        {
            CryptographySettings mySeTings = new CryptographySettings()
            {
                // 68+68 characters
                JwtSecret = Guid.NewGuid().ToString("X") + Guid.NewGuid().ToString("X")
            };

            return Options.Create<CryptographySettings>(mySeTings);
        }

        internal TokenCreateRequest BuildTokenCreateRequest()
        {
            TokenIdentity obo = new TokenIdentity(null, "testid");
            TokenCreateRequest request = new TokenCreateRequest(obo)
            {
                ProtectedUrl = _protectedUrl,
                Version = "1.0",
                Context = JObject.Parse(@"{ ""x"":""value""}"),
            };
            return request;

        }


        /********************************************************************************
         * 
         * 
         * ****************************************************************************** */

        /// <summary>
        /// Empty objects should fail input validation
        /// </summary>
        [Fact]
        public void ValidateRequestInvalid()
        {
            // set none of mandatory properties
            TokenValidateRequest request = new TokenValidateRequest();
            try
            {
                TokenValidateResponse response = underTest.ValidateToken(request);
                Assert.True(false, "Should have thrown an exception");
            }
            catch (BadArgumentException e)
            {
                // should validate all the properties called out
                Assert.NotNull(e.ServiceResponse);
                _output.WriteLine("validation messages included " + e.ServiceResponse);
            }
        }

        /// <summary>
        /// this test assumes the CreateToken tests passed
        /// </summary>
        [Fact]
        public void ValidateEncodedJwtBadId()
        {
            TokenCreateRequest request = BuildTokenCreateRequest();
            // added this catch block in when we failed creating tokens because regex didn't fit in URL validation.
            // The tangled web we weave
            try
            {
                TokenCreateResponse createResult = underTest.CreateToken(request);

                Assert.NotNull(createResult.jwtToken);
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.jwtToken);
                TokenEntity badEntity = new TokenEntity(null, null)
                {
                    JwtUniqueIdentifier = "dogfood"
                };
                try
                {
                    underTest.ValidateEncodedJwt(receivedToken, badEntity, badEntity.ProtectedUrl);
                    Assert.False(true, "Expected FailedException");
                }
                catch (FailedException e)
                {
                    _output.WriteLine("Caught expected exception: " + e.Message);
                }
            }
            catch (BadArgumentException e)
            {
                Assert.False(true, "Caught unexpected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }

        [Fact]
        public void ValidateEncodedJwtBadUrl()
        {
            TokenCreateRequest request = BuildTokenCreateRequest();
            TokenCreateResponse createResult = underTest.CreateToken(request);
            Assert.NotNull(createResult.jwtToken);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.jwtToken);
            Assert.NotNull(receivedToken);
            TokenEntity foundEntity = inMemoryRepo.GetById(receivedToken.Id);
            Assert.NotNull(foundEntity);
            try
            {
                underTest.ValidateEncodedJwt(receivedToken, foundEntity, "http://badurl");
                Assert.False(true, "Expected FailedException");
            }
            catch (FailedException e)
            {
                _output.WriteLine("Caught expected exception: " + e.Message);
            }
        }

        [Fact]
        public void ValidateEncodedJwtBadUrlAnchor()
        {
            TokenCreateRequest request = BuildTokenCreateRequest();
            TokenCreateResponse createResult = underTest.CreateToken(request);
            Assert.NotNull(createResult.jwtToken);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.jwtToken);
            Assert.NotNull(receivedToken);
            TokenEntity foundEntity = inMemoryRepo.GetById(receivedToken.Id);
            Assert.NotNull(foundEntity);
            try
            {
                underTest.ValidateEncodedJwt(receivedToken, foundEntity, "x" + foundEntity.ProtectedUrl);
                Assert.False(true, "Expected FailedException");
            }
            catch (FailedException e)
            {
                _output.WriteLine("Caught expected exception: " + e.Message);
            }
        }

        /// <summary>
        /// feature not yet implemented
        /// </summary>
        [Fact(Skip = "not yet implemented")] // Xunit 2.x https://xunit.github.io/docs/comparisons.html
        public void ValidateEncodedJwtBadSignature()
        {

        }

        [Fact]
        public void ValidateEncodedJwtSuccess()
        {
            TokenCreateRequest request = BuildTokenCreateRequest();
            TokenCreateResponse createResult = underTest.CreateToken(request);
            Assert.NotNull(createResult.jwtToken);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.jwtToken);
            Assert.NotNull(receivedToken);
            TokenEntity foundEntity = inMemoryRepo.GetById(receivedToken.Id);
            Assert.NotNull(foundEntity);
            try
            {
                underTest.ValidateEncodedJwt(receivedToken, foundEntity, foundEntity.ProtectedUrl + "dogfood");
            }
            catch (FailedException e)
            {
                Assert.False(true, "Caught unexpected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }

        /// <summary>
        /// don't have anegative validation test mostly because ValidateEncodedJwt() does all the heavy lifting
        /// </summary>
        [Fact]
        public void ValidateToken()
        {
            TokenCreateRequest request = BuildTokenCreateRequest();
            TokenCreateResponse createResult = underTest.CreateToken(request);
            Assert.NotNull(createResult.jwtToken);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.jwtToken);
            Assert.NotNull(receivedToken);
            // create validation request from the data used to create the token
            TokenValidateRequest validateThis = new TokenValidateRequest()
            {
                Version = "1.0",
                JwtToken = createResult.jwtToken,
                ProtectedUrl = request.ProtectedUrl
            };
            try
            {
                TokenValidateResponse response = underTest.ValidateToken(validateThis);
                // jam a context validation into this test also. probably should be broken out into its own test in the future
                Assert.NotNull(response.Context);
            }
            catch (FailedException e)
            {
                Assert.False(true, "Caught unexpected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }
    }
}
