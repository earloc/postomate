using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Postomate
{
    public class PostmanFolder
    {
        private readonly string name;
        private readonly JsonElement element;
        private readonly PostmanCollection collection;

        public PostmanFolder(string name, JsonElement element, PostmanCollection collection)
        {
            this.name = name;
            this.element = element;
            this.collection = collection;
        }

        private JsonElement FindBy(Regex regex)
        {
            var request = collection.FindItemRecursive(element, _ => regex.IsMatch(_));
            if (!request.HasValue)
            {
                throw new RequestNotFoundException($"Could not find request matching '{regex}' in folder '{name}'");
            }
            return request.Value;
        }

        private JsonElement FindBy(string name)
        {
            var request = collection.FindItemRecursive(element, _ => _.Contains(name));
            if (!request.HasValue)
            {
                throw new RequestNotFoundException($"Could not find request named '{name}' in folder '{this.name}'");
            }

            return request.Value;
        }

        public PostmanGrapqhQlRequest FindGrapqhQl(Regex regex, VariableContext context)
            => new PostmanGrapqhQlRequest(FindBy(regex), context, collection.Log);

        public PostmanGrapqhQlRequest FindGrapqhQl(string name, VariableContext context)
            => new PostmanGrapqhQlRequest(FindBy(name), context, collection.Log);

        public PostmanRawRequest FindRaw(Regex regex, VariableContext context) 
            => new PostmanRawRequest(FindBy(regex), context, collection.Log);

        public PostmanRawRequest FindRaw(string name, VariableContext context) 
            => new PostmanRawRequest(FindBy(name), context, collection.Log);

        public PostmanFormUrlEncodedRequest FindFormUrlEncoded(Regex regex, VariableContext context)
            => new PostmanFormUrlEncodedRequest(FindBy(regex), context, collection.Log);

        public PostmanFormUrlEncodedRequest FindFormUrlEncoded(string name, VariableContext context)
            => new PostmanFormUrlEncodedRequest(FindBy(name), context, collection.Log);
    }
}
