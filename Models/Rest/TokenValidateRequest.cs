using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TokenService.Model.Rest
{
    public class TokenValidateRequest: IDataVersion
    {
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
        [JsonProperty(PropertyName = "jwt")]
        public string Jwt { get; set;  }

        /// <summary>
        /// URL that this token is issued for.  The protected resource 
        /// </summary>
        [Required]
        [Url]
        [JsonProperty(PropertyName = "protectedUrl")]
        public string ProtectedUrl { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
