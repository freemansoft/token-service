using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TokenService.Core.Exception;
using TokenService.Model.Dto;
using TokenService.Core.Service;

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

        private readonly ITokenOperationsService _creationService;
        private readonly ITokenOperationsService _validationService;

        /// <summary>
        /// constructor based injection
        /// </summary>
        /// <param name="creationService"></param>
        /// <param name="validationService"></param>
        /// <param name="logger"></param>
        public TokenController(ITokenOperationsService creationService, ITokenOperationsService validationService, ILogger<TokenController> logger)
        {
            _creationService = creationService;
            _validationService = validationService;
            _logger = logger;
        }

        /// <summary>
        /// Generate a Token.
        /// </summary>
        /// <remarks>POST api/v1/Token/Generate</remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created token</response>
        /// <response code="400">Token not created</response>
        /// <response code="500">Internal configuration error</response>
        [HttpPost("Generate", Name = "GenerateToken")]
        [ProducesResponseType(typeof(TokenCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ConsistencyException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BadArgumentException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ConfigurationException), StatusCodes.Status500InternalServerError)]
        public IActionResult Create([FromBody]TokenCreateRequest value)
        {
            TokenCreateResponse response = _creationService.CreateToken(value);
            return Created("../Validate", response);
        }

        /// <summary>
        /// Validate a Token
        /// </summary>
        /// <remarks>POST: api/v1/Token/Validate </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <response code="200">Token was validated successfully</response>
        /// <response code="400">Some problem processing requests or token mismatch</response>
        /// <response code="404">Token does not exist in data store</response>
        /// <response code="409">Token is invalid due to expiration or effectivity</response>
        /// <response code="500">Internal configuration error</response>
        [HttpPost("Validate", Name = "ValidateToken")]
        [ProducesResponseType(typeof(TokenValidateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadArgumentException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ConsistencyException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundException), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ViolationException), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ConfigurationException), StatusCodes.Status500InternalServerError)]
        public IActionResult Validate([FromBody]TokenValidateRequest value)
        {
            TokenValidateResponse response = _validationService.ValidateToken(value);
            return Ok(response);
        }

    }
}
