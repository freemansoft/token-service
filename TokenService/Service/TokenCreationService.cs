using Microsoft.Extensions.Logging;
using TokenService.Model.Entity;
using TokenService.Repository;

namespace TokenService.Service
{
    /// <summary>
    /// Token creation and managment service implementation
    /// </summary>
    public class TokenCreationService : ITokenCreationService
    {

        private readonly ILogger<TokenCreationService> _logger;

        private readonly IRepository<TokenEntity> _repository;

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
    }
}
