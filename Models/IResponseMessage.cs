using Newtonsoft.Json;

namespace TokenService.Model
{
    /// <summary>
    /// Added to the top level Request, Response and Entities to tell us the data model version.
    /// Used when more than a data model is required to communicate to caller
    /// </summary>
    public interface IResponseMessage
    {
        /// <summary>
        /// Machine readable identifier for each message
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        string Id { get; }
        /// <summary>
        /// Human readable message
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        string Message { get; }
    }
}
