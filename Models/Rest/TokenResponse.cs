﻿using Newtonsoft.Json;

namespace TokenService.Model.Rest
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
        public TokenResponse(TokenMessage[] messages)
        {
            Messages = messages;
        }

        /// <summary>
        /// Primarily exists to support throwing exceptions with empty response or for unit testing.
        /// </summary>
        public TokenResponse() : this( new TokenMessage[0])
        {

        }

        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        /// Diagnostic messages
        /// </summary>
        [JsonProperty(PropertyName = "messages", NullValueHandling = NullValueHandling.Ignore)]
        public IResponseMessage[] Messages { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
