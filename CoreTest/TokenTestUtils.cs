using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using TokenService.Model.Dto;
using TokenService.Core.Service;

namespace CoreTest
{
    class TokenTestUtils
    {
        /// <summary>
        /// Note that this is anchored at the beginning of a line
        /// </summary>
        internal string bogusTestUrl = "http://myurl.com";

        /// <summary>
        /// Test key needs to be >= 128 characters
        /// </summary>
        /// <returns></returns>
        internal IOptions<CryptographySettings> BuildCryptographySettings()
        {
            CryptographySettings mySeTings = new CryptographySettings()
            {
                // 68+68 characters
                JwtSecret = Guid.NewGuid().ToString("X") + Guid.NewGuid().ToString("X")
            };

            return Options.Create<CryptographySettings>(mySeTings);
        }

        internal TokenCreateRequest BuildTokenCreateRequest()
        {
            TokenIdentity obo = new TokenIdentity(null, "testid");
            TokenCreateRequest request = new TokenCreateRequest(obo)
            {
                MaxUsageCount = 1,
                ProtectedResource = bogusTestUrl,
                Context = JObject.Parse(@"{ ""x"":""value""}"),
            };
            return request;

        }

        internal TokenValidateRequest BuildTokenValidateRequest(string jwtToken, string protectedUrl)
        {
            // should we create a constructor in TokenValidateRequeste for this since there are so few properties
            TokenValidateRequest ourValidateRequest = new TokenValidateRequest()
            {
                JwtToken = jwtToken,
                AccessedResource = protectedUrl
            };
            return ourValidateRequest;
        }
    }
}
