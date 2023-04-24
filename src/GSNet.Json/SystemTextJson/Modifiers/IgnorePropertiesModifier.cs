using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace GSNet.Json.SystemTextJson.Modifiers
{
    /// <summary>
    /// 配置忽略属性，实现与 [JsonIgnore] 特性一样的能力，用于对于不能使用属性注释，或者不想直接依赖的时候。
    /// </summary>
    public class IgnorePropertiesModifier : IJsonTypeInfoModifier
    {
        private readonly IDictionary<Type, IList<string>> _typeIgnorePropertiesDict = new Dictionary<Type, IList<string>>();

        public void ModifyJsonTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
                return;

            //判断是否需要忽略属性
            if (_typeIgnorePropertiesDict.TryGetValue(jsonTypeInfo.Type, out var ignoreProperties))
            {
                foreach (var ignorePropertyName in ignoreProperties)
                {
                    var propertyInfo = jsonTypeInfo.Properties.FirstOrDefault(x => x.Name == ignorePropertyName);

                    if (propertyInfo != null)
                    {
                        jsonTypeInfo.Properties.Remove(propertyInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 通过表达式，配置在序列化/反序列化的类型（<typeparamref name="TDestination"/>）的时候，其需要忽略的属性
        /// </summary>
        /// <typeparam name="TDestination">序列化/反序列化的类型</typeparam>
        /// <param name="destinationMemberLambdaExpression">指向其属性成员的Lambda表达式</param>
        public IgnorePropertiesModifier AddIgnoreProperty<TDestination>(Expression<Func<TDestination, object>> destinationMemberLambdaExpression)
        {
            var pInfo = GetPropertyInfo(destinationMemberLambdaExpression);

            if (_typeIgnorePropertiesDict.ContainsKey(typeof(TDestination)))
            {
                _typeIgnorePropertiesDict[typeof(TDestination)].Add(pInfo.Name);
            }
            else
            {
                _typeIgnorePropertiesDict.Add(typeof(TDestination), new List<string>() { pInfo.Name });
            }

            return this;
        }

        /// <summary>
        /// 配置在序列化/反序列化的类型（<paramref name="type"/>）的时候，其需要忽略的属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public IgnorePropertiesModifier AddIgnoreProperty(Type type, string propertyName)
        {
            if (_typeIgnorePropertiesDict.TryGetValue(type, out var propertyList))
            {
                if (!propertyList.Contains(propertyName))
                {
                    propertyList.Add(propertyName);
                }
            }
            else
            {
                _typeIgnorePropertiesDict.Add(type, new List<string>() { propertyName });
            }

            return this;
        }

        /// <summary>
        /// 通过lambdaExpression获取属性PropertyInfo
        /// </summary>
        private PropertyInfo GetPropertyInfo<TDestination>(Expression<Func<TDestination, object>> destinationMemberLambdaExpression)
        {
            //获取LambdaExpression 的主体 如x => x.b  则获取到 x.b
            var lambdaExpressionBody = ((LambdaExpression)destinationMemberLambdaExpression).Body;
            // x.b 属于 MemberExpression 或者 UnaryExpression 

            if (lambdaExpressionBody is MemberExpression expression)
            {
                //获取Member , 这里不做具体校验，所以调用配置必须是 属性 
                var memberInfo = expression.Member;
                var pInfo = (PropertyInfo)memberInfo;

                return pInfo;
            }
            else
            {
                var unaryExpression = ((UnaryExpression)lambdaExpressionBody);

                var memberExpression = unaryExpression.Operand as MemberExpression;
                var memberInfo = memberExpression.Member;
                var pInfo = (PropertyInfo)memberInfo;

                return pInfo;
            }
        }
    }
}
