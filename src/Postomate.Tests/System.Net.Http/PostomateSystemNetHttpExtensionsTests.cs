using System;
using Xunit;

namespace Postomate.Tests.System.Net.Http
{
    public class PostomateSystemNetHttpExtensionsTests : IClassFixture<ApiFixture>
    {



        public PostomateSystemNetHttpExtensionsTests(ApiFixture fixture)
        {

        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("PATCH")]
        [InlineData("DELETE")]
        public void Extension_Correctly_Configures_HttpMethod(string httpMethod)
        {

        }
    }
}
