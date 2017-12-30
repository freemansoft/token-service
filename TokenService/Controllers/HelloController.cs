using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TokenService.Model.Rest;

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
            return Ok(new TokenResponse
            {
                Version = "1.0",
                Messages = new TokenMessage[]
                {
                    new TokenMessage
                    {
                        Message = "You can find Swagger documentation on this service at endpoint /swagger"
                    },
                    new TokenMessage
                    {
                        Message = "Application code can be found on GitHub https://github.com/freemansoft/token-service"
                    }

                }
            });
        }

    }
}
