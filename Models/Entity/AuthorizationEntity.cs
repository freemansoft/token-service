using Newtonsoft.Json;
using System.Collections.Generic;

namespace TokenService.Model.Entity
{
    /// <summary>
    /// What should be the primary key?, UserName?, identity composite (ProviderName/UserName) ...?
    /// </summary>
    public class AuthorizationEntity : IIdentity, IDataVersion, IHasId
    {
        /// <summary>
        /// only a version of "1.0" is currently supported
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        /// Identity provider. should configure JSON serializaton for providerName
        /// </summary>
        [JsonProperty(PropertyName = "providerName")]
        public string ProviderName { get; set; } = "global";

        /// <summary>
        /// user name. should configure JSON serializaton for userName
        /// </summary>
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Used by Repositories
        /// </summary>
        [JsonIgnore]
        public string Id => ProviderName + "/" + UserName;


        /// <summary>
        /// a bunch of URLs as regex
        /// </summary>
        public List<string> conditions;

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
