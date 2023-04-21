using Newtonsoft.Json;

namespace GSNet.Json.NewtonsoftJson
{
    /// <summary>
    ///  基于微软官方的JSON类库 Newtonsoft.Json / Json.Net 实现 IJsonConverter
    /// </summary>
    public class NewtonsoftJsonConverter : IJsonConverter
    {
        /// <summary>
        /// 序列化配置
        /// </summary>
        private JsonSerializerSettings SerializerSettings { get; set; }

        /// <summary>
        /// </summary>
        public NewtonsoftJsonConverter()
        {
        }

        /// <summary>  
        /// 基于指定的序列化配置构建
        /// </summary>
        /// <param name="serializerSettings">序列化配置</param>
        public NewtonsoftJsonConverter(JsonSerializerSettings serializerSettings)
        {
            SerializerSettings = serializerSettings;
        }

        /// <summary>
        /// 序列化对象成JSON字符串
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <returns>序列化后的json字符串</returns>
        public string Serialize<T>(T obj)
        {
            if (SerializerSettings != null)
            {
                return JsonConvert.SerializeObject(obj, typeof(T), SerializerSettings);
            }

            return JsonConvert.SerializeObject(obj, typeof(T), null);
        }

        /// <summary>
        /// 序列化对象成JSON字符串
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <returns>序列化后的json字符串</returns>
        public string SerializeObject(object obj)
        {
            if (SerializerSettings != null)
            {
                return JsonConvert.SerializeObject(obj, SerializerSettings);
            }

            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Json字符串反序列化为对象
        /// </summary>
        /// <param name="jsonString">JSON字符串</param>
        /// <typeparam name="T">反序列化的类型</typeparam>
        /// <returns>反序列化出的对象</returns>
        public T Deserialize<T>(string jsonString)
        {
            if (SerializerSettings != null)
            {
                return JsonConvert.DeserializeObject<T>(jsonString, SerializerSettings);
            }

            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 从Stream流读取，进行反序列化
        /// </summary>
        /// <param name="type">反序列化的类型</param>
        /// <param name="stream">Stream流</param>
        /// <returns>反序列化出的对象</returns>
        public object Deserialize(Type type, Stream stream)
        {
            using StreamReader reader = new StreamReader(stream);
            using JsonTextReader jsonReader = new JsonTextReader(reader);

            var serializer = SerializerSettings != null ? JsonSerializer.Create(SerializerSettings) : JsonSerializer.CreateDefault();

            return new JsonSerializer().Deserialize(jsonReader, type);
        }

        /// <summary>
        /// Json字符串反序列化为指定Type类型的对象
        /// </summary>
        /// <param name="jsonString">JSON字符串</param>
        /// <param name="type">数据类型</param>
        /// <returns>反序列化出的对象</returns>
        public object Deserialize(Type type, string jsonString)
        {
            if (SerializerSettings != null)
            {
                return JsonConvert.DeserializeObject(jsonString, type, SerializerSettings);
            }

            return JsonConvert.DeserializeObject(jsonString, type);
        }

        /// <summary>
        /// 序列化对象成JSON字符串，输出到Stream流
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <param name="stream">Stream流</param>
        public void SerializeToStream(object obj, Stream stream)
        {
            using StreamWriter writer = new StreamWriter(stream);
            using JsonTextWriter jsonWriter = new JsonTextWriter(writer);

            var serializer = SerializerSettings != null ? JsonSerializer.Create(SerializerSettings) : JsonSerializer.CreateDefault();
            serializer.Serialize(jsonWriter, obj);
            jsonWriter.Flush();
        }

        /// <summary>
        /// 序列化对象成JSON字符串，输出到Stream流
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <param name="type">对象类型</param>
        /// <param name="stream">Stream流</param>
        public void SerializeToStream(Type type, object obj, Stream stream)
        {
            using StreamWriter writer = new StreamWriter(stream);
            using JsonTextWriter jsonWriter = new JsonTextWriter(writer);

            var serializer = SerializerSettings != null ? JsonSerializer.Create(SerializerSettings) : JsonSerializer.CreateDefault();
            serializer.Serialize(jsonWriter, obj, type);
            jsonWriter.Flush();
        }
    }
}