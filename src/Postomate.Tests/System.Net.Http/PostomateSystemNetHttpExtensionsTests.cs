using FluentAssertions;
using Postomate.Postman;
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

            requestMessage.Method.Method.Should().Be(httpMethod, "the http-verb defined in the postman-request should be used");
        }

        [Theory]
        [InlineData("text/plain")]
        [InlineData("application/javascript")]
        [InlineData("application/json")]
        [InlineData("text/html")]
        [InlineData("application/xml")]
        [InlineData("custom/contentType")]
        public void Extension_Uses_ContentType_FromPostman(string contentType)
        {
            var postmanRequest = folder.FindRaw(contentType);

            var requestMessage = postmanRequest.ToHttpRequestMessage();

            requestMessage.Content.Should().NotBeNull("otherwise, the next assertion would not make any sense at all");
            requestMessage.Content?.Headers.ContentType.Should().NotBeNull("otherwise, the next assertion would not make any sense at all, either");
            requestMessage.Content?.Headers.ContentType?.MediaType.Should().Be(contentType, "the content-type defined in the postman-request should be used");
        }

        [Theory]
        [InlineData("CustomHeaders", "X-POSTOMATE-SAMPLE", "SomeValue")]
        public void Extension_Respects_CustomHeaders(string requestName, string headerName, string headerValue)
        {
            var postmanRequest = folder.FindRaw(requestName);

            var requestMessage = postmanRequest.ToHttpRequestMessage();

            requestMessage.Headers.Contains(headerName).Should().BeTrue("this header should be mapped from the postman-request onto the request-message");

            requestMessage.Headers.GetValues(headerName)
                .Should()
                .BeEquivalentTo(new[] { headerValue }, "this header-value should be mapped from the postman-request onto the request-message")
            ;
        }

        [Theory]
        [InlineData("CustomHeaders", "X-POSTOMATE-DISABLED")]
        public void Extension_Respects_Disabled_CustomHeaders(string requestName, string headerName)
        {
            var postmanRequest = folder.FindRaw(requestName);

            var requestMessage = postmanRequest.ToHttpRequestMessage();

            requestMessage.Headers.Contains(headerName).Should().BeFalse($"the header '{headerName}' is disabled, which should reflect in the request-message");
        }
    }
}

