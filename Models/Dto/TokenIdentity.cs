using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TokenService.Model.Dto
{
    /// <summary>
    /// Used to identify a user, often the initiator, the obo user or the validator
    /// </summary>
    public class TokenIdentity : IIdentity
    {
        /// <summary>
        /// constructor injection!
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="username"></param>
        public TokenIdentity(string providerName, string username)
        {
            ProviderName = providerName;
            UserName = username;
        }

        /// <summary>
        /// Identity provider
        /// </summary>
        [JsonProperty(PropertyName = "providerName")]
        public string ProviderName { get; set; }
        /// <summary>
        /// User name set for this Identity definition
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "userName", Required = Required.Always)]
        public string UserName { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
