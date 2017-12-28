using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TokenService.Models.Rest;
using TokenService.Services;

namespace TokenService.Controllers
{
    /// <summary>
    /// Main entry point for token service
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/Token")]
    public class TokenController : Controller
    {

        private readonly ILogger<TokenController> _logger;

        private readonly ITokenCreationService _creationService;
        private readonly ITokenValidationService _validationService;

        /// <summary>
        /// constructor based injection
        /// </summary>
        /// <param name="creationService"></param>
        /// <param name="validationService"></param>
        /// <param name="logger"></param>
        public TokenController(ITokenCreationService creationService, ITokenValidationService validationService, ILogger<TokenController> logger)
        {
            _creationService = creationService;
            _validationService = validationService;
            _logger = logger;
        }

        /// <summary>
        /// Generate a Token.
        /// </summary>
        /// <remarks>POST api/Token/Generate</remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created token</response>
        /// <response code="400">Token not created</response>
        [HttpPost("Generate", Name = "GenerateToken")]
        [ProducesResponseType(typeof(TokenCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(TokenCreateResponse), StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody]TokenCreateRequest value)
        {
            return BadRequest(new TokenCreateResponse());
        }

        /// <summary>
        /// Validate a Token
        /// </summary>
        /// <remarks>POST: api/Token/Validate </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <response code="200">Token was validated successfully</response>
        /// <response code="400">Some problem processing requests</response>
        /// <response code="404">Token does not exist in data store</response>
        /// <response code="409">Token is invalid</response>
        [HttpPost("Validate", Name = "ValidateToken")]
        [ProducesResponseType(typeof(TokenValidateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TokenValidateResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TokenValidateResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(TokenValidateResponse), StatusCodes.Status409Conflict)]
        public IActionResult Post([FromBody]TokenValidateRequest value)
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
