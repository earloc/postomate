using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Postomate.Postman
{
    public class PostmanRawRequest: RequestBase
    {
        public PostmanRawRequest(JsonElement element, IVariableContext? context, Action<string> log) : base(element, context, log)
        {
            Name = enrichedElement.TryGetProperty("name")?.GetString() ?? "";

            var requestElement = enrichedElement.TryGetProperty("request");
            var bodyElement = requestElement?.TryGetProperty("body");

            Body = bodyElement?.TryGetProperty("raw")?.GetString() ?? "";

            var bodyLanguage = bodyElement?
                .TryGetProperty("options")?
                .TryGetProperty("raw")?
                .TryGetProperty("language")?
                .GetString() ?? "";

            InferredContentType = bodyLanguage switch
            {
                "text" => "text/plain",
                "javascript" => "application/javascript",
                "json" => "application/json",
                "html" => "text/html",
                "xml" => "application/xml",
                _ => null
            };

        }
        public string Body { get; }

        public string? InferredContentType { get; }

        public string Name { get; }
    }
}
