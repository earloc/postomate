using System;
using System.IO;
using System.Text.Json;

namespace Postomate
{

    public class RequestCollection
    {

        readonly JsonDocument rawContent;
        private readonly Action<string> log;

        public void Log(string message) => log(message);

        public RequestFolder Root { get; }


        public static RequestCollection Load(string path, Action<string>? log = null)
        {
            var content = File.ReadAllText(path);
            var json = JsonDocument.Parse(content);

            log?.Invoke($"using postman-collection '{Path.GetFullPath(path)}'");

            return new RequestCollection(json, log);
        }

        public RequestCollection(JsonDocument rawContent, Action<string>? log = null)
        {
            this.rawContent = rawContent;
            this.log = log ?? new Action<string>((message) => Console.WriteLine(message));

            Root = new RequestFolder("root", rawContent.RootElement, this);
        }

        public RequestFolder FindFolder(string name)
        {
            var item = FindItemRecursive(rawContent.RootElement, _ => _.Contains(name));

            if (!item.HasValue)
            {
                return Root;
            }
            return new RequestFolder(name, item.Value, this);
        }

        public JsonElement? FindItemRecursive(JsonElement element, Func<string, bool> predicate)
        {
            if (element.TryGetProperty("name", out var nameProperty) && predicate(nameProperty.GetString() ?? ""))
            {
                return element;
            }

            if (element.TryGetProperty("item", out var itemProperty))
            {
                foreach (var item in itemProperty.EnumerateArray())
                {
                    var foundItem = FindItemRecursive(item, predicate);
                    if (foundItem.HasValue)
                    {
                        return foundItem;
                    }
                }
            }

            return null;
        }
    }
}
