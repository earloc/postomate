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
            var request = enrichedElement.GetProperty("request");
            Body = request.GetProperty("body").GetProperty("raw").GetString() ?? "";

        }
        public string Body { get; }

        public string MediaType { get; protected set; } = "application/xml";

        protected override HttpContent CreateContent() => new StringContent(Body, Encoding.Default, MediaType);
    }
}
