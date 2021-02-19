using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Postomate.Postman
{

    public class PostmanCollection
    {

        readonly JsonDocument rawContent;
        private readonly Action<string> log;

        public void Log(string message) => log(message);

        public static PostmanCollection Load(string path, Action<string>? log = null)
        {
            var content = File.ReadAllText(path);
            var json = JsonDocument.Parse(content);

            log?.Invoke($"using postman-collection '{Path.GetFullPath(path)}'");

            return new PostmanCollection(json, log);
        }

        public PostmanCollection(JsonDocument rawContent, Action<string>? log = null)
        {
            this.rawContent = rawContent;
            this.log = log ?? new Action<string>((message) => Console.WriteLine(message));
        }


        public PostmanFolder FindFolder(string name)
        {
            var item = FindItemRecursive(rawContent.RootElement, _ => _.Contains(name));

            if (!item.HasValue)
            {
                return new PostmanFolder("root", rawContent.RootElement, this);
            }
            return new PostmanFolder(name, item.Value, this);
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
