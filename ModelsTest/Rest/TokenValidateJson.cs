namespace TokenService.ModelTest.Rest
{
    class TokenValidateJson
    {
        public static string TokenValidateRequest = @"{
            ""version"" : ""1.0"",
            ""jwt"" : ""=jlsdfjkldfdlkd"",
            ""protectedUrl"" : ""http://www.foobar.com""
        }";

        public static string TokenValidateResponseContextNone = @"{
            ""version"" : ""1.0"",
            ""messages"" : [
            ],
        }";

        public static string TokenValidateResponseMessages = @"{
            ""version"" : ""1.0"",
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
            ""version"" : ""1.0"",
            ""messages"" : [
            ],
            ""context"" : ""singleProp""
        }";

        public static string TokenValidateResponseContextObject = @"{
            ""version"" : ""1.0"",
            ""messages"" : [
            ],
            ""context"" : {
                ""key1"" : ""value1"",
                ""key2"" : ""value2""
            }
        }";

    }
}
