using FluentAssertions;
using Postomate.Postman;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace Postomate.Tests.Features
{
    [Binding]
    public class PostmanAutomationSteps : IClassFixture<ApiFixture>
    {

        private readonly RequestFolder featureFolder;
        private readonly ScenarioContext scenarioContext;
        private IVariableContext variables;
        private HttpClient api;

        public PostmanAutomationSteps(ApiFixture fixture, ITestOutputHelper output, ScenarioContext scenarioContext)
        {
            this.featureFolder = fixture.PostmanCollection(output).FindFolder("Features");
            this.scenarioContext = scenarioContext;

            this.api = fixture.Api;

            variables = new ImmutableVariableContext(new
            {
                baseUrl = api.BaseAddress?.ToString().Trim('/') ?? "http://localhost:5042"
            });
        }

        [Given(@"the person '(.*)' '(.*)'")]
        public void GivenThePerson(string firstName, string surname)
        {
            variables = variables.Enrich(new
            {
                firstName,
                surname
            });
        }
        
        [When(@"the person is posted to the api")]
        public async Task WhenThePersonIsPostedToTheApi()
        {
            var request = featureFolder.FindRaw(scenarioContext.StepContext.StepInfo.BindingMatch.StepBinding.Regex, variables);

            var response = await api.SendAsync(request.ToHttpRequestMessage());

            scenarioContext.Set(response);

        }
        
        [Then(@"the response-statuscode is '(.*)'")]
        public void ThenTheResponseStatusCodeIs(int httpStatusCode)
        {
            var response = scenarioContext.Get<HttpResponseMessage>();

            response.StatusCode.Should().Be((HttpStatusCode)httpStatusCode);
        }
    }
}
