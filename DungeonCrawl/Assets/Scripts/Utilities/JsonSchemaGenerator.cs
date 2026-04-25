using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace DefaultNamespace.Utilities
{
    public static class JsonSchemaGenerator
    {
        public static string GenerateTemplateAsString(Type type)
        {
            var obj = new Dictionary<string, object>();

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propType = prop.PropertyType;

                var underlying = Nullable.GetUnderlyingType(propType);
                var isNullable = underlying != null;
                var actualType = underlying ?? propType;

                if (actualType.IsEnum)
                {
                    var values = Enum.GetNames(actualType);
                    obj[prop.Name] = string.Join("|", values);
                }
                else if (actualType == typeof(string))
                {
                    obj[prop.Name] = "";
                }
                else if (actualType == typeof(Guid))
                {
                    obj[prop.Name] = Guid.Empty;
                }
                else if (actualType.FullName == "UnityEngine.Vector2")
                {
                    obj[prop.Name] = Activator.CreateInstance(actualType);
                }
                else if (actualType.IsValueType)
                {
                    obj[prop.Name] = Activator.CreateInstance(actualType);
                }
                else
                {
                    obj[prop.Name] = null;
                }
            }

            return JsonService.Serialize( obj );
        }
    }
}