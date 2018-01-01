using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace TokenService.Model.Entity
{
    /// <summary>
    /// Root object for token information stored in a persistent store. This token's services root model object
    /// </summary>
    public class TokenEntity: IDataVersion
    {
        /// <summary>
        /// This constructor is required so that the JSON serializer knows which concreate class to use for a proprty declared as an interface
        /// <a href="https://stackoverflow.com/questions/5780888/casting-interfaces-for-deserialization-in-json-net">from stackoverflow</a>
        /// Another option is to use a converter
        /// </summary>
        /// <param name="onBehalfOf"></param>
        /// <param name="initiator"></param>
        /// <param name="audience"></param>
        [JsonConstructor]
        public TokenEntity(TokenIdentityEntity onBehalfOf, TokenIdentityEntity initiator, TokenIdentityEntity[] audience)
        {
            this.onBehalfOf = onBehalfOf;
            this.initiator = initiator;
            this.audience = audience;
        }

        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
        /// <summary>
        /// url protected by this token
        /// The JWT <i>sub</i>
        /// </summary>
        public string protectedUrl;
        /// <summary>
        /// the JWT given to the user to be presented to the consuming system and passed to the validate call()
        /// </summary>
        public string jwt;
        /// <summary>
        /// The unique identifier that was put inside the JWT. This will also act as the primary key for storage
        /// The JWT <i>jti</i>
        /// </summary>
        public string jwtUniqueIdentifier;
        /// <summary>
        /// The secret used to encrypt the token.  Should this really be here?
        /// </summary>
        public string jwtSecret = "secret";
        /// <summary>
        /// Intended user of this token
        /// </summary>
        public IIdentity onBehalfOf;
        /// <summary>
        /// Creator of the token, the caller of CreateToken()
        /// The JWT <i>iss</i>
        /// </summary>
        public IIdentity initiator;
        /// <summary>
        /// list of parties that have called validate()
        /// The JWT <i>aud</i>
        /// </summary>
        public IIdentity[] audience = new TokenIdentityEntity[0];

        /// <summary>
        /// Maximum number of times this token can be used
        /// </summary>
        public int maxUseCount = 1;
        /// <summary>
        /// The number of times this token has been used
        /// </summary>
        public int currentUseCount = 0;
        /// <summary>
        /// Length of time this token is valid.  Added to the initiation time
        /// </summary>
        public int expirationIntervalSec = 300;
        /// <summary>
        /// The time this token was created. Used to create expiration time
        /// The JWT <i>iat</i>
        /// </summary>
        public DateTime initiationTime = DateTime.Now;
        /// <summary>
        /// The expiration time for this token. Calculated using initiationTime + expirationIntervalSec
        /// The JWT <i>exp</i>
        /// </summary>
        public DateTime expirationTime = DateTime.MaxValue;
        /// <summary>
        /// When the token can first be used
        /// The JWT <i>nbf</i>
        /// </summary>
        public DateTime effectiveTime = DateTime.Now;

        /// <summary>
        /// Arbitrary context shared by the token initator and consumes by the audience
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public JToken context;

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
