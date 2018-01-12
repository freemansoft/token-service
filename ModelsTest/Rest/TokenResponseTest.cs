using System.Collections.Generic;
using TokenService.Model.Rest;
using Xunit;

namespace TokenService.ModelTest.Rest
{
    public class TokenResponseTest
    {
        [Fact]
#pragma warning disable CA1822
        public void ZeroArg()
#pragma warning restore CA1822 
        {
            Assert.NotNull(new TokenResponse().Messages);
        }

        [Fact]
#pragma warning disable CA1822
        public void TestToString()
#pragma warning restore CA1822 
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
