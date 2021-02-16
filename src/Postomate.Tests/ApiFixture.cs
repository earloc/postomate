using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Postomate.Tests.Api;
using System.Net.Http;

namespace Postomate.Tests
{
    public class ApiFixture
    {
        public ApiFixture()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());

            Api = server.CreateClient();
        }
        public HttpClient Api { get; }
    }
}
