﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System.Text.Json
{
    internal static class JsonElementExtensions
    {
        public static JsonElement? TryGetProperty(this JsonElement that, string name)
        {
            if (that.TryGetProperty(name, out var property))
            {
                return property;
            }

            return default;
        }

        public static JsonElement RequireProperty(this JsonElement that, string name)
        {
            if (that.TryGetProperty(name, out var property))
            {
                return property;
            }
            throw new KeyNotFoundException($"The property '{name}' was not found on the specified JsonElement");
        }

    }
}
