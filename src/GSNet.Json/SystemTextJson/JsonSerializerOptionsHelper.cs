using GSNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace GSNet.Json.SystemTextJson
{

    /// <summary>
    /// JsonSerializerOptions 辅助类
    /// 内部维护一个JsonSerializerOptions缓存，每一个对象关联到一个名称。
    /// 系统启动时候会默认创建一个名为 default 的JsonSerializerOptions
    /// </summary>
    public static class JsonSerializerOptionsHelper
    {
        /// <summary>
        /// 用于缓存
        /// </summary>
        private static readonly IDictionary<string, JsonSerializerOptions> JsonSerializerOptionsStorage = new Dictionary<string, JsonSerializerOptions>();

        private static readonly object SyncLockObj = new object();

        static JsonSerializerOptionsHelper()
        {
            JsonSerializerOptionsStorage.Add("default", CreateDefaultJsonSerializerOptions());
        }

        /// <summary>
        /// 注册一个JsonSerializerOptions到内置的内存缓存中，且指定一个名字（参数：<paramref name="name"/>）
        /// </summary>
        /// <param name="name">关联一个JsonSerializerOptions对象的名称</param>
        /// <param name="options">JsonSerializerOptions对象</param>
        /// <param name="isReplaceExisting">如果已经注册，是否替换，默认false</param>
        public static void RegisterJsonSerializerOptions(string name, JsonSerializerOptions options, bool isReplaceExisting = false)
        {
            RegisterJsonSerializerOptions(name, () => options, isReplaceExisting);
        }

        /// <summary>
        /// 注册一个JsonSerializerOptions到内置的内存缓存中，且指定一个名字（参数：<paramref name="name"/>）
        /// </summary>
        /// <param name="name">关联一个JsonSerializerOptions对象的名称</param>
        /// <param name="createFunc">创建JsonSerializerOptions对象的委托方法</param>
        /// <param name="isReplaceExisting">如果已经注册，是否替换，默认false</param>
        public static void RegisterJsonSerializerOptions(string name, Func<JsonSerializerOptions> createFunc, bool isReplaceExisting = false)
        {
            Check.Argument.IsNotNullOrEmpty(name, nameof(name));
            Check.Argument.IsNotNull(createFunc, nameof(createFunc));

            if (JsonSerializerOptionsStorage.ContainsKey(name))
            {
                if (isReplaceExisting)
                {
                    JsonSerializerOptionsStorage[name] = createFunc();
                    return;
                }

                throw new ArgumentException($"The JsonSerializerOptions {name} has been registered");
            }

            JsonSerializerOptionsStorage[name] = createFunc();
        }

        /// <summary>
        /// 根据给定的名字（参数：<paramref name="name"/>）,从内存缓存中获取对应的JsonSerializerOptions对象，若不存在，根据指定的委托方法（参数：<paramref name="createFunc"/>）创建。
        /// </summary>
        /// <param name="name">关联一个JsonSerializerOptions对象的名称</param>
        /// <param name="createFunc">创建JsonSerializerOptions对象的委托方法</param>
        /// <returns>JsonSerializerOptions东西</returns>
        public static JsonSerializerOptions GetOrCreateJsonSerializerOptions(string name, Func<JsonSerializerOptions> createFunc)
        {
            Check.Argument.IsNotNullOrEmpty(name, nameof(name));
            Check.Argument.IsNotNull(createFunc, nameof(createFunc));

            if (JsonSerializerOptionsStorage.ContainsKey(name))
            {
                return JsonSerializerOptionsStorage[name];
            }
            else
            {
                lock (SyncLockObj)
                {
                    JsonSerializerOptionsStorage.TryAdd(name, createFunc());
                }

                return JsonSerializerOptionsStorage[name];
            }
        }

        /// <summary>
        /// 根据给定的名字（参数：<paramref name="name"/>）,从内存缓存中获取对应的JsonSerializerOptions对象，若不存在，则返回一个默认的。
        /// </summary>
        /// <param name="name">关联一个JsonSerializerOptions对象的名称</param>
        /// <returns>JsonSerializerOptions东西</returns>
        public static JsonSerializerOptions GetOrReturnDefaultJsonSerializerOptions(string name)
        {
            Check.Argument.IsNotNullOrEmpty(name, nameof(name));

            if (JsonSerializerOptionsStorage.ContainsKey(name))
            {
                return JsonSerializerOptionsStorage[name];
            }
            else
            {
                return JsonSerializerOptionsStorage["default"];
            }
        }

        /// <summary>
        /// 创建一个新的默认序列化选项
        /// 基于
        /// --》UnsafeRelaxedJsonEscaping 的 Encoder
        /// --》允许（和忽略）对象或数组中 JSON 值的列表末尾多余的逗号
        /// </summary>
        /// <returns>JsonSerializerOptions对象</returns>
        public static JsonSerializerOptions CreateDefaultJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                //避免中文序列化成 类似"\u4ee3\u7801\u6539\u53d8\u4e16\u754c", UnicodeRanges.All对于一些符号，也还是会被转义
                //Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All),
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, //避免中文，特殊字符等被转义成 \uXXXX
                //是否在序列化时进行整齐打印，避免字符串过大，直接false
                WriteIndented = false,
                //该值确定是否 null 在序列化过程中忽略值, 一般不要忽略
                //IgnoreNullValues = false, //过时了
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                //反序列化的 JSON 有效负载中是否允许（和忽略）对象或数组中 JSON 值的列表末尾多余的逗号
                AllowTrailingCommas = true
            };

            return options;
        }
    }
}
