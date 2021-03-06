﻿using FluentAssertions;
using Postomate.Postman;
using System.Net.Http;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Postomate.Tests
{
    public class PostmanCollectionTests : IClassFixture<ApiFixture>
    {

        private readonly RequestCollection sut;
        private readonly HttpClient api;
        private readonly MutableVariableContext variables;

        public PostmanCollectionTests(ApiFixture fixture, ITestOutputHelper output)
        {

            sut = fixture.PostmanCollection(output);
            api = fixture.Api;

            variables = new MutableVariableContext(new { 
                baseUrl = api.BaseAddress?.ToString().Trim('/') ?? "http://localhost:5042"
            });
        }

        [Theory]
        [InlineData("GetAllPersons")]
        [InlineData("PostPerson")]
        public void Finds_a_Request_When_Searching_Recursive_From_Root(string requestName)
        {

            var folder = sut.FindFolder("NonExistingFolder");
            var request = folder.FindRaw(requestName, new MutableVariableContext(requiresFullSubstitution: false));

            request.Should().NotBeNull("when a folder is not found, a recursive search is performed from the collection-root, which should find the required request");
        }

        [Theory]
        [InlineData("GetAllPersons", "Get.*Persons")]
        [InlineData("PostPerson", "^.*Person$")]
        public void Finds_a_Request_ByRegex(string requestName, string regex)
        {

            var folder = sut.FindFolder("NonExistingFolder");
            var request = folder.FindRaw(new Regex(regex), new MutableVariableContext(requiresFullSubstitution: false));

            request.Should().NotBeNull("searching by a regex should discover a request");

            request.Name.Should().Be(requestName, "that should be the first request matching the specified regex");
        }


        [Theory]
        [InlineData("Tests", "PostPerson")]
        public void Finding_A_Post_RawRequest_Substitutes_Variables(string folderName, string requestName)
        {
            variables.Enrich(new
            {
                firstName = "Zaphod",
                surname = "Beeblebrox"
            });

            var folder = sut.FindFolder(folderName);
            var request = folder.FindRaw(requestName, variables);

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
        public void Finding_A_Post_RawRequest_Optionally_Ensures_That_No_Unsubstituted_Variables_Are_Left(string folderName, string requestName)
        {
            var folder = sut.FindFolder(folderName);

            folder.Invoking(_ => _.FindRaw(requestName, new MutableVariableContext(requiresFullSubstitution: true)))
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
        public void Finding_A_Post_RawRequest_Optionally_Can_Contain_Unsubstituted_Varaibles(string folderName, string requestName)
        {
            var folder = sut.FindFolder(folderName);

            folder.Invoking(_ => _.FindRaw(requestName, new MutableVariableContext(requiresFullSubstitution: false)))
                .Should()
                .NotThrow<UnsubstitutedVariablesException>()
            ;
        }

        [Theory]
        [InlineData("NonExistingRequest")]
        public void Throws_When_Request_Not_Found_ByName(string requestName)
        {
            var folder = sut.FindFolder("NonExistingFolder");

            folder.Invoking(_ => _.FindRaw(requestName, new MutableVariableContext()))
                .Should()
                .Throw<RequestNotFoundException>()
                .WithMessage($"Could not find request named '{requestName}' in folder 'root'")
            ;
        }

        [Theory]
        [InlineData("NonExisting.*")]
        public void Throws_When_Request_Not_Found_ByRegex(string requestName)
        {
            var folder = sut.FindFolder("NonExistingFolder");

            folder.Invoking(_ => _.FindRaw(new Regex(requestName), new MutableVariableContext()))
                .Should()
                .Throw<RequestNotFoundException>()
                .WithMessage($"Could not find request matching '{requestName}' in folder 'root'")
            ;
        }
    }
}
