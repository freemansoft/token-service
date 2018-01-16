using Newtonsoft.Json;
using System.Collections.Generic;

namespace TokenService.Model.Rest
{
    /// <summary>
    /// The response object for TokenCreateRequest
    /// </summary>
    public class TokenCreateResponse : TokenResponse
    {
        /// <summary>
        /// This constructor is required so that the JSON serializer knows which concreate class to use for a proprty declared as an interface
        /// <a href="https://stackoverflow.com/questions/5780888/casting-interfaces-for-deserialization-in-json-net">from stackoverflow</a>
        /// Another option is to use a converter
        /// </summary>
        /// <param name="messages"></param>
        [JsonConstructor]
        public TokenCreateResponse(List<TokenResponseMessage> messages) : base(messages)
        {
        }

        /// <summary>
        /// Primarily exists to support throwing exceptions with empty response or for unit testing.
        /// </summary>
        public TokenCreateResponse() : this(new List<TokenResponseMessage>())
        {

        }

        /// <summary>
        /// The jwt token to be added to URL or stuffed in headers when using external entity uses token
        /// </summary>
        [JsonProperty(PropertyName = "jwtToken")]
        public string JwtToken { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
