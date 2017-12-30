using Newtonsoft.Json;

namespace TokenService.Model
{
    public interface IIdentity
    {
        /// <summary>
        /// Identity provider. should configure JSON serializaton for providerName
        /// </summary>
        [JsonProperty(PropertyName = "providerName")]
        string ProviderName { get; set; }
        /// <summary>
        /// user name. should configure JSON serializaton for userName
        /// </summary>
        [JsonProperty(PropertyName = "userName")]
        string UserName { get; set; }
    }
}
