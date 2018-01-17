using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace TokenService.Model.Entity
{
    /// <summary>
    /// Root object for token information stored in a persistent store. This token's services root model object
    /// </summary>
    public class TokenEntity : IDataVersion, IHasId
    {
        /// <summary>
        /// This constructor is required so that the JSON serializer knows which concreate class to use for a proprty declared as an interface
        /// <a href="https://stackoverflow.com/questions/5780888/casting-interfaces-for-deserialization-in-json-net">from stackoverflow</a>
        /// Another option is to use a converter
        /// Defaults
        /// InitiationTime = now
        /// </summary>
        /// <param name="onBehalfOf"></param>
        /// <param name="initiator"></param>
        /// <param name="consumedBy"></param>
        [JsonConstructor]
        public TokenEntity(TokenIdentityEntity onBehalfOf, TokenIdentityEntity initiator, TokenIdentityEntity[] consumedBy)
        {
            if (onBehalfOf != null)
            {
                this.OnBehalfOf = onBehalfOf;
            }
            if (initiator != null)
            {
                this.Initiator = initiator;
            }
            if (consumedBy != null)
            {
                this.ConsumedBy = consumedBy;
            }
            Version = "1.0";
            ExpirationIntervalSec = 300;
            InitiationTime = DateTime.Now;
            EffectiveTime = InitiationTime;
            ExpirationTime = InitiationTime.AddSeconds(ExpirationIntervalSec);
            CurrentUseCount = 0;
            MaxUseCount = int.MaxValue;

        }

        /// <summary>
        /// Primary constructor used by services.
        /// </summary>
        /// <param name="onBehalfOf"></param>
        /// <param name="initiator"></param>
        public TokenEntity(TokenIdentityEntity onBehalfOf, TokenIdentityEntity initiator) : this(onBehalfOf, initiator, null)
        {
        }

        /// <summary>
        /// Why did I create this constructor?
        /// </summary>
        public TokenEntity() : this(null, null)
        {

        }


        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "version", Required = Required.Always)]
        public string Version { get; set; }
        /// <summary>
        /// url protected by this token.
        /// Matched with Regex "^"+ProtectedUrl
        /// The JWT <i>sub</i>
        /// </summary>
        [JsonProperty(PropertyName = "protectedUrl")]
        public string ProtectedUrl { get; set; }
        /// <summary>
        /// the JWT given to the user to be presented to the consuming system and passed to the validate call()
        /// </summary>
        [JsonProperty(PropertyName = "jwtToken")]
        public string JwtToken { get; set; }
        /// <summary>
        /// The unique identifier that was put inside the JWT. This will also act as the primary key for storage
        /// The JWT <i>jti</i>
        /// </summary>
        [JsonProperty(PropertyName = "jwtUniqueIdentifier", Required = Required.Always)]
        public string JwtUniqueIdentifier { get; set; }
        /// <summary>
        /// The secret used to encrypt the token.  This should be encrypted or not put here.
        /// </summary>
        [JsonProperty(PropertyName = "jwtSecret")]
        public string JwtSecret { get; set; } = "secret";
        /// <summary>
        /// Intended user of this token.  Can be empty but should never be null.
        /// The JWT <i>aud</i>
        /// </summary>
        [JsonProperty(PropertyName = "onBehalfOf")]
        public IIdentity OnBehalfOf { get; private set; } = new TokenIdentityEntity(null, null);
        /// <summary>
        /// Creator of the token, the caller of CreateToken(). Can be empty but should never be null
        /// The JWT <i>iss</i>
        /// </summary>
        [JsonProperty(PropertyName = "initiator")]
        public IIdentity Initiator { get; private set; } = new TokenIdentityEntity(null, null);
        /// <summary>
        /// list of parties that have called validate().  Can be empty but should never be null
        /// for audit
        /// </summary>
        [JsonProperty(PropertyName = "consumedBy", Required = Required.Always)]
        public IIdentity[] ConsumedBy { get; set; } = Array.Empty<TokenIdentityEntity>();

        /// <summary>
        /// Maximum number of times this token can be used.  Defaults to Integer MaxValue
        /// </summary>
        [JsonProperty(PropertyName = "maxUseCount", Required = Required.Always)]
        public int MaxUseCount { get; set; }
        /// <summary>
        /// The number of times this token has been used.
        /// Default is 0
        /// </summary>
        [JsonProperty(PropertyName = "currentUseCount", Required = Required.Always)]
        public int CurrentUseCount { get; set; }
        /// <summary>
        /// Length of time this token is valid.  Added to the initiation time
        /// Default is 300 seconds
        /// </summary>
        [JsonProperty(PropertyName = "expirationIntervalSec", Required = Required.Always)]
        public int ExpirationIntervalSec { get; set; }
        /// <summary>
        /// The time this token was created. Used to create expiration time
        /// The JWT <i>iat</i>
        /// </summary>
        [JsonProperty(PropertyName = "initiationTime", Required = Required.Always)]
        public DateTime InitiationTime { get; set; }
        /// <summary>
        /// The expiration time for this token. Calculated using initiationTime + expirationIntervalSec
        /// Default is now + 300 seconds
        /// The JWT <i>exp</i>
        /// </summary>
        [JsonProperty(PropertyName = "expirationTime", Required = Required.Always)]
        public DateTime ExpirationTime { get; set; }
        /// <summary>
        /// When the token can first be used
        /// The JWT <i>nbf</i>
        /// </summary>
        [JsonProperty(PropertyName = "effectiveTime", Required = Required.Always)]
        public DateTime EffectiveTime { get; set; }

        /// <summary>
        /// Arbitrary context shared by the token initator and consumes by the audience
        /// </summary>
        [JsonProperty(PropertyName = "context", NullValueHandling = NullValueHandling.Ignore)]
        public JToken Context { get; set; }

        /// <summary>
        /// Used by the repositoryies
        /// </summary>
        [JsonIgnore]
        public string Id => this.JwtUniqueIdentifier;

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
