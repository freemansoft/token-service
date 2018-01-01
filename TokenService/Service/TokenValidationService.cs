﻿using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TokenService.Exception;
using TokenService.Model.Entity;
using TokenService.Model.Rest;
using TokenService.Repository;

namespace TokenService.Service
{
    /// <summary>
    /// Token validation service implementation
    /// </summary>
    public class TokenValidationService : ITokenValidationService
    {
        private readonly ILogger<TokenValidationService> _logger;

        private readonly IRepository<TokenEntity> _repository;

        /// <summary>
        /// Constructor for Dependency injection
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        public TokenValidationService(ILogger<TokenValidationService> logger, IRepository<TokenEntity> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Validates the passed in token should be honored
        /// Returns the response.  Throws an exception containing a response if it fails
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TokenValidateResponse ValidateToken(TokenValidateRequest request)
        {
            ValidateRequest(request);
            return null;
        }

        //#pragma warning disable IDE0017
        /// <summary>
        /// throws CreateBadArgumentException if there is a problem with the token.
        /// </summary>
        /// <param name="request"></param>
        private void ValidateRequest(TokenValidateRequest request)
        {
            ValidationContext context = new ValidationContext(request, null, null);
            IEnumerable<ValidationResult> results = request.Validate(context);
            List<TokenMessage> messages = new List<TokenMessage>();
            foreach (ValidationResult oneResult in results)
            {
                TokenMessage message = new TokenMessage(null, oneResult.ErrorMessage);
                messages.Add(message);
            }
            if (messages.Count > 0)
            {
                throw new CreateBadArgumentException("Failed Object Validation", new TokenResponse(messages));
            }
        }

    }
}