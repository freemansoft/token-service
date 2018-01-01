using Newtonsoft.Json;
using System.Collections.Generic;

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
        IList<IResponseMessage> Messages { get; }
    }
}
