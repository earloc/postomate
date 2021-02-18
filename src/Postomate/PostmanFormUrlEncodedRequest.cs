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
            var request = enrichedElement.RequireProperty("request");

            Items = request
                .RequireProperty("body")
                .RequireProperty("urlencoded")
                .EnumerateArray()
                .Select(x => 
                    new KeyValuePair<string?, string?>(
                            x.RequireProperty("key").GetString(), 
                            x.RequireProperty("value").GetString()
                    )
                )
                .ToArray();
        }
        public IEnumerable<KeyValuePair<string?, string?>> Items { get; }
    }
}
