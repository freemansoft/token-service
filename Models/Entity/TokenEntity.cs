using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenService.Models.Entity
{
    /// <summary>
    /// Root object for token information stored in a persistent store. This token's services root model object
    /// </summary>
    class TokenEntity: IDataVersion
    {
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
        /// The unique identifier that was put inside the JWT
        /// </summary>
        public string jwtUniqueIdentifier;
        /// <summary>
        /// The secret used to encrypt the token.  Should this really be here?
        /// </summary>
        public string jwtSecret = "secret";
        /// <summary>
        /// Intended user of this token
        /// </summary>
        public TokenIdentityEntity onBehalfOf;
        /// <summary>
        /// Creator of the token, the caller of CreateToken()
        /// </summary>
        public TokenIdentityEntity initiator;
        /// <summary>
        /// list of parties that have called validate()
        /// </summary>
        public TokenIdentityEntity[] validatees = new TokenIdentityEntity[0];
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
