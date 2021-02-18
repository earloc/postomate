using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Postomate
{
    public class PostmanRawRequest: PostmanRequestBase
    {
        public PostmanRawRequest(JsonElement element, VariableContext context, Action<string> log) : base(element, context, log)
        {
            Name = enrichedElement.TryGetProperty("name")?.GetString() ?? "";

            var request = enrichedElement.TryGetProperty("request");
            Body = request?.TryGetProperty("body")?.TryGetProperty("raw")?.GetString() ?? "";
        }
        public string Body { get; }

        //todo: read from postman-request
        public string MediaType { get; protected set; } = "application/xml";
        public string Name { get; }

        protected override HttpContent CreateContent() => new StringContent(Body, Encoding.Default, MediaType);
    }
}
