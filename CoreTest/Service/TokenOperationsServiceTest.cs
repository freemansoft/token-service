using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using TokenService.Core.Exception;
using TokenService.Core.Repository;
using TokenService.Core.Service;
using TokenService.Model.Dto;
using TokenService.Model.Entity;
using Xunit;
using Xunit.Abstractions;

namespace CoreTest.Service
{
    public class TokenOperationsServiceTest
    {
        /// <summary>
        /// logging support 
        /// </summary>
        private readonly ITestOutputHelper _output;

        private ILogger<IRepository<TokenEntity>> repositoryLogger;
        private IRepository<TokenEntity> inMemoryRepo;
        private ILogger<TokenOperationsService> serviceLogger;
        private TokenOperationsService serviceUnderTest;

        private TokenTestUtils ttu = new TokenTestUtils();


        public TokenOperationsServiceTest(ITestOutputHelper output)
        {
            this._output = output;
            serviceLogger = Mock.Of<ILogger<TokenOperationsService>>();
            repositoryLogger = Mock.Of<ILogger<IRepository<TokenEntity>>>();

            // oh moma I live in fear of my life from from the long arm of Moq 
            Mock<IRepository<TokenEntity>> someMock = new Mock<IRepository<TokenEntity>>();
            Dictionary<string, TokenEntity> dict = new Dictionary<string, TokenEntity>();
            someMock.Setup(x => x.Create(It.IsAny<TokenEntity>()))
                .Callback((TokenEntity t) =>
                {
                    dict.Add(t.Id, t);
                });
            someMock.Setup(x => x.Update(It.IsAny<TokenEntity>()))
                .Callback((TokenEntity t) =>
                {
                    dict.Remove(t.Id);
                    dict.Add(t.Id, t);
                });
            someMock.Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((string key) =>
                    dict.GetValueOrDefault(key, null));
            someMock.Setup(x => x.Delete(It.IsAny<TokenEntity>()))
                .Callback((TokenEntity t) =>
                {
                    dict.Remove(t.Id);
                });
            inMemoryRepo = someMock.Object;

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
            request.ModelVersion = null;
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
                // TODO should assert all the properties called out
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
                    serviceUnderTest.ValidateEncodedJwt(receivedToken, badEntity);
                    Assert.False(true, "Expected FailedException");
                }
                catch (ConsistencyException e)
                {
                    _output.WriteLine("Caught expected exception: " + e.Message);
                }
            }
            catch (BadArgumentException e)
            {
                Assert.False(true, "Caught unexpected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }

        /// <summary>
        /// Verify completely different url works
        /// </summary>
        [Fact]
        public void ValidateProtectedUrlDifferent()
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
                serviceUnderTest.ValidateResourceAllowed(receivedToken, foundEntity, "http://badurl");
                Assert.False(true, "Expected FailedException");
            }
            catch (ViolationException e)
            {
                _output.WriteLine("Caught expected exception: " + e.Message);
            }
        }

