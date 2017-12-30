using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TokenService.Model.Rest
{
    /// <summary>
    /// The root class for token creation requests.
    /// </summary>
    public class TokenCreateRequest : IDataVersion
    {
        /// <summary>
        /// This constructor is required so that the JSON serializer knows which concreate class to use for a proprty declared as an interface
        /// <a href="https://stackoverflow.com/questions/5780888/casting-interfaces-for-deserialization-in-json-net">from stackoverflow</a>
        /// Another option is to use a converter
        /// </summary>
        /// <param name="onBehalfOf"></param>
        [JsonConstructor]
        public TokenCreateRequest(TokenIdentity onBehalfOf)
        {
            this.onBehalfOf = onBehalfOf;
        }

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
        public IIdentity onBehalfOf;
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
