﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TokenService.ModelsTest.Rest
{
    class TokenCreateJson
    {
        static string TokenCreateBodyWithoutContextc = @"
            ""version"" : ""1.0"",
            ""protectedUrl"" : ""http://www.google.com"",
            ""onBehalfOf"":
            {
                ""providerName"" : ""google"",
                ""userName"" : ""foo@bar.com""
            },
            ""maxUsageCount"" : 10,
            ""expirationIntervalSeconds"": 0,
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

        public static string CreateTokenRequestContextEmpty = "{\n" + TokenCreateBodyWithoutContextc + EmptyContext +"\n}";

        public static string CreateTokenRequestContextProperty = "{\n" + TokenCreateBodyWithoutContextc + SimpleContext + "\n}";

        public static string CreateTokenRequestContextArray = "{\n" + TokenCreateBodyWithoutContextc + ArrayContext + "\n}";

        public static string CreateTokenResponseMessageEmpty = @"{
            ""version"" : ""1.0"",
            ""jwt"" : ""=asdfasdf"",
            ""messages"" : [
            ]
        }";

        public static string CreateTokenResponseMessageSingle = @"{
            ""version"" : ""1.0"",
            ""jwt"" : ""=asdfasdf"",
            ""messages"" : [
                {
                    ""message"" : ""some message""
                }
            ]
        }";
    }
}