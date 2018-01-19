using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TokenService.Model.Dto;

namespace TokenService.Controllers
{
    /// <summary>
    /// Act as a home page.  What should actually go here?
    /// </summary>
    [Route("/")]
    public class HelloController : Controller
    {

        private readonly ILogger<HelloController> _logger;

        /// <summary>
        /// Constructor for Dependency Injection
        /// </summary>
        /// <param name="logger"></param>
        public HelloController(ILogger<HelloController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This should return a link to the swagger docs or a list of hyper media URI
        /// </summary>
        /// <returns>TokenResponse</returns>
        [HttpGet]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        public IActionResult Get()
        {
            _logger.LogDebug("Returning the hello message :)");
            List<TokenResponseMessage> messages = new List<TokenResponseMessage>(new TokenResponseMessage[] {
                new TokenResponseMessage(null, "You can find Swagger documentation on this service at endpoint /swagger"),
                new TokenResponseMessage(null, "Application code can be found on GitHub https://github.com/freemansoft/token-service")
            });
            return Ok(new TokenResponse(messages)
            {
            });
        }

    }
}
