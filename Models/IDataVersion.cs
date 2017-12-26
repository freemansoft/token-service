using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenService.Models
{
    /// <summary>
    /// Added to the top level Request, Response and Entities to tell us the data model version
    /// </summary>
    public interface IDataVersion
    {
        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        string Version { get; set; }
    }
}
