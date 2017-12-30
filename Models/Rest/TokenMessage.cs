using Newtonsoft.Json;

namespace TokenService.Model.Rest
{
    public class TokenMessage : IResponseMessage
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
