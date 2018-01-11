using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// BadArgumentException if the request is bad
        /// FailedException if there was some other problem
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TokenValidateResponse ValidateToken(TokenValidateRequest request)
        {
            ValidateRequest(request);
            return null;
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

    }
}
