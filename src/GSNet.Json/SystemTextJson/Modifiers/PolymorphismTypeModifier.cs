using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using GSNet.Common.Extensions;

namespace GSNet.Json.SystemTextJson.Modifiers
{
    /// <summary>
    /// 配置多态方式序列化，实现与 [JsonPolymorphic]，[JsonDerivedType] 等特性一样的能力，用于对于不能使用属性注释，或者不想直接依赖的时候。
    /// </summary>
    public class PolymorphismTypeModifier : IJsonTypeInfoModifier
    {
        private readonly IDictionary<Type, JsonPolymorphismOptions> _jsonPolymorphismOptionsMap = new Dictionary<Type, JsonPolymorphismOptions>();

        public void ModifyJsonTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            {
                return;
            }

            //判断是否需要配置
            if (_jsonPolymorphismOptionsMap.TryGetValue(jsonTypeInfo.Type, out var jsonPolymorphismOptions))
            {
                jsonTypeInfo.PolymorphismOptions = jsonPolymorphismOptions;
            }
        }

        /// <summary>
        /// 配置在序列化/反序列化的类型（<typeparamref name="T"/>）的时候，配置其多态方式序列化
        /// </summary>
        /// <typeparam name="T">序列化/反序列化的类型</typeparam>
        /// <param name="jsonDerivedTypes"><typeparamref name="T"/>的子类型的类型鉴别器</param>
        /// <param name="discriminatorPropertyName">自定义类型鉴别器名称，为空的情况下则认为不配置，使用默认（类型鉴别器的默认属性名称为 $type）</param>
        public PolymorphismTypeModifier ConfigPolymorphism<T>(IList<JsonDerivedType> jsonDerivedTypes, string discriminatorPropertyName = "")
        {
            return ConfigPolymorphism(typeof(T), jsonDerivedTypes, discriminatorPropertyName);
        }

        /// <summary>
        /// 配置在序列化/反序列化的类型（<paramref name="type"/>）的时候，配置其多态方式序列化
        /// </summary>
        /// <param name="type">序列化/反序列化的类型</param>
        /// <param name="jsonDerivedTypes"><typeparamref name="T"/>的子类型的类型鉴别器</param>
        /// <param name="discriminatorPropertyName">自定义类型鉴别器名称，为空的情况下则认为不配置，使用默认（类型鉴别器的默认属性名称为 $type）</param>
        public PolymorphismTypeModifier ConfigPolymorphism(Type type, IList<JsonDerivedType> jsonDerivedTypes, string discriminatorPropertyName = "")
        {
            var options = new JsonPolymorphismOptions();
            
            if (discriminatorPropertyName.NotNullOrBlank())
            {
                options.TypeDiscriminatorPropertyName = discriminatorPropertyName;
            }

            foreach (var derivedType in jsonDerivedTypes)
            {
                options.DerivedTypes.Add(derivedType);
            }
            
            if (!_jsonPolymorphismOptionsMap.ContainsKey(type))
            {
                _jsonPolymorphismOptionsMap.Add(type, options);
            }
            else
            {
                _jsonPolymorphismOptionsMap[type] = options;
            }

            return this;
        }

        /// <summary>
        /// 配置在序列化/反序列化的类型（<typeparamref name="T"/>）的时候，配置其多态方式序列化
        /// </summary>
        /// <typeparam name="T">序列化/反序列化的类型</typeparam>
        /// <param name="options"><typeparamref name="T"/>的多态方式序列化配置信息</param>
        public PolymorphismTypeModifier ConfigJsonPolymorphismOptions<T>(JsonPolymorphismOptions options)
        {
            return ConfigJsonPolymorphismOptions(typeof(T), options);
        }

        /// <summary>
        /// 配置在序列化/反序列化的类型（<paramref name="type"/>）的时候，配置其多态方式序列化
        /// </summary>
        /// <param name="type">序列化/反序列化的类型</param>
        /// <param name="options"><paramref name="type"/>的多态方式序列化配置信息</param>
        public PolymorphismTypeModifier ConfigJsonPolymorphismOptions(Type type, JsonPolymorphismOptions options)
        {
            if (!_jsonPolymorphismOptionsMap.ContainsKey(type))
            {
                _jsonPolymorphismOptionsMap.Add(type, options);
            }
            else
            {
                _jsonPolymorphismOptionsMap[type] = options;
            }

            return this;
        }
    }
}
