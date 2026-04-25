using System;
using Newtonsoft.Json;
using UnityEngine;

public class Vector2Converter : JsonConverter<Vector2>
{
    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("x");
        writer.WriteValue(value.x);

        writer.WritePropertyName("y");
        writer.WriteValue(value.y);

        writer.WriteEndObject();
    }

    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        float x = 0;
        float y = 0;

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                string prop = reader.Value.ToString();
                reader.Read();

                if (prop == "x") x = Convert.ToSingle(reader.Value);
                if (prop == "y") y = Convert.ToSingle(reader.Value);
            }

            if (reader.TokenType == JsonToken.EndObject)
                break;
        }

        return new Vector2(x, y);
    }
}