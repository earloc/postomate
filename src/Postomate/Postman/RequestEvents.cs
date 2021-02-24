using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Postomate.Postman
{
    public class RequestEvents
    {
        public RequestEvents(JsonElement? eventProperty)
        {
            var events = eventProperty?.EnumerateArray() ?? Enumerable.Empty<JsonElement>();

            foreach (var eventElement in events)
            {
                var listen = eventElement.TryGetProperty("listen")?.GetString();

                switch (listen)
                {
                    case "prerequest": PreRequestScript = eventElement
                        .TryGetProperty("script")?
                        .TryGetProperty("exec")?
                        .EnumerateArray()
                        .Select(x => x.GetString() ?? "")
                        .ToArray().AsEnumerable() ?? Enumerable.Empty<string>();
                        break;

                    case null: break;
                }
            }
        }

        public IEnumerable<string> PreRequestScript { get; }
    }
}