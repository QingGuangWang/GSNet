using GSNet.Common;
using StackExchange.Redis;

namespace GSNet.Redis.StackExchange
{
    /// <summary>
    /// 连接管理
    /// </summary>
    public static class RedisConnectionHelp
    {
        /// <summary>
        /// 连接Redis
        /// </summary>
        /// <param name="connectionOptions">连接选项</param>
        public static IConnectionMultiplexer Connect(RedisConnectionOptions connectionOptions)
        {
            IConnectionMultiplexer connectionMultiplexer;
            
            if (connectionOptions.ConfigurationOptions != null)
            {
                connectionMultiplexer = ConnectionMultiplexer.Connect(connectionOptions.ConfigurationOptions);
            }
            else
            {
                connectionMultiplexer = ConnectionMultiplexer.Connect(connectionOptions.Configuration);
            }

            RegisterProfiler(connectionMultiplexer, connectionOptions);

            return connectionMultiplexer;
        }

        /// <summary>
        /// 异步连接Redis
        /// </summary>
        /// <param name="connectionOptions">连接选项</param>
        /// <param name="token">取消令牌</param>
        public static async Task<IConnectionMultiplexer> ConnectAsync(RedisConnectionOptions connectionOptions, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            IConnectionMultiplexer connectionMultiplexer;

            if (connectionOptions.ConfigurationOptions != null)
            {
                connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(connectionOptions.ConfigurationOptions).ConfigureAwait(false);
            }
            else
            {
                connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(connectionOptions.Configuration).ConfigureAwait(false);
            }

            RegisterProfiler(connectionMultiplexer, connectionOptions);

            return connectionMultiplexer;
        }

        /// <summary>
        /// 注册Redis分析器
        /// </summary> 
        private static void RegisterProfiler(IConnectionMultiplexer connectionMultiplexer, RedisConnectionOptions connectionOptions)
        {
            Check.Argument.IsNotNull(connectionMultiplexer, nameof(connectionMultiplexer));
            Check.Argument.IsNotNull(connectionMultiplexer, nameof(connectionOptions));
            
            if (connectionOptions.ProfilingSession != null)
            {
                connectionMultiplexer.RegisterProfiler(connectionOptions.ProfilingSession);
            }
        }
    }
}
