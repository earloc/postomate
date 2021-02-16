using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Postomate
{

    public class PostmanCollection
    {

        readonly JsonDocument doc;
        private readonly Action<string> log;

        public void Log(string message) => log(message);

        public PostmanCollection(string path, Action<string> log)
        {
            var content = File.ReadAllText(path);
            doc = JsonDocument.Parse(content);
            log($"using postman-collection '{Path.GetFullPath(path)}'");
            this.log = log;
        }

        public PostmanFolder FindFolder(string name)
        {
            var item = FindItemRecursive(doc.RootElement, _ => _.Contains(name));

            if (!item.HasValue)
            {
                return new PostmanFolder("root", doc.RootElement, this);
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
