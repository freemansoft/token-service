using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenService.Models.Rest
{
    /// <summary>
    /// Used to identify a user, often the initiator, the obo user or the validator
    /// </summary>
    public class TokenIdentity : IIdentityInfo
    {
        /// <summary>
        /// Identity provider
        /// </summary>
        [JsonProperty(PropertyName = "providerName")]
        public string ProviderName { get; set; }
        /// <summary>
        /// User name set for this Identity definition
        /// </summary>
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
    }
}
