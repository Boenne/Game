using System;
using Newtonsoft.Json;

namespace Game.Model
{
    [JsonConverter(typeof(UrnJsonConverter))]
    public class Urn
    {
        private readonly string _value;

        public Urn(string value)
        {
            _value = value;
        }

        public static implicit operator string(Urn urn)
        {
            return urn._value;
        }

        public static implicit operator Urn(string input)
        {
            return new Urn(input);
        }

        public override bool Equals(object? obj)
        {
            var urn = obj as Urn;
            if (urn == null) return false;
            return urn.ToString() == ToString();
        }

        public override string ToString()
        {
            return _value;
        }
    }

    public class UrnJsonConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return new Urn((string) reader.Value);
        }
    }
}