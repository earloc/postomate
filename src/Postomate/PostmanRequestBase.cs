using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Postomate
{

    public abstract class PostmanRequestBase
    {
        protected readonly JsonElement enrichedElement;

        public string EnrichedContent { get; }
        public string RawContent { get; }

        private readonly Action<string> log;

        protected PostmanRequestBase(JsonElement element, VariableContext context, Action<string> log)
        {
            this.log = log;
            RawContent = element.ToString() ?? "";
            EnrichedContent = RawContent;

            foreach(var variable in context.Variables)
            {
                var oldValue = $"{{{{{variable.Key}}}}}";
                var newValue = variable.Value;

                EnrichedContent = EnrichedContent.Replace(oldValue, newValue);
            }

            if (context.RequiresFullSubstitution)
            {
                var unsubstitutedVariables = new Regex("{{[a-zA-Z0-9_-]*}}");

                var matches = unsubstitutedVariables.Matches(EnrichedContent);

                if (matches.Any())
                {
                    throw new UnsubstitutedVariablesException(matches.Select(x => x.Value).Distinct());
                }
            }

            this.enrichedElement = JsonDocument.Parse(EnrichedContent).RootElement;

            var request = enrichedElement.GetProperty("request");
            Url = request.GetProperty("url").GetProperty("raw").GetString() ?? "";
            Method = new HttpMethod(request.GetProperty("method").GetString() ?? "");

            var headers = request.GetProperty("header");

            foreach (var header in headers.EnumerateArray())
            {
                var key = header.GetProperty("key").GetString();
                var value = header.GetProperty("value").GetString();

                //some postman-headers may be disabled
                if (header.TryGetProperty("disabled", out var disabled) && disabled.GetBoolean())
                {
                    continue;
                }

                Headers.Add(key ?? "", value ?? "");
            }

            if (request.TryGetProperty("auth", out var authProperty))
            {
                //just supporting bearer atm
                var tokenElement = authProperty.GetProperty("bearer").EnumerateArray().FirstOrDefault(x => x.TryGetProperty("value", out var value));
                var token = tokenElement.GetProperty("value").GetString();

                Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }


        public string Url { get; }
        public HttpMethod Method { get; }

        public AuthenticationHeaderValue? Authorization { get; }

        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

    }
}
