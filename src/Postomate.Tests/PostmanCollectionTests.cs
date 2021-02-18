using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Postomate.Tests
{
    public class PostmanCollectionTests : IClassFixture<ApiFixture>
    {

        private readonly PostmanCollection sut;
        private readonly HttpClient api;
        private readonly VariableContext variables;

        public PostmanCollectionTests(ApiFixture fixture, ITestOutputHelper output)
        {
            sut = PostmanCollection.Load("postomate.postman_collection.json", message => output.WriteLine(message));
            api = fixture.Api;

            variables = new VariableContext(new { 
                baseUrl = api.BaseAddress?.ToString().Trim('/') ?? "http://localhost:5042"
            });
        }


        [Theory]
        [InlineData("Tests", "PostPerson")]
        public async Task Finding_A_Request_Substitutes_Variables(string folderName, string requestName)
        {
            variables.Enrich(new
            {
                firstName = "Zaphod",
                surname = "Beeblebrox"
            });

            var folder = sut.FindFolder(folderName);
            var request = folder.FindJson(requestName, variables);

            request.EnrichedContent.Should().NotBe(request.RawContent, "we expect the two to be unequal");
            request.Url.Should().Contain("Zaphod");
            request.Url.Should().Contain("Beeblebrox");

        }
    }
}
