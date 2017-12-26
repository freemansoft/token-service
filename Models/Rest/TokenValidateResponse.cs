using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenService.Models.Rest
{
    class TokenValidateResponse: TokenResponse
    {
        /// <summary>
        /// Arbitrary valid json that acts as a shared context between token initiator and the validator.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JToken context;
    }
}
