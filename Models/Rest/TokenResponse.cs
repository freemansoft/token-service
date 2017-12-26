using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenService.Models.Rest
{
    /// <summary>
    /// Standard response.
    /// Responses are based on this.  Services will generally extend this class to add custom attributes..
    /// </summary>
    public class TokenResponse : IDataVersion
    {
        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
        /// <summary>
        /// Diagnostic messages
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TokenMessage[] messages;
    }
}
