﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TokenService.Controllers;
using TokenService.Model.Rest;
using TokenService.Repository;
using TokenService.Service;
using Xunit;
using Xunit.Abstractions;

namespace TokenServiceTest.Controllers
{
    public class TokenControllerTest
    {
        /// <summary>
        /// logging support 
        /// </summary>
        private readonly ITestOutputHelper _output;

        private TokenInMemRepository inMemoryRepo;
        private ILogger<TokenOperationsService> serviceLogger;
        private TokenOperationsService serviceUnderTest;
        private ILogger<TokenController> controllerLogger;

        private TokenTestUtils ttu = new TokenTestUtils();



        public TokenControllerTest(ITestOutputHelper output)
        {
            this._output = output;
            serviceLogger = Mock.Of<ILogger<TokenOperationsService>>();
            controllerLogger = Mock.Of<ILogger<TokenController>>();
            inMemoryRepo = new TokenInMemRepository();
            serviceUnderTest = new TokenOperationsService(serviceLogger, inMemoryRepo, ttu.BuildCryptographySettings());
        }


        [Fact]
        public void CreateTokenHappyPath()
        {
            TokenController controller = new TokenController(serviceUnderTest, serviceUnderTest, controllerLogger);
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            CreatedResult result = controller.Create(request) as CreatedResult;
            TokenCreateResponse response = result.Value as TokenCreateResponse;
            Assert.NotNull(response);
            Assert.Equal("1.0", response.Version);
            Assert.NotEmpty(response.JwtToken);
            // shouldn't be any messages
            Assert.Equal(0, response.Messages.Count);
        }

        [Fact]
        public void ValidateTokenHappyPath()
        {
            TokenController controller = new TokenController(serviceUnderTest, serviceUnderTest, controllerLogger);
            TokenCreateRequest request = ttu.BuildTokenCreateRequest();
            CreatedResult result = controller.Create(request) as CreatedResult;
            TokenCreateResponse response = result.Value as TokenCreateResponse;

            // assume CreateTokenHappyPath() validates the create path so now lets run the validate path
            TokenValidateRequest validateThis = ttu.BuildTokenValidateRequest(response.JwtToken, request.ProtectedUrl);
            Assert.Equal(validateThis.JwtToken, response.JwtToken);
            // shouldn't be any messages
            Assert.Equal(0, response.Messages.Count);
        }
    }
}