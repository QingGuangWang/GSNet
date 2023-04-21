namespace GSNet.Json
{
    /// <summary>
    /// JSON序列化/反序列化 转换器
    /// </summary>
    public interface IJsonConverter
    {
        /// <summary>
        /// 序列化对象成JSON字符串
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <returns>序列化后的json字符串</returns>
        public string Serialize<T>(T obj);

        /// <summary>
        /// 序列化对象成JSON字符串
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <returns>序列化后的json字符串</returns>
        string SerializeObject(object obj);

        /// <summary>
        /// 序列化对象成JSON字符串，输出到Stream流
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <param name="stream">Stream流</param>
        void SerializeToStream(object obj, Stream stream);

        /// <summary>
        /// 序列化对象成JSON字符串，输出到Stream流
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <param name="type">对象类型</param>
        /// <param name="stream">Stream流</param>
        void SerializeToStream(Type type, object obj, Stream stream);

        /// <summary>
        /// Json字符串反序列化为对象
        /// </summary>
        /// <param name="jsonString">JSON字符串</param>
        /// <typeparam name="T">反序列化的类型</typeparam>
        /// <returns>反序列化出的对象</returns>
        T Deserialize<T>(string jsonString);

        /// <summary>
        /// Json字符串反序列化为指定Type类型的对象
        /// </summary>
        /// <param name="jsonString">JSON字符串</param>
        /// <param name="type">数据类型</param>
        /// <returns>反序列化出的对象</returns>
        object Deserialize(Type type, string jsonString);

        /// <summary>
        /// 从Stream流读取，进行反序列化
        /// </summary>
        /// <param name="type">反序列化的类型</param>
        /// <param name="stream">Stream流</param>
        /// <returns>反序列化出的对象</returns>
        object Deserialize(Type type, Stream stream);

    }
}