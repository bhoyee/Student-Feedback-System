using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using API.Entities;

namespace API.Entities
{
   public class FeedbackStatusConverter : JsonConverter<FeedbackStatus>
    {
        public override FeedbackStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (FeedbackStatus)reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, FeedbackStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}