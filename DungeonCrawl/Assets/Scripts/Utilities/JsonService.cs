using Newtonsoft.Json;
using Zenject;

namespace Utilities
{
    public static class JsonService
    {
        private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        
        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _jsonSerializerSettings);
        }
        
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
        }
    }
}