using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TokenService.Model.Rest
{
    /// <summary>
    /// The root class for token creation requests.
    /// </summary>
    public class TokenCreateRequest : IDataVersion, IValidatableObject
    {
        /// <summary>
        /// This constructor is required so that the JSON serializer knows which concreate class to use for a proprty declared as an interface
        /// <a href="https://stackoverflow.com/questions/5780888/casting-interfaces-for-deserialization-in-json-net">from stackoverflow</a>
        /// Another option is to use a converter
        /// </summary>
        /// <param name="onBehalfOf"></param>
        [JsonConstructor]
        public TokenCreateRequest(TokenIdentity onBehalfOf)
        {
            this.OnBehalfOf = onBehalfOf;
        }

        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
        /// <summary>
        /// URL that this token is issued for.  The protected resource.
        /// The <i>aud</i> in the JWT
        /// </summary>
        [Required]
        [Url]
        [JsonProperty(PropertyName = "protectedUrl")]
        public string ProtectedUrl { get; set; }
        /// <summary>
        /// The token inititiation request is on behalf of the expected token user
        /// The <i>sub</i> in the JWT
        /// </summary>
        [JsonProperty(PropertyName = "onBehalfOf")]
        public IIdentity OnBehalfOf { get; set; }
        /// <summary>
        /// The maximum number of times this token can be used. The default is 1.
        /// </summary>
        [JsonProperty(PropertyName = "maxUsageCount")]
        public int MaxUsageCount { get; set; } = 1;

        /// <summary>
        /// How long a token is valid.  The default is 300 seconds.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        [JsonProperty(PropertyName = "expirationIntervalSeconds")]
        public int ExpirationIntervalSeconds { get; set; } = 300;
        /// <summary>
        /// When the token is initially valid
        /// The JWT <i>nbf</i>
        /// </summary>
        [JsonProperty(PropertyName = "effectiveTime")]
        public DateTime EffectiveTime { get; set; } = DateTime.Now;
        /// <summary>
        /// Arbitrary valid json that acts as a shared context between token initiator and the validator.
        /// </summary>
        /// 
        [JsonProperty(PropertyName = "context")]
        public JToken Context { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(this, validationContext, results, true);
            return results;
        }
    }
}
