using Microsoft.AspNetCore.Mvc;
using TokenService.Models.Rest;

namespace TokenService.Controllers
{
    [Route("/")]
    public class HelloController : Controller
    {
        private const string MESSAGE_FORMAT = "Hello {0}!";

        [HttpGet]
        public JsonResult Get([FromQuery] string name = "World")
        {
            return Json(new HelloResponse
            {
                greeting = string.Format(MESSAGE_FORMAT, name)
            });
        }

        [HttpPost]
        public JsonResult Post([FromBody]string name = "World")
        {
            return Json(new HelloResponse
            {
                greeting = string.Format(MESSAGE_FORMAT, name)
            });
        }
    }
}
