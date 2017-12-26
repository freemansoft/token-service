using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TokenService.Models.Rest;

namespace TokenService.Controllers
{
    /// <summary>
    /// Act as a home page.  What should actually go here?
    /// </summary>
    [Route("/")]
    public class HelloController : Controller
    {
        /// <summary>
        /// This should return a link to the swagger docs or a list of hyper media URI
        /// </summary>
        /// <returns>TokenResponse</returns>
        [HttpGet]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        public IActionResult Get()
        {
            return Ok(new TokenResponse
            {
                Version = "1.0",
                messages = new TokenMessage[]
                {
                    new TokenMessage
                    {
                        message = "You can find Swagger documentation at ..."
                    },
                    new TokenMessage
                    {
                        message = "This application example can be found on GitHub https://github.com/freemansoft/token-service"
                    }

                }
            });
        }

    }
}
