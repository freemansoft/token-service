using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TokenService.Model.Entity;
using Xunit;
using Xunit.Abstractions;

namespace TokenService.ModelTest.Entity
{
    public class AuthorizationEntityTest
    {

        private readonly ITestOutputHelper output;

        public AuthorizationEntityTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SerializeAuthorization()
        {
            DeserializeSerializeCompare(AuthorizationEntityJson.AuthorizationEntityWithConditions);
        }



        private void DeserializeSerializeCompare(string jsonRep)
        {
            // convert the JSON to objects.  Convert the objects to JSON.
            AuthorizationEntity hydrated = JsonConvert.DeserializeObject<AuthorizationEntity>(jsonRep);
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
