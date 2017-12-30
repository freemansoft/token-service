using Newtonsoft.Json;

namespace TokenService.Model
{
    /// <summary>
    /// Added to the top level Request, Response and Entities to tell us the data model version
    /// </summary>
    public interface IResponseMessages
    {
        /// <summary>
        /// Diagnostic messages
        /// </summary>
        [JsonProperty(PropertyName = "messages", NullValueHandling = NullValueHandling.Ignore)]
        IResponseMessage[] Messages { get; set; }
    }
}
