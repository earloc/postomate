using System;
using System.Net.Http;
using System.Text.Json;

namespace Postomate
{
    //public class PostmanGrapqhQlRequest : PostmanRequestBase
    //{
    //    public PostmanGrapqhQlRequest(JsonElement element, IVariableContext? context, Action<string> log) : base(element, context, log)
    //    {
    //        var request = enrichedElement.RequireProperty("request");
    //        var graphql = request.RequireProperty("body").RequireProperty("graphql");

    //        Query = graphql.RequireProperty("query").GetString() ?? "";
    //        Variables = graphql.RequireProperty("variables").GetString() ?? "";
    //    }

    //    public string Query { get; }
    //    public string Variables { get; }

    //    //protected override HttpContent CreateContent()
    //    //{
    //    //    return new StringContent(@$"{{ 
    //    //        ""query"" = ""{Query}"",
    //    //        ""variables"" = {Variables}
    //    //    }}");
    //    //}
    //}
}
