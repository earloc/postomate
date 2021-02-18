using FluentAssertions;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace Postomate.Tests.System.Net.Http
{
    public class PostomateSystemNetHttpExtensionsTests : IClassFixture<ApiFixture>
    {
        private readonly HttpClient api;
        private readonly PostmanFolder folder;

        public PostomateSystemNetHttpExtensionsTests(ApiFixture fixture, ITestOutputHelper output)
        {
            api = fixture.Api;
            folder = fixture
                .PostmanCollection(output)
                .FindFolder("System.Net.Http");
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("PATCH")]
        [InlineData("DELETE")]
        public void Extension_Correctly_Configures_HttpMethod(string httpMethod)
        {
            var postmanRequest = folder.FindRaw(httpMethod);
            
            var requestMessage = postmanRequest.ToHttpRequestMessage();

            requestMessage.Method.Method.Should().Be(httpMethod);

        }
    }
}
