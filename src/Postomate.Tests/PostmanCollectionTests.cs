using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
        public void Finding_A_Post_JsonRequest_Substitutes_Variables(string folderName, string requestName)
        {
            variables.Enrich(new
            {
                firstName = "Zaphod",
                surname = "Beeblebrox"
            });

            var folder = sut.FindFolder(folderName);
            var request = folder.FindJson(requestName, variables);

            request.EnrichedContent.Should().NotBe(request.RawContent, "we expect them to be unequal");
            request.Url.Should().Contain("Zaphod");
            request.Url.Should().Contain("Beeblebrox");
        }

        [Theory]
        [InlineData("Tests", "GetAllPersons")]
        public void Finding_A_Get_RawRequest_Substitutes_Variables(string folderName, string requestName)
        {
            var folder = sut.FindFolder(folderName);
            var request = folder.FindRaw(requestName, variables );

            request.EnrichedContent.Should().NotBe(request.RawContent, "we expect them to be unequal");
        }

        [Theory]
        [InlineData("Tests", "PostPerson")]
        public void Finding_A_Post_JsonRequest_Optionally_Ensures_That_No_Unsubstituted_Variables_Are_Left(string folderName, string requestName)
        {
            var folder = sut.FindFolder(folderName);

            folder.Invoking(_ => _.FindJson(requestName, new VariableContext(requiresFullSubstitution: true)))
                .Should()
                .Throw<UnsubstitutedVariablesException>()
                .Which.Variables
                .Should()
                .BeEquivalentTo(new [] { "{{baseUrl}}", "{{firstName}}", "{{surname}}" }, 
                    "these should be all variables defined in the request"
                );
        }

        [Theory]
        [InlineData("Tests", "PostPerson")]
        public void Finding_A_Post_JsonRequest_Optionally_Can_Contain_Unsubstituted_Varaibles(string folderName, string requestName)
        {
            var folder = sut.FindFolder(folderName);

            folder.Invoking(_ => _.FindJson(requestName, new VariableContext(requiresFullSubstitution: false)))
                .Should()
                .NotThrow<UnsubstitutedVariablesException>()
            ;
        }
    }
}
