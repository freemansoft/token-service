using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TokenService.Model.Rest
{
    public class TokenValidateResponse: TokenResponse
    {
        /// <summary>
        /// This constructor is required so that the JSON serializer knows which concreate class to use for a proprty declared as an interface
        /// <a href="https://stackoverflow.com/questions/5780888/casting-interfaces-for-deserialization-in-json-net">from stackoverflow</a>
        /// Another option is to use a converter
        /// </summary>
        /// <param name="messages"></param>
        [JsonConstructor]
        public TokenValidateResponse(TokenMessage[] messages) : base(messages)
        {
        }

        /// <summary>
        /// Primarily exists to support throwing exceptions with empty response or for unit testing.
        /// </summary>
        public TokenValidateResponse() : this(new TokenMessage[0])
        {

        }

        /// <summary>
        /// Arbitrary valid json that acts as a shared context between token initiator and the validator.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JToken context;
    }
}
