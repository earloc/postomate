using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace Postomate
{
    public class PostmanFormUrlEncodedRequest : PostmanRequestBase
    {
        public PostmanFormUrlEncodedRequest(JsonElement element, VariableContext context, Action<string> log) : base(element, context, log)
        {
            var request = enrichedElement.GetProperty("request");

            Items = request
                .GetProperty("body")
                .GetProperty("urlencoded")
                .EnumerateArray()
                .Select(x => 
                    new KeyValuePair<string?, string?>(
                            x.GetProperty("key").GetString(), 
                            x.GetProperty("value").GetString()
                    )
                )
                .ToArray();
        }
        public IEnumerable<KeyValuePair<string?, string?>> Items { get; }

        protected override HttpContent CreateContent() => new FormUrlEncodedContent(Items);
    }
}
