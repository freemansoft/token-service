using System.Collections.Generic;
using TokenService.Model.Rest;
using Xunit;

namespace TokenService.ModelTest.Rest
{
    public class TokenResponseTest
    {
#pragma warning disable CA1822
        [Fact]
        public void ZeroArg()
        {
            Assert.NotNull(new TokenResponse().Messages);
        }

        [Fact]
        public void TestToString()
        {
            List<TokenResponseMessage> messages = new List<TokenResponseMessage>()
            {
                new TokenResponseMessage("a","b")
            };
            TokenResponse ut = new TokenResponse(messages);
            Assert.True(ut.ToString().Contains("a"), "serialized TokenResponse didn't contain expected message value 'a'");
            Assert.True(ut.ToString().Contains("b"), "serialized TokenResponse didn't contain expected message value 'b'");
        }
    }
}
