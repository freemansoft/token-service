using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        /// <param name="validatees"></param>
        [JsonConstructor]
        public TokenEntity(TokenIdentityEntity onBehalfOf, TokenIdentityEntity initiator, TokenIdentityEntity[] validatees)
        {
            this.onBehalfOf = onBehalfOf;
            this.initiator = initiator;
            this.validatees = validatees;
        }

        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
        /// <summary>
        /// url protected by this token
        /// </summary>
        public string protectedUrl;
        /// <summary>
        /// the JWT given to the user to be presented to the consuming system and passed to the validate call()
        /// </summary>
        public string jwt;
        /// <summary>
        /// The unique identifier that was put inside the JWT. This will also act as the primary key for storage
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
        /// </summary>
        public IIdentity initiator;
        /// <summary>
        /// list of parties that have called validate()
        /// </summary>
        public IIdentity[] validatees = new TokenIdentityEntity[0];
        /// <summary>
        /// Token configuration and current state
        /// </summary>
        public TokenStateEntity tokenState;
        /// <summary>
        /// Arbitrary context shared by the token initator and consumes by the validatees
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public JToken context;
    }
}
