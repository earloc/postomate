using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Postomate.Postman;
using Postomate.Tests.Api;
using System.Net.Http;
using Xunit.Abstractions;

namespace Postomate.Tests
{
    public class ApiFixture
    {
        public ApiFixture()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());

            Api = server.CreateClient();
            ApiClient = new TestApiClient.swaggerClient(Api);
        }
        public HttpClient Api { get; }

        public TestApiClient.swaggerClient ApiClient { get; }


        public RequestCollection PostmanCollection(ITestOutputHelper output) => Postomate.Postman.RequestCollection.Load("postomate.postman_collection.json", message => output.WriteLine(message));
    }
}
