using Newtonsoft.Json;

namespace TokenService.Model
{
    /// <summary>
    /// Added to the top level Request, Response and Entities to tell us the data model version
    /// </summary>
    public interface IDataVersion
    {
        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "modelVersion")]
        string ModelVersion { get; set; }
    }
}
