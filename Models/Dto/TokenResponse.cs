using Newtonsoft.Json;
using System.Collections.Generic;

namespace TokenService.Model.Dto
{
    /// <summary>
    /// Standard response.
    /// Responses are based on this.  Services will generally extend this class to add custom attributes..
    /// </summary>
    public class TokenResponse : IResponse
    {
        /// <summary>
        /// This constructor is required so that the JSON serializer knows which concreate class to use for a proprty declared as an interface
        /// <a href="https://stackoverflow.com/questions/5780888/casting-interfaces-for-deserialization-in-json-net">from stackoverflow</a>
        /// Another option is to use a converter
        /// </summary>
        /// <param name="messages"></param>
        [JsonConstructor]
        public TokenResponse(List<TokenResponseMessage> messages)
        {
            Messages = new List<IResponseMessage>(messages);
            ModelVersion = "1.0";
        }

        /// <summary>
        /// Primarily exists to support throwing exceptions with empty response or for unit testing.
        /// </summary>
        public TokenResponse() : this(new List<TokenResponseMessage>())
        {

        }

        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "modelVersion", Required = Required.Always)]
        public string ModelVersion { get; set; }

        /// <summary>
        /// Diagnostic messages
        /// </summary>
        [JsonProperty(PropertyName = "messages", NullValueHandling = NullValueHandling.Ignore)]
        public IList<IResponseMessage> Messages { get; private set; } = new List<IResponseMessage>();

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
