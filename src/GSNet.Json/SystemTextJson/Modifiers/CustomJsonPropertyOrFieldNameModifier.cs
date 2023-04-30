using GSNet.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace GSNet.Json.SystemTextJson.Modifiers
{
    /// <summary>
    /// 配置序列化和反序列化时 JSON 中存在的属性/字段名称，实现与 [JsonPropertyName] 特性一样的能力，用于对于不能使用属性注释，或者不想直接依赖的时候。
    /// </summary>
    public class CustomJsonPropertyOrFieldNameModifier : IJsonTypeInfoModifier
    {
        private readonly IDictionary<Type, IList<Tuple<MemberInfo, string>>> _customPropertyNameDictionary = new Dictionary<Type, IList<Tuple<MemberInfo, string>>>();
        
        public void ModifyJsonTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
                return;

            //判断是否定义序列化/反序列化的名称
            if (_customPropertyNameDictionary.TryGetValue(jsonTypeInfo.Type, out var customPropertyNameTuples))
            {
                foreach (var customPropertyNameTuple in customPropertyNameTuples)
                {
                    var memberInfo = customPropertyNameTuple.Item1;

                    var propertyInfo = jsonTypeInfo.Properties.FirstOrDefault(x =>
                    {
                        //正常情况下AttributeProvider都是MemberInfo，除非用jsonTypeInfo.CreateJsonPropertyInfo等方式自己定义的
                        if (x.AttributeProvider is MemberInfo attributeProvider)
                        {
                            return attributeProvider.Name == memberInfo.Name;
                        }

                        return false;
                    });
                    
                    if (propertyInfo != null)
                    {
                        propertyInfo.Name = customPropertyNameTuple.Item2;
                    }
                }
            }
        }

        /// <summary>
        /// 配置自定义的公开的属性/字段名称
        /// </summary>
        /// <typeparam name="TDestination">序列化/反序列化的对象</typeparam>
        /// <param name="destinationMemberLambdaExpression">指向其成员的Lambda表达式</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public CustomJsonPropertyOrFieldNameModifier ConfigCustomPropertyOrFieldName<TDestination>(
            Expression<Func<TDestination, object>> destinationMemberLambdaExpression, string name)
        {
            var memberInfo = ExpressionHelper.GetMemberInfo(destinationMemberLambdaExpression);

            if (_customPropertyNameDictionary.ContainsKey(typeof(TDestination)))
            {
                _customPropertyNameDictionary[typeof(TDestination)].Add(new Tuple<MemberInfo, string>(memberInfo, name));
            }
            else
            {
                _customPropertyNameDictionary.Add(typeof(TDestination), new List<Tuple<MemberInfo, string>>() { new Tuple<MemberInfo, string>(memberInfo, name) });
            }

            return this;
        }
    }
}
