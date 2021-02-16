﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Postomate
{

    public abstract class PostmanRequestBase
    {
        protected readonly JsonElement enrichedElement;
        private readonly Action<string> log;

        protected PostmanRequestBase(JsonElement element, VariableContext context, Action<string> log)
        {
            this.log = log;
            var enrichedString = element.ToString() ?? "";

            foreach(var variable in context.Variables)
            {
                var oldValue = $"{{{{{variable.Key}}}}}";
                var newValue = variable.Value;

                enrichedString = enrichedString.Replace(oldValue, newValue);
            }

            this.enrichedElement = JsonDocument.Parse(enrichedString).RootElement;

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

        public HttpRequestMessage CreateRequestMessage()
        {
            var requestMessage = new HttpRequestMessage(Method, Url);

            foreach (var header in Headers)
            {

                if (header.Key == "Content-Type")
                {
                    continue;
                }

                requestMessage.Headers.Add(header.Key, header.Value);
            }

            requestMessage.Content = CreateContent();

            return requestMessage;
        }

        protected abstract HttpContent CreateContent();
    }
}
