using TokenService.Model.Rest;
using Xunit;

namespace TokenService.ModelTest.Rest
{
    public class TokenResponseTest
    {
        [Fact]
        public void TestZeroArg()
        {
            Assert.NotNull(new TokenResponse().Messages);
        }
    }
}
