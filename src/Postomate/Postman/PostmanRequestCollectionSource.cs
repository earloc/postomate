using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using static Postomate.RequestCollection;

namespace Postomate.Postman
{
    public class PostmanRequestCollectionSource : IRequestCollectionSource
    {
        private readonly string path;

        public PostmanRequestCollectionSource(string path, Action<string> log)
        {
            this.path = path;
            log?.Invoke($"using postman-collection '{Path.GetFullPath(path)}'");
        }

        public JsonDocument Parse()
        {
            var content = Read();
            return JsonDocument.Parse(content);
        }

        public string Read() => File.ReadAllText(path);
    }
}
