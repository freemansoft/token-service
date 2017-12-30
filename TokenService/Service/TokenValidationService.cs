using Microsoft.Extensions.Logging;
using TokenService.Model.Entity;
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
    }
}
