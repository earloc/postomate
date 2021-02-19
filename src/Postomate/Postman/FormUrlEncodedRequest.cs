namespace Postomate.Postman
{
    //public class FormUrlEncodedRequest : RequestBase
    //{
    //    public FormUrlEncodedRequest(JsonElement element, IVariableContext? context, Action<string> log) : base(element, context, log)
    //    {
    //        var request = enrichedElement.RequireProperty("request");

    //        Items = request
    //            .RequireProperty("body")
    //            .RequireProperty("urlencoded")
    //            .EnumerateArray()
    //            .Select(x => 
    //                new KeyValuePair<string?, string?>(
    //                        x.RequireProperty("key").GetString(), 
    //                        x.RequireProperty("value").GetString()
    //                )
    //            )
    //            .ToArray();
    //    }
    //    public IEnumerable<KeyValuePair<string?, string?>> Items { get; }
    //}
}
