using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenService.Services
{
    /// <summary>
    /// Token validation service implementation
    /// </summary>

    public class TokenValidationService : ITokenValidationService
    {
        private readonly ILogger<TokenValidationService> _logger;

        /// <summary>
        /// Constructor for Dependency injection
        /// </summary>
        /// <param name="logger"></param>
        public TokenValidationService(ILogger<TokenValidationService> logger)
        {
            _logger = logger;
        }
    }
}
