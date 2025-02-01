using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static class JObjectExtensions
    {
        public static object? GetKeyValue(this JObject? jobject, string key)
        {
            if (jobject == null || string.IsNullOrWhiteSpace(key))
                return null;

            // Retrieve the value associated with the specified key
            var token = jobject[key];

            if (token == null)
                return null;

            // Handle based on the JToken type
            return token.Type switch
            {
                JTokenType.Array => token.ToObject<JArray>(), // Return as JArray
                JTokenType.Object => token.ToObject<JObject>(), // Return as JObject
                _ => token.ToString()?.Trim() // Return as a trimmed string for other types
            };
        }
    }

}
