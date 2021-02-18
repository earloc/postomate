using Postomate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    //draft
    //public class HttpRequestMessageConverter
    //{

    //    public HttpRequestMessage ToRequestMessage(PostmanRawRequest postmanRequest)
    //    {
    //        var requestMessage = new HttpRequestMessage(postmanRequest.Method, postmanRequest.Url);

    //        foreach (var header in postmanRequest.Headers)
    //        {

    //            if (header.Key == "Content-Type")
    //            {
    //                continue;
    //            }

    //            requestMessage.Headers.Add(header.Key, header.Value);
    //        }

    //        requestMessage.Content = new StringContent(postmanRequest.Body, Encoding.UTF8, "application/json");

    //        return requestMessage;
    //    }
    //}
}
