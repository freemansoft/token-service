using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenService.Models.Rest;

namespace TokenService.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Token")]
    public class TokenController : Controller
    {

        /// <summary>
        /// POST: api/Token/Generate
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("Generate", Name = "GenerateToken")]
        [ProducesResponseType(typeof(TokenCreateResponse), 200)]
        [ProducesResponseType(typeof(TokenCreateResponse), 400)]
        public IActionResult Post([FromBody]TokenCreateRequest value)
        {
            return BadRequest(new TokenCreateResponse());
        }

        /// <summary>
        /// POST: api/Token/Validate 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("Validate", Name = "ValidateToken")]
        [ProducesResponseType(typeof(TokenValidateResponse), 200)]
        [ProducesResponseType(typeof(TokenValidateResponse), 400)]
        [ProducesResponseType(typeof(TokenValidateResponse), 404)]
        public IActionResult Post([FromBody]string value)
        {
            return NotFound(new TokenCreateResponse());
        }

        /*


                // GET: api/Token
                [HttpGet]
                public IEnumerable<string> Get()
                {
                    return new string[] { "value1", "value2" };
                }

                // GET: api/Token/5
                [HttpGet("{id}", Name = "Get")]
                public string Get(int id)
                {
                    return "value";
                }

                // PUT: api/Token/5
                [HttpPut("{id}")]
                public void Put(int id, [FromBody]string value)
                {
                }

                // DELETE: api/ApiWithActions/5
                [HttpDelete("{id}")]
                public void Delete(int id)
                {
                }
            */
    }
}
