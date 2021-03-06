﻿using System.Text.Json;

namespace System
{
    internal static class JsonExtensions
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
