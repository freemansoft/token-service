using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TokenService.Model.Rest
{
    /// <summary>
    /// Used to identify a user, often the initiator, the obo user or the validator
    /// </summary>
    public class TokenIdentity : IIdentity
    {
        /// <summary>
        /// Identity provider
        /// </summary>
        [JsonProperty(PropertyName = "providerName")]
        public string ProviderName { get; set; }
        /// <summary>
        /// User name set for this Identity definition
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
