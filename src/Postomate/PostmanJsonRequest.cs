using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Postomate
{
    public class PostmanJsonRequest : PostmanRawRequest
    {
        public PostmanJsonRequest(JsonElement element, VariableContext context, Action<string> log) : base(element, context, log)
        {
            this.MediaType = "application/json";
        }

        protected override HttpContent CreateContent()
        {
            return new StringContent(this.Body, Encoding.UTF8, "application/json");
        }
    }
}
