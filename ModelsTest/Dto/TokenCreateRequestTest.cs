using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TokenService.Model.Dto;
using Xunit;
using Xunit.Abstractions;

namespace TokenService.ModelTest.Dto
{
    public class TokenCreateRequestTest
    {
        private readonly ITestOutputHelper output;

        public TokenCreateRequestTest(ITestOutputHelper output)
        {
            this.output = output;

        }

        [Fact]
        public void SerializeContextEmpty()
        {
            DeserializeSerializeCompare(TokenCreateJson.CreateTokenRequestContextEmpty);
        }

        [Fact]
        public void SerializeContextProperty()
        {
            DeserializeSerializeCompare(TokenCreateJson.CreateTokenRequestContextProperty);
        }

        [Fact]
        public void SerializeContextArray()
        {
            DeserializeSerializeCompare(TokenCreateJson.CreateTokenRequestContextArray);
        }

        private void DeserializeSerializeCompare(string jsonRep)
        {
            // convert the JSON to objects.  Convert the objects to JSON.
            TokenCreateRequest hydrated = JsonConvert.DeserializeObject<TokenCreateRequest>(jsonRep);
            Assert.NotNull(hydrated);
            output.WriteLine("Original=" + jsonRep);
            string serialized = JsonConvert.SerializeObject(hydrated, Formatting.Indented);
            output.WriteLine("Serialized=" + serialized);
            // compare original JSON with results of deserialize / serialize            
            var nodeSet1 = JsonConvert.DeserializeObject<JObject>(jsonRep);
            var nodeSet2 = JsonConvert.DeserializeObject<JObject>(serialized);
            Assert.True(JToken.DeepEquals(nodeSet1, nodeSet2),
                "Original JSON and results of deserialize,serialize are different token graphs");
        }
    }
}