        /// <summary>
        /// This was intended to be part of regex based tests where suffixes or paths were allowed based on protectedResource
        /// </summary>
        [Fact]
        public void ValidateProtectedExtraSuffixFails()
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
                serviceUnderTest.ValidateResourceAllowed(receivedToken, foundEntity, foundEntity.ProtectedResource + "/some-suffix");
                Assert.False(true, "Expected FailedException");
            }
            catch (ViolationException e)
            {
                _output.WriteLine("Caught expected exception: " + e.Message);
            }
        }

        /// <summary>
        /// feature not yet implemented
        /// This test will validate that the encryption signature comparison is working correctly
        /// </summary>
        [Fact(Skip = "not yet implemented")] // Xunit 2.x https://xunit.github.io/docs/comparisons.html
        public void ValidateEncodedJwtBadSignature()
        {
            // ValidateTokenSignature(string signedToken, TokenEntity jwtToken)
        }

        /// <summary>
        /// Exact match
        /// </summary>
        [Fact]
        public void ValidateProtectedExactMatch()
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
                serviceUnderTest.ValidateResourceAllowed(receivedToken, foundEntity, foundEntity.ProtectedResource);
            }
            catch (ViolationException e)
            {
                Assert.False(true, "Caught unexpected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }

        /// <summary>
        /// No accessed provided
        /// </summary>
        [Fact]
        public void ValidateProtectedNoAccessed()
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
                serviceUnderTest.ValidateResourceAllowed(receivedToken, foundEntity, null);
                serviceUnderTest.ValidateResourceAllowed(receivedToken, foundEntity, "");
                serviceUnderTest.ValidateResourceAllowed(receivedToken, foundEntity, "    ");
            }
            catch (ViolationException e)
            {
                Assert.False(true, "Caught unexpected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }

        /// <summary>
        /// <list type="number">
        /// <item>
        ///     <description>Make sure we can pass validation.</description></item>
        /// <item>
        ///     <description>Then fail validation on usage count exceeded</description></item>
        /// </list>
        /// Could be simplified by testing ValidateExpirationPolicy() directly
        /// </summary>
        [Fact]
        public void ValidateTokenCountExceeded()
        {
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            TokenCreateResponse createResult = serviceUnderTest.CreateToken(request);
            Assert.NotNull(createResult.JwtToken);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.JwtToken);
            Assert.NotNull(receivedToken);
            // create validation request from the data used to create the token
            TokenValidateRequest validateThis = ttu.BuildTokenValidateRequest(createResult.JwtToken, request.ProtectedResource);

            // first one should succeed
            TokenValidateResponse response1 = serviceUnderTest.ValidateToken(validateThis);
            // Lets jam a context validation into this test also. probably should be broken out into its own test in the future
            Assert.NotNull(response1.Context);

            try
            {
                // usage count was set to one so should now fail
                TokenValidateResponse response2 = serviceUnderTest.ValidateToken(validateThis);
                // Lets jam a context validation into this test also. probably should be broken out into its own test in the future
                Assert.False(true, "Did not catch exception when usage count exceeded");
            }
            catch (ViolationException e)
            {
                _output.WriteLine("Caught expected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }

        /// <summary>
        /// Fail because token expired.
        /// Could be simplified by testing ValidateExpirationPolicy() directly
        /// </summary>
        [Fact]
        public void ValidateTokenTimeExpired()
        {
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            // make effective and expiration time in the past
            request.EffectiveTime = DateTime.Now.AddDays(-1);
            request.ExpirationIntervalSeconds = 0;

            TokenCreateResponse createResult = serviceUnderTest.CreateToken(request);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.JwtToken);
            // create validation request from the data used to create the token
            TokenValidateRequest validateThis = ttu.BuildTokenValidateRequest(createResult.JwtToken, request.ProtectedResource);
            try
            {
                // should fail as expired
                TokenValidateResponse response = serviceUnderTest.ValidateToken(validateThis);
                Assert.False(true, "Did not catch exception when token expired");
            }
            catch (ViolationException e)
            {
                // TODO should validate the message or something...
                _output.WriteLine("Caught expected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }

        /// <summary>
        /// Fail because token expired.
        /// Could be simplified by testing ValidateExpirationPolicy() directly
        /// </summary>
        [Fact]
        public void ValidateTokenNotEffective()
        {
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            // make effective and expiration time in the past
            request.EffectiveTime = DateTime.Now.AddDays(+20);

            TokenCreateResponse createResult = serviceUnderTest.CreateToken(request);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken receivedToken = tokenHandler.ReadJwtToken(createResult.JwtToken);
            // create validation request from the data used to create the token
            TokenValidateRequest validateThis = ttu.BuildTokenValidateRequest(createResult.JwtToken, request.ProtectedResource);
            try
            {
                // should fail as expired
                TokenValidateResponse response = serviceUnderTest.ValidateToken(validateThis);
                Assert.False(true, "Did not catch exception when token not yet effective");
            }
            catch (ViolationException e)
            {
                // TODO should validate the message or something...
                _output.WriteLine("Caught expected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }

        [Fact]
        public void ExpirationPolicyValid()
        {
            TokenEntity anEntity = new TokenEntity()
            {
                // TODO validate the actual exception
            };
            serviceUnderTest.ValidateExpirationPolicy(anEntity);
        }

        [Fact]
        public void ExpirationPolicyExpiredTime()
        {
            TokenEntity anEntity = new TokenEntity()
            {
                InitiationTime = DateTime.Now.AddDays(-2.0),
                ExpirationTime = DateTime.Now.AddDays(-1.0),
                MaxUseCount = int.MaxValue,
                CurrentUseCount = 0,
            };
            try
            {
                serviceUnderTest.ValidateExpirationPolicy(anEntity);
                Assert.False(true, "Should have failed due to time based expiration");
            }
            catch (ViolationException e)
            {
                // TODO validate the actual exception
                _output.WriteLine("Caught expected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }

        [Fact]
        public void ExpirationPolicyExceededCount()
        {
            TokenEntity anEntity = new TokenEntity()
            {
                InitiationTime = DateTime.Now.AddDays(-2.0),
                ExpirationTime = DateTime.Now.AddDays(1.0),
                MaxUseCount = 1,
                CurrentUseCount = 1,
            };
            try
            {
                serviceUnderTest.ValidateExpirationPolicy(anEntity);
                Assert.False(true, "Should have failed due exceeded usage count");
            }
            catch (ViolationException e)
            {
                _output.WriteLine("Caught expected exception: " + e.Message + " " + e.ServiceResponse);
            }
        }

    }
}
