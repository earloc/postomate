using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
    public class IntegrationTests : IClassFixture<ApiFixture>
    {

        private readonly PostmanCollection sut;
        private readonly HttpClient api;
        private readonly IVariableContext variables;
        private readonly PostmanFolder folder;

        public IntegrationTests(ApiFixture fixture, ITestOutputHelper output)
        {
            sut = fixture.PostmanCollection(output);
            api = fixture.Api;

            variables = new ImmutableVariableContext(new
            {
                baseUrl = api.BaseAddress?.ToString().Trim('/') ?? "http://localhost:5042"
            });

            folder = sut.FindFolder("Tests");
        }

        [Fact]
        public async Task Orchestrate_Cread_And_Read_Of_Persons_With_Postman()
        {

            var getRequest = folder.FindRaw("GetAllPersons", variables);
            var getResponse = await api.SendAsync(getRequest.ToHttpRequestMessage());
            getResponse.StatusCode.Should().Be(StatusCodes.Status200OK, "this should work ;)");
            var getResponseContent = await getResponse.Content.ReadAsStringAsync();
            getResponseContent.Should().Be("[]", "in the beginning, there are no persons");


            var createArthurRequest = folder.FindRaw("PostPerson", variables.Enrich(new {
                firstName = "Arthur",
                surname = "Dent"
            }));
            var createArthurResponse = await api.SendAsync(createArthurRequest.ToHttpRequestMessage());
            createArthurResponse.StatusCode.Should().Be(StatusCodes.Status200OK, "Arthur needs to be on the bridge!");

            var createZaphodRequest = folder.FindRaw("PostPerson", variables.Enrich(new { 
                firstName = "Zaphod", 
                surname = "Beeblebrox"
            }));
            var createZaphodResponse = await api.SendAsync(createZaphodRequest.ToHttpRequestMessage());
            createZaphodResponse.StatusCode.Should().Be(StatusCodes.Status200OK, "we need Zaphods heads - all of them!");

            var response = await api.SendAsync(getRequest.ToHttpRequestMessage());
            response.StatusCode.Should().Be(StatusCodes.Status200OK, "this should work ;)");
            var responseContent = await response.Content.ReadAsStringAsync();

            responseContent.Should().NotBe("[]", "in the end, there should be persons");
        }
    }
}
