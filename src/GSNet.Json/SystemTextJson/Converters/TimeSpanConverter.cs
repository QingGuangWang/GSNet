using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace GSNet.Json.SystemTextJson.Converters
{
    /// <summary>
    /// TimeSpan JSON 转换器
    /// System.Text.Json .Net 3.1 的时候，不支持TimeSpan序列化，需要通过转换器处理
    /// 但是在.Net 5 ，.Net 6，.Net 7 的时候，已经支持，则可以不需要用这个转换器
    /// </summary>
    public class JsonTimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new NotSupportedException();
            }

            if (typeToConvert != typeof(TimeSpan))
            {
                throw new NotSupportedException();
            }

            // 使用常量 "c" 來指定用 [-][d.]hh:mm:ss[.fffffff] 作为 TimeSpans 转换的格式
            return TimeSpan.ParseExact(reader.GetString() ?? string.Empty, "c", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("c", CultureInfo.InvariantCulture));
        }
    }
}
