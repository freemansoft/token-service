using System;

namespace TokenService.Models.Rest
{
    public class HelloResponse
    {
        public string greeting;
        public string createDate = "Response Time: "+DateTime.Now.ToString();
    }
}
