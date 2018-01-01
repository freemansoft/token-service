namespace TokenService.ModelTest.Entity
{
    class AuthorizationEntityJson
    {
        public static string AuthorizationEntityWithConditions = @"{
            ""version"" : ""1.0"",
            ""providerName"": ""OAuth2"",
            ""userName"": ""testuser"",
            ""conditions"": [
                ""condition 1"",
                ""condition 2""
            ]
        }";

    }
}