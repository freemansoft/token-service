using Newtonsoft.Json;

namespace TokenService.Model.Entity
{
    public class TokenIdentityEntity : IIdentity
    {
        /// <summary>
        /// constructor injection!
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="username"></param>
        public TokenIdentityEntity(string providerName, string username)
        {
            ProviderName = providerName;
            UserName = username;
        }


        /// <summary>
        /// Identity provider
        /// </summary>
        [JsonProperty(PropertyName = "providerName")]
        public string ProviderName { get; private set; }
        /// <summary>
        /// User name set for this Identity definition
        /// </summary>
        [JsonProperty(PropertyName = "userName", Required = Required.Always)]
        public string UserName { get; private set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
