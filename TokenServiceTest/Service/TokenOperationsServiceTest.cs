using Microsoft.Extensions.Logging;
using Moq;
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

        private TokenInMemRepository inMemoryRepo;
        private ILogger<TokenOperationsService> serviceLogger;
        private TokenOperationsService serviceUnderTest;

        private TokenTestUtils ttu = new TokenTestUtils();


        public TokenOperationsServiceTest(ITestOutputHelper output)
        {
            this._output = output;
            serviceLogger = Mock.Of<ILogger<TokenOperationsService>>();
            inMemoryRepo = new TokenInMemRepository();
            serviceUnderTest = new TokenOperationsService(serviceLogger, inMemoryRepo, ttu.BuildCryptographySettings());

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
                TokenCreateResponse response = serviceUnderTest.CreateToken(request);
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
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            request.Version = null;
            try
            {
                TokenCreateResponse createResult = serviceUnderTest.CreateToken(request);
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
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            TokenCreateResponse createResult = serviceUnderTest.CreateToken(request);
            Assert.NotNull(createResult);
            Assert.NotNull(createResult.JwtToken);
            _output.WriteLine("Calculated Token Encoded : " + createResult.JwtToken);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken foundToken = tokenHandler.ReadJwtToken(createResult.JwtToken);
            Assert.NotNull(foundToken);
            Assert.Equal(ttu.bogusTestUrl, foundToken.Payload.Sub);
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
                TokenValidateResponse response = serviceUnderTest.ValidateToken(request);
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
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            // added this catch block in when we failed creating tokens because regex didn't fit in URL validation.
            // The tangled web we weave
            try
            {
                TokenCreateResponse createResult = serviceUnderTest.CreateToken(request);

                Assert.NotNull(createResult.JwtToken);
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.JwtToken);
                TokenEntity badEntity = new TokenEntity(null, null)
                {
                    JwtUniqueIdentifier = "dogfood"
                };
                try
                {
                    serviceUnderTest.ValidateEncodedJwt(receivedToken, badEntity, badEntity.ProtectedUrl);
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
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            TokenCreateResponse createResult = serviceUnderTest.CreateToken(request);
            Assert.NotNull(createResult.JwtToken);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.JwtToken);
            Assert.NotNull(receivedToken);
            TokenEntity foundEntity = inMemoryRepo.GetById(receivedToken.Id);
            Assert.NotNull(foundEntity);
            try
            {
                serviceUnderTest.ValidateEncodedJwt(receivedToken, foundEntity, "http://badurl");
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
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            TokenCreateResponse createResult = serviceUnderTest.CreateToken(request);
            Assert.NotNull(createResult.JwtToken);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.JwtToken);
            Assert.NotNull(receivedToken);
            TokenEntity foundEntity = inMemoryRepo.GetById(receivedToken.Id);
            Assert.NotNull(foundEntity);
            try
            {
                serviceUnderTest.ValidateEncodedJwt(receivedToken, foundEntity, "x" + foundEntity.ProtectedUrl);
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
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            TokenCreateResponse createResult = serviceUnderTest.CreateToken(request);
            Assert.NotNull(createResult.JwtToken);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.JwtToken);
            Assert.NotNull(receivedToken);
            TokenEntity foundEntity = inMemoryRepo.GetById(receivedToken.Id);
            Assert.NotNull(foundEntity);
            try
            {
                serviceUnderTest.ValidateEncodedJwt(receivedToken, foundEntity, foundEntity.ProtectedUrl + "dogfood");
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
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            TokenCreateResponse createResult = serviceUnderTest.CreateToken(request);
            Assert.NotNull(createResult.JwtToken);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.JwtToken);
            Assert.NotNull(receivedToken);
            // create validation request from the data used to create the token
            TokenValidateRequest validateThis = ttu.BuildTokenValidateRequest(createResult.JwtToken, request.ProtectedUrl);
            try
            {
                TokenValidateResponse response = serviceUnderTest.ValidateToken(validateThis);
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
