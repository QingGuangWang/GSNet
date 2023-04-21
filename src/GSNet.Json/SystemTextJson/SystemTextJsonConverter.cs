using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GSNet.Json.SystemTextJson
{
    /// <summary>
    /// 基于.net core 3.0后微软官方出的新JSON类库 System.Text.Json 实现 IJsonConverter
    /// System.Text.Json 相对轻量级，有些复杂序列化情况支持程度没有那么强，需要额外编写自定义转换器等方式实现。
    /// 与Newtonsoft.Json相比较可以查看官方说明:  https://learn.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft?pivots=dotnet-7-0&WT.mc_id=DT-MVP-5003918
    /// </summary>
    public class SystemTextJsonConverter : IJsonConverter
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SystemTextJsonConverter()
        {
            _jsonSerializerOptions = JsonSerializerOptionsHelper.CreateDefaultJsonSerializerOptions();
        }

        public SystemTextJsonConverter(JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions ?? JsonSerializerOptionsHelper.CreateDefaultJsonSerializerOptions();
        }

        /// <summary>
        /// 序列化对象成JSON字符串
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <returns>序列化后的json字符串</returns>
        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize<T>(obj, _jsonSerializerOptions);
        }

        /// <summary>
        /// 序列化对象成JSON字符串
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <returns>序列化后的json字符串</returns>
        public string SerializeObject(object obj)
        {
            return JsonSerializer.Serialize(obj, _jsonSerializerOptions);
        }

        /// <summary>
        /// Json字符串反序列化为对象
        /// </summary>
        /// <param name="jsonString">JSON字符串</param>
        /// <typeparam name="T">反序列化的类型</typeparam>
        /// <returns>反序列化出的对象</returns>
        public T Deserialize<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString, _jsonSerializerOptions);
        }

        /// <summary>
        /// 从Stream流读取，进行反序列化
        /// </summary>
        /// <param name="type">反序列化的类型</param>
        /// <param name="stream">Stream流</param>
        /// <returns>反序列化出的对象</returns>
        public object Deserialize(Type type, Stream stream)
        {
            using var reader = new StreamReader(stream);

            string jsonString = reader.ReadToEnd();

            return Deserialize(type, jsonString);
        }

        /// <summary>
        /// Json字符串反序列化为指定Type类型的对象
        /// </summary>
        /// <param name="jsonString">JSON字符串</param>
        /// <param name="type">数据类型</param>
        /// <returns>反序列化出的对象</returns>
        public object Deserialize(Type type, string jsonString)
        {
            return JsonSerializer.Deserialize(jsonString, type, _jsonSerializerOptions);
        }

        /// <summary>
        /// 序列化对象成JSON字符串，输出到Stream流
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <param name="stream">Stream流</param>
        public void SerializeToStream(object obj, Stream stream)
        {
            using var jsonWriter = new Utf8JsonWriter(stream);

            JsonSerializer.Serialize(jsonWriter, obj, _jsonSerializerOptions);
        }

        /// <summary>
        /// 序列化对象成JSON字符串，输出到Stream流
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <param name="type">对象类型</param>
        /// <param name="stream">Stream流</param>
        public void SerializeToStream(Type type, object obj, Stream stream)
        {
            using var jsonWriter = new Utf8JsonWriter(stream);

            JsonSerializer.Serialize(jsonWriter, obj, type, _jsonSerializerOptions);
        }
    }
}
