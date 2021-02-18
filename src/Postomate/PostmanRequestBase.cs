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

        protected PostmanRequestBase(JsonElement element, VariableContext? context = null, Action<string>? log = null)
        {
            this.log = new Action<string>(message => log?.Invoke(message));

            RawContent = element.ToString() ?? "";
            EnrichedContent = RawContent;

            context ??= new VariableContext();

            foreach (var variable in context.Variables)
            {
                var oldValue = $"{{{{{variable.Key}}}}}";
                var newValue = variable.Value;

                EnrichedContent = EnrichedContent.Replace(oldValue, newValue);
            }

            if (context.RequiresFullSubstitution)
            {
                var unsubstitutedVariables = new Regex("{{[a-zA-Z0-9_-]*}}");

                var matches = unsubstitutedVariables.Matches(EnrichedContent).OfType<Match>();
                
                if (matches.Any())
                {
                    throw new UnsubstitutedVariablesException(matches.Select(x => x.Value).Distinct());
                }
            }

            this.enrichedElement = JsonDocument.Parse(EnrichedContent).RootElement;

            var request = enrichedElement.RequireProperty("request");


            Url = request.RequireProperty("url").RequireProperty("raw").GetString() ?? throw new InvalidOperationException("Url must not be null or empty");
            Method = new HttpMethod(request.RequireProperty("method").GetString() ?? throw new InvalidOperationException("Method must not be null or empty"));

            var headers = request.TryGetProperty("header");


            if (headers is not null)
            {
                foreach (var header in headers?.EnumerateArray()!)
                {
                    var key = header.RequireProperty("key").GetString();
                    var value = header.RequireProperty("value").GetString();

                    //some postman-headers may be disabled -> ignore them
                    if (header.TryGetProperty("disabled", out var disabled) && disabled.GetBoolean())
                    {
                        continue;
                    }

                    Headers.Add(key ?? "", value ?? "");
                }
            }

            

            if ((request.TryGetProperty("auth", out var authProperty)))
            {
                //just supporting bearer atm
                var tokenElement = authProperty.RequireProperty("bearer").EnumerateArray().FirstOrDefault(x => x.TryGetProperty("value", out var value));
                var token = tokenElement.RequireProperty("value").GetString();

                Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }


        public string Url { get; }
        public HttpMethod Method { get; }

        public AuthenticationHeaderValue? Authorization { get; }

        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

    }
}
