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

            variables = new VariableContext();
        }


        [Fact]
        public async Task Posting_A_Person_Substitutes_Variables_Json()
        {
            variables.Enrich(new
            {
                firstName = "Zaphod",
                surname = "Beeblebrox"
            });

            var folder = sut.FindFolder("Tests");
            var request = folder.FindJson("PostPerson", variables);

            request.Body.Should().NotBe(request.RawBody, "we expect to the two to be unequal");

        }
    }
}
