using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System
{
    public static class JsonExtensions
    {
        public static string ToJson(this object that, bool indented)
        {
            return JsonSerializer.Serialize(that, new JsonSerializerOptions()
            {
                WriteIndented = indented
            });
        }

    }
}
