using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenService.Models.Rest
{
    public class TokenValidateRequest: IDataVersion
    {
        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        /// The jwt tokent to be added to URL or stuffed in headers when using external entity uses token
        /// </summary>
        public string jwt;

        /// <summary>
        /// URL that this token is issued for.  The protected resource 
        /// </summary>
        public string protectedUrl;

    }
}
