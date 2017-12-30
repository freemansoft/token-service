using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TokenService.Model.Entity;
using Xunit;
using Xunit.Abstractions;

namespace TokenService.ModelTest.Entity
{
    public class TokenEntityTest
    {

        private readonly ITestOutputHelper output;

        public TokenEntityTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SerializeContextEmpty()
        {
            DeserializeSerializeCompare(TokenEntityJson.TokenEntityContextEmpty);
        }

        [Fact]
        public void SerializeContextProperty()
        {
            DeserializeSerializeCompare(TokenEntityJson.TokenEntityContextProperty);
        }

        [Fact]
        public void SerializeContextArray()
        {
            DeserializeSerializeCompare(TokenEntityJson.TokenEntityContextArray);
        }



        private void DeserializeSerializeCompare(string jsonRep)
        {
            // convert the JSON to objects.  Convert the objects to JSON.
            TokenEntity hydrated = JsonConvert.DeserializeObject<TokenEntity>(jsonRep);
            Assert.NotNull(hydrated);
            string serialized = JsonConvert.SerializeObject(hydrated, Formatting.Indented);
            output.WriteLine(serialized);
            // compare original JSON with results of deserialize / serialize            
            var nodeSet1 = JsonConvert.DeserializeObject<JObject>(jsonRep);
            var nodeSet2 = JsonConvert.DeserializeObject<JObject>(serialized);
            Assert.True(JToken.DeepEquals(nodeSet1, nodeSet2),
                "Original JSON and results of deserialize,serialize are different token graphs");
        }

    }
}
