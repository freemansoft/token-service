using Newtonsoft.Json;

namespace TokenService.Model.Entity
{
    public class TokenIdentityEntity : IIdentity
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

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
