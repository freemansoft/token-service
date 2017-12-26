using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenService.Models.Rest
{
    /// <summary>
    /// The root class for token creation requests.
    /// </summary>
    public class TokenCreateRequest : IDataVersion
    {
        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
        /// <summary>
        /// URL that this token is issued for.  The protected resource 
        /// </summary>
        public string protectedUrl;
        /// <summary>
        /// The token inititiation request is on behalf of the expected token user
        /// </summary>
        public TokenIdentity onBehalfOf;
        /// <summary>
        /// The maximum number of times this token can be used. The default is 1.
        /// </summary>
        public int maxUsageCount = 1;
        /// <summary>
        /// How long a token is valid.  The default is 300 seconds.
        /// </summary>
        public int expirationIntervalSeconds = 300;
        /// <summary>
        /// Arbitrary valid json that acts as a shared context between token initiator and the validator.
        /// </summary>
        public JToken context;
    }
}
