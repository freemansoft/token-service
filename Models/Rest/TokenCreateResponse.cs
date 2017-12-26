using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenService.Models.Rest
{
    /// <summary>
    /// The response object for TokenCreateRequest
    /// </summary>
    public class TokenCreateResponse: TokenResponse
    {
        /// <summary>
        /// The jwt tokent to be added to URL or stuffed in headers when using external entity uses token
        /// </summary>
        public string jwt;
    }
}
