using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace GSNet.Json.SystemTextJson.Converters
{
    /// <summary>
    /// 该转换器是用于 兼容字符串转换为boolean值类型的。
    /// 因为有时候json中，含有字符串类型的布尔值如 "true"或者"false", 而对象的数据类型是 boolean。
    /// </summary>
    public class StringBooleanConverter : JsonConverter<bool>
    {
        public bool IsWriteToString { get; }


        public StringBooleanConverter() : this(false)
        {
        }

        public StringBooleanConverter(bool isWriteToString)
        {
            IsWriteToString = isWriteToString;
        }
        
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //判断是否是字符串
            if (reader.TokenType == JsonTokenType.String)
            {
                var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

                if (Utf8Parser.TryParse(span, out bool b1, out var bytesConsumed) && span.Length == bytesConsumed)
                {
                    return b1;
                }

                if (bool.TryParse(reader.GetString(), out var b2))
                {
                    return b2;
                }
            }

            //按照默认返回对应布尔值
            return reader.GetBoolean();
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            if (IsWriteToString)
            {
                writer.WriteStringValue(value.ToString());
            }
            else
            {
                writer.WriteBooleanValue(value);
            }
        }
    }
}