namespace TokenService.ModelTest.Dto
{
    class TokenValidateJson
    {
        public static string TokenValidateRequest = @"{
            ""modelVersion"" : ""1.0"",
            ""jwtToken"" : ""=jlsdfjkldfdlkd"",
            ""accessedResource"" : ""http://www.foobar.com""
        }";

        public static string TokenValidateResponseContextNone = @"{
            ""modelVersion"" : ""1.0"",
            ""protectedResource"" : ""http://www.foobar.com"",
            ""messages"" : [
            ],
        }";

        public static string TokenValidateResponseMessages = @"{
            ""modelVersion"" : ""1.0"",
            ""protectedResource"" : ""http://www.foobar.com"",
            ""messages"" : [
                {
                    ""id"" : ""DOOM001"",
                    ""message"" : ""we're doomed""
                },
                {
                    ""id"" : ""NOMOR001"",
                    ""message"" : ""it can't take any more of this""
                }
            ],
        }";

        public static string TokenValidateResponseContextProperty = @"{
            ""modelVersion"" : ""1.0"",
            ""protectedResource"" : ""http://www.foobar.com"",
            ""messages"" : [
            ],
            ""context"" : ""singleProp""
        }";

        public static string TokenValidateResponseContextObject = @"{
            ""modelVersion"" : ""1.0"",
            ""protectedResource"" : ""http://www.foobar.com"",
            ""messages"" : [
            ],
            ""context"" : {
                ""key1"" : ""value1"",
                ""key2"" : ""value2""
            }
        }";

    }
}
