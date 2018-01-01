using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TokenService.Model.Rest
{
    public class TokenMessage : IResponseMessage
    {
        /// <summary>
        /// for serialization
        /// </summary>
        public TokenMessage()
        {

        }

        /// <summary>
        /// convenience constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public TokenMessage(string id, string message)
        {
            Id = id;
            Message = message;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// text message
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
