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

        public string Url { get; }
        public HttpMethod Method { get; }
        public AuthenticationHeaderValue? Authorization { get; }
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        protected PostmanRequestBase(JsonElement element, IVariableContext? context = null, Action<string>? log = null)
        {
            this.log = new Action<string>(message => log?.Invoke(message));

            RawContent = element.ToString() ?? "";
            context ??= new MutableVariableContext();

            EnrichedContent = SubstituteVariables(RawContent, context);

            if (!TryEnsureVariableSubstitution(context, out var missingVariables))
            {
                throw new UnsubstitutedVariablesException(missingVariables);
            }

            this.enrichedElement = JsonDocument.Parse(EnrichedContent).RootElement;
            var request = enrichedElement.RequireProperty("request");

            Method = new HttpMethod(request.RequireProperty("method").GetString() ?? throw new InvalidOperationException("Method must not be null or empty"));
            Url = request.RequireProperty("url").RequireProperty("raw").GetString() ?? throw new InvalidOperationException("Url must not be null or empty");
            Headers = AdoptHeaders(request);
            Authorization = AdoptAuthorization(request);
        }

        private bool TryEnsureVariableSubstitution(IVariableContext context, out IEnumerable<string> missingVariables)
        {
            missingVariables = Enumerable.Empty<string>();

            if (!context.RequiresFullSubstitution)
            {
                return true;
            }
            var unsubstitutedVariables = new Regex("{{[a-zA-Z0-9_-]*}}");

            var matches = unsubstitutedVariables.Matches(EnrichedContent).OfType<Match>();
            if (matches.Any())
            {
                missingVariables = matches.Select(x => x.Value).Distinct();
                return false;
            }

            return true;
        }

        private IDictionary<string, string> AdoptHeaders(JsonElement request)
        {
            var headersProperty = request.TryGetProperty("header");

            var headers = new Dictionary<string, string>();

            if (headersProperty is null)
            {
                return headers;
            }

            foreach (var header in headersProperty.Value.EnumerateArray())
            {
                var key = header.RequireProperty("key").GetString();
                var value = header.RequireProperty("value").GetString();

                //some postman-headers may be disabled -> ignore them
                if (header.TryGetProperty("disabled", out var disabled) && disabled.GetBoolean())
                {
                    continue;
                }

                headers.Add(key ?? "", value ?? "");
            }

            return headers;
        }

        private AuthenticationHeaderValue? AdoptAuthorization(JsonElement request)
        {
            if ((request.TryGetProperty("auth", out var authProperty)))
            {
                //just supporting bearer atm
                var tokenElement = authProperty.RequireProperty("bearer").EnumerateArray().FirstOrDefault(x => x.TryGetProperty("value", out var value));
                var token = tokenElement.RequireProperty("value").GetString();

                return new AuthenticationHeaderValue("Bearer", token);
            }

            return default;
        }

        private string SubstituteVariables(string rawContent, IVariableContext context)
        {
            var enrichedContent = rawContent;

            foreach (var variable in context.Variables)
            {
                var oldValue = $"{{{{{variable.Key}}}}}";
                var newValue = variable.Value;

                enrichedContent = enrichedContent.Replace(oldValue, newValue);
            }

            return enrichedContent;
        }

    }
}
