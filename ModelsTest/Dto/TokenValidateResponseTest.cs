﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TokenService.Model.Dto;
using Xunit;
using Xunit.Abstractions;

namespace ModelTest.Dto
{
    public class TokenValidateResponseTest
    {
        private readonly ITestOutputHelper output;

        public TokenValidateResponseTest(ITestOutputHelper output)
        {
            this.output = output;

        }

        [Fact]
        public void SerializeContextNone()
        {
            DeserializeSerializeCompare(TokenValidateJson.TokenValidateResponseContextNone);
        }

        [Fact]
        public void SerializeWithMessages()
        {
            DeserializeSerializeCompare(TokenValidateJson.TokenValidateResponseMessages);
        }

        [Fact]
        public void SerializeContextProperty()
        {
            DeserializeSerializeCompare(TokenValidateJson.TokenValidateResponseContextProperty);
        }

        [Fact]
        public void SerializeContextObject()
        {
            DeserializeSerializeCompare(TokenValidateJson.TokenValidateResponseContextObject);
        }

        private void DeserializeSerializeCompare(string jsonRep)
        {
            // convert the JSON to objects.  Convert the objects to JSON.
            TokenValidateResponse hydrated = JsonConvert.DeserializeObject<TokenValidateResponse>(jsonRep);
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
