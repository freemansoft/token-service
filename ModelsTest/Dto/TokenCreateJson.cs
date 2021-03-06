﻿namespace ModelTest.Dto
{
    class TokenCreateJson
    {
        static string TokenCreateBodyWithoutContextc = @"
            ""modelVersion"" : ""1.0"",
            ""protectedResource"" : ""http://www.google.com"",
            ""onBehalfOf"":
            {
                ""providerName"" : ""google"",
                ""userName"" : ""foo@bar.com""
            },
            ""maxUsageCount"" : 10,
            ""expirationIntervalSeconds"": 0,
            ""effectiveTime"":  ""2000-12-29T20:00:00Z"",
        ";

        static string EmptyContext = @"
            ""context"" : 
            {
            }
        ";

        static string SimpleContext = @"
            ""context"" : ""dogfood""
        ";

        static string ArrayContext = @"
            ""context"" : 
            [
                { ""key"" : ""value1"", ""id"" : ""value"" },
                { ""key"" : ""value2"", ""id"" : ""value"" }
            ]
        ";

        public static string CreateTokenRequestContextEmpty = "{\n" + TokenCreateBodyWithoutContextc + EmptyContext + "\n}";

        public static string CreateTokenRequestContextProperty = "{\n" + TokenCreateBodyWithoutContextc + SimpleContext + "\n}";

        public static string CreateTokenRequestContextArray = "{\n" + TokenCreateBodyWithoutContextc + ArrayContext + "\n}";

        public static string CreateTokenResponseMessageEmpty = @"{
            ""modelVersion"" : ""1.0"",
            ""jwtToken"" : ""=asdfasdf"",
            ""messages"" : [
            ]
        }";

        public static string CreateTokenResponseMessageSingle = @"{
            ""modelVersion"" : ""1.0"",
            ""jwtToken"" : ""=asdfasdf"",
            ""messages"" : [
                {
                    ""id"" : ""MSG001"",
                    ""message"" : ""some message""
                }
            ]
        }";
    }
}
