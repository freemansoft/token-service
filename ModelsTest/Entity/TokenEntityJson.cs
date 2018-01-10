namespace TokenService.ModelTest.Entity
{
    class TokenEntityJson
    {
        static string TokenEntityBodyWithoutContext = @"
            ""version"" : ""1.0"",
            ""protectedUrl"" : ""http://www.google.com"",
            ""jwt"" : ""=xdlkjfdkjdf"",
            ""jwtUniqueIdentifier"" : ""ADFG-ASDKL-SCFF"",
            ""jwtSecret"" : ""secret"",
            ""onBehalfOf"":
            {
                ""providerName"" : ""google"",
                ""userName"" : ""someuser@bar.com""
            },
            ""initiator"":
            {
                ""providerName"" : ""freemansoft"",
                ""userName"" : ""initiator@bar.com""
            },
            ""consumedBy"":
            [
                {
                    ""providerName"" : ""freemansoft"",
                    ""userName"" : ""validateme@bar.com""
                }
            ],

            ""maxUseCount"": 1,
            ""currentUseCount"": 0,
            ""expirationIntervalSec"": 300,
            ""initiationTime"": ""2020-12-29T00:00:00Z"",
            ""expirationTime"": ""2000-12-29T20:00:00Z"",
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

        public static string TokenEntityContextEmpty = "{\n" + TokenEntityBodyWithoutContext + EmptyContext + "\n}";

        public static string TokenEntityContextProperty = "{\n" + TokenEntityBodyWithoutContext + SimpleContext + "\n}";

        public static string TokenEntityContextArray = "{\n" + TokenEntityBodyWithoutContext + ArrayContext + "\n}";
    }
}
