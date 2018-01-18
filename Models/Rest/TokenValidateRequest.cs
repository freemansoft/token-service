using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TokenService.Model.Rest
{
    public class TokenValidateRequest : IDataVersion, IValidatableObject
    {
        /// <summary>
        /// Zero argument constructor.
        /// Set only the version number. All other properties must be set by creator!
        /// </summary>
        public TokenValidateRequest()
        {
            Version = "1.0";
        }

        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        /// The jwt tokent to be added to URL or stuffed in headers when using external entity uses token
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "jwtToken")]
        public string JwtToken { get; set; }

        /// <summary>
        /// URL that this token is issued for.  The protected resource 
        /// </summary>
        [Required]
        [Url]
        [JsonProperty(PropertyName = "protectedUrl")]
        public string ProtectedUrl { get; set; }

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
