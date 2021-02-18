using Postomate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Postomate
{
    public static class PostomateSystemNetHttpExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postmanRequest">The <see cref="PostmanRawRequest"/> to build the <see cref="HttpRequestMessage"/> from</param>
        /// <param name="contentType">The override content-type. Defaults to the <see cref="PostmanRawRequest"/> Content-Type, if specified</param>
        /// <param name="encoding">Defaults to <see cref="Encoding.UTF8"/>, when null</param>
        /// <returns></returns>
        public static HttpRequestMessage ToHttpRequestMessage(this PostmanRawRequest postmanRequest, string? contentType = null, Encoding? encoding = null)
        {
            //encoding ??= Encoding.UTF8;

            var requestMessage = new HttpRequestMessage(postmanRequest.Method, postmanRequest.Url);

            //foreach (var header in postmanRequest.Headers)
            //{
            //    if (header.Key == "Content-Type")
            //    {
            //        contentType ??= header.Value;
            //        continue;
            //    }

            //    requestMessage.Headers.Add(header.Key, header.Value);
            //}

            //requestMessage.Content = new StringContent(postmanRequest.Body, encoding, contentType);

            return requestMessage;
        }
    }
}
