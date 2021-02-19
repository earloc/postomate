using Postomate.Postman;
using System.Net.Http;
using System.Text;

namespace Postomate
{
    public static class PostomateSystemNetHttpExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postmanRequest">The <see cref="RawRequest"/> to build the <see cref="HttpRequestMessage"/> from</param>
        /// <param name="contentType">The override content-type. Defaults to the <see cref="RawRequest"/> Content-Type, if specified</param>
        /// <param name="encoding">Defaults to <see cref="Encoding.UTF8"/>, when null</param>
        /// <returns></returns>
        public static HttpRequestMessage ToHttpRequestMessage(this RawRequest postmanRequest, string? contentType = null, Encoding? encoding = null)
        {
            var requestMessage = new HttpRequestMessage(postmanRequest.Method, postmanRequest.Url);

            contentType = postmanRequest.InferredContentType;

            foreach (var header in postmanRequest.Headers)
            {
                if (header.Key == "Content-Type")
                {
                    contentType = header.Value;
                    break;
                }

                requestMessage.Headers.Add(header.Key, header.Value);
            }

            requestMessage.Content = new StringContent(postmanRequest.Body, encoding, contentType);

            return requestMessage;
        }
    }
}
