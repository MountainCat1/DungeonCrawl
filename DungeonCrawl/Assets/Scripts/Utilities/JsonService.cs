using System.Collections.Generic;
using Newtonsoft.Json;

namespace DefaultNamespace
{
    public static class JsonService
    {
        public static string Serialize(object json)
        {
            return JsonConvert.SerializeObject(
                json,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Converters = new List<JsonConverter>
                    {
                        new Vector2Converter()
                    }
                }
            );
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(
                json,
                new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                    {
                        new Vector2Converter()
                    }
                }
            );
        }
    }
}