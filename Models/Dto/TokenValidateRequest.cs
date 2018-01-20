using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TokenService.Model.Dto
{
    public class TokenValidateRequest : IDataVersion, IValidatableObject
    {
        /// <summary>
        /// Zero argument constructor.
        /// Set only the version number. All other properties must be set by creator!
        /// </summary>
        public TokenValidateRequest()
        {
            ModelVersion = "1.0";
        }

        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "modelVersion")]
        public string ModelVersion { get; set; }

        /// <summary>
        /// The jwt tokent to be added to URL or stuffed in headers when using external entity uses token
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "jwtToken")]
        public string JwtToken { get; set; }

        /// <summary>
        /// Resource for which the client presented the token for validation.
        /// The system will match this resource against the registered protected resource if not null.
        /// The system will ignore this value and the protected resource if this value is not present, null or empty.
        /// </summary>
        [RegularExpression(pattern: "^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\\?([^#]*))?(#(.*))?")] // RFC 2396
        [JsonProperty(PropertyName = "accessedResource")]
        public string AccessedResource { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

        /// <summary>
        /// For future customization?
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

    }
}
