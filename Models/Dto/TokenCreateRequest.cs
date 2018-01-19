using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TokenService.Model.Dto
{
    /// <summary>
    /// The root class for token creation requests.
    /// </summary>
    public class TokenCreateRequest : IDataVersion, IValidatableObject
    {
        public TokenCreateRequest() : this(null)
        {

        }

        /// <summary>
        /// This constructor is required so that the JSON serializer knows which concreate class to use for a proprty declared as an interface
        /// <a href="https://stackoverflow.com/questions/5780888/casting-interfaces-for-deserialization-in-json-net">from stackoverflow</a>
        /// Another option is to use a converter.
        /// <para></para>
        /// Set only some default properties. Should we set none and force all to be set with no defaults?
        /// </summary>
        /// <param name="onBehalfOf"></param>
        [JsonConstructor]
        public TokenCreateRequest(TokenIdentity onBehalfOf)
        {
            this.OnBehalfOf = onBehalfOf;
            ModelVersion = "1.0";
            MaxUsageCount = 1;
            EffectiveTime = DateTime.Now;
            ExpirationIntervalSeconds = int.MaxValue;
        }

        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "modelVersion", Required = Required.Always)]
        public string ModelVersion { get; set; }
        /// <summary>
        /// URL that this token is issued for.  The protected resource. 
        /// Matched with Regex "^"+ProtectedUrl
        /// The <i>sub</i> in the JWT
        /// </summary>
        [Required]
        [Url]
        [JsonProperty(PropertyName = "protectedUrl", Required = Required.Always)]
        public string ProtectedUrl { get; set; }
        /// <summary>
        /// The token inititiation request is on behalf of the expected token user
        /// The <i>aud</i> in the JWT
        /// </summary>
        [JsonProperty(PropertyName = "onBehalfOf")]
        public IIdentity OnBehalfOf { get; set; }
        /// <summary>
        /// The maximum number of times this token can be used. The default is 1.
        /// </summary>
        [Range(1, int.MaxValue)]
        [JsonProperty(PropertyName = "maxUsageCount")]
        public int MaxUsageCount { get; set; }

        /// <summary>
        /// How long a token is valid. Range 0..MaxValue 
        /// TokenService calculates expiration date/time adding this to DateTime.Now;
        /// The default is int.MaxValue seconds so can be marked "Required"
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        [JsonProperty(PropertyName = "expirationIntervalSeconds")]
        public int ExpirationIntervalSeconds { get; set; }
        /// <summary>
        /// When the token is initially valid. Defaults to "Now" so can be marked "Required"
        /// The JWT <i>nbf</i>
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "effectiveTime")]
        public DateTime EffectiveTime { get; set; }
        /// <summary>
        /// Arbitrary valid json that acts as a shared context between token initiator and the validator.
        /// </summary>
        /// 
        [JsonProperty(PropertyName = "context", Required = Required.DisallowNull)]
        public JToken Context { get; set; }

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
