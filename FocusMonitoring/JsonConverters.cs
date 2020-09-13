using System;
using System.Collections.Generic;
using System.Linq;
using FocusAccess;
using Newtonsoft.Json;

namespace FocusMonitoring
{
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new QueryConverter(),
                new ApiMethodConverter()
            }
        };
    }
    
    public class ApiMethodConverter : JsonConverter<ApiMethodEnum>
    {
        public override void WriteJson(JsonWriter writer, ApiMethodEnum value, JsonSerializer serializer) => 
            serializer.Serialize(writer,value.ToString());

        public override ApiMethodEnum ReadJson(JsonReader reader, Type objectType, ApiMethodEnum existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return default;
            var value = serializer.Deserialize<string>(reader);
            return value switch
            {
                "req" => ApiMethodEnum.req,
                "analytics" => ApiMethodEnum.analytics,
                "contacts" => ApiMethodEnum.contacts,
                "egrDetails" => ApiMethodEnum.egrDetails,
                _ => throw new NotImplementedException($"{value} method unnknown or not implemented")// ArgumentOutOfRangeException(nameof(existingValue), existingValue, null);
            };
        }
    }
    
    public class QueryConverter : JsonConverter<IQuery>
    {
        public override void WriteJson(JsonWriter writer, IQuery value, JsonSerializer serializer)=> 
            serializer.Serialize(writer,value.AssembleQuery());

        public override IQuery ReadJson(JsonReader reader, Type objectType, IQuery existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var str =  (string) reader.Value;
            return reader.TokenType == JsonToken.Null
                ? null
                : new DeserializedQuery(str);
        }
    }

    public class DeserializedQuery : Query
    {
        public DeserializedQuery(string query)
        {
            var pairs = query.Split('&');
            var keys = new string[pairs.Length];
            var values = new string[pairs.Length];
            for(var i =0 ;i<pairs.Length;i++)
            {
                var keyValue = pairs[i].Split('=');
                keys[i] = keyValue[0];
                values[i] = keyValue[1];
            }

            Keys = keys;
            Values = values;
        }

        public override string[] Keys { get; }
       
    }
}