using StackExchange.Redis;
using StackExchange.Redis.Profiling;

namespace GSNet.Redis.StackExchange
{
    /// <summary>
    /// Redis 配置选项
    /// </summary>
    public class RedisConnectionOptions
    {
        /// <summary>
        /// 用于连接到Redis的配置
        /// Redis的连接字符串，eg: 127.0.0.1:6379,defaultDatabase=6,connectTimeout=5000,name=clientName
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// 用于连接到Redis的配置，这是首选配置。若该对象不为空，则取该对象作为连接配置，若该对象为空，则取<see cref="Configuration"/>属性。
        /// 来自StackExchange.Redis的类。
        /// </summary>
        public ConfigurationOptions ConfigurationOptions { get; set; }

        /// <summary>
        /// 获取或设置用于创建ConnectionMultiplexer实例的委托。 若该属性为空，则以<see cref="ConfigurationOptions"/>属性作为配置。
        /// </summary>
        public Func<Task<IConnectionMultiplexer>> ConnectionMultiplexerFactory { get; set; }

        /// <summary>
        ///  Redis 性能分析 （StackExchange.Redis公开了一些方法和类型来启用性能分析）
        /// </summary>
        public Func<ProfilingSession> ProfilingSession { get; set; }

        /// <summary>
        /// Redis实例名
        /// </summary>
        public string InstanceName { get; set; }
    }
}
