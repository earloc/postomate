using System;
using System.Net.Http;
using System.Text.Json;

namespace Postomate
{
    public class PostmanGrapqhQlRequest : PostmanRequestBase
    {
        public PostmanGrapqhQlRequest(JsonElement element, VariableContext context, Action<string> log) : base(element, context, log)
        {
            var request = enrichedElement.GetProperty("request");
            var graphql = request.GetProperty("body").GetProperty("graphql");

            Query = graphql.GetProperty("query").GetString() ?? "";
            Variables = graphql.GetProperty("variables").GetString() ?? "";
        }

        public string Query { get; }
        public string Variables { get; }

        //protected override HttpContent CreateContent()
        //{
        //    return new StringContent(@$"{{ 
        //        ""query"" = ""{Query}"",
        //        ""variables"" = {Variables}
        //    }}");
        //}
    }
}
