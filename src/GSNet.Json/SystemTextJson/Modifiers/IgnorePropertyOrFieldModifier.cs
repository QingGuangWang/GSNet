using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using GSNet.Common.Extensions;
using GSNet.Common.Helper;

namespace GSNet.Json.SystemTextJson.Modifiers
{
    /// <summary>
    /// 配置忽略属性或者字段，实现与 [JsonIgnore] 特性一样的能力，用于对于不能使用属性注释，或者不想直接依赖的时候。
    /// </summary>
    public class IgnorePropertyOrFieldModifier : IJsonTypeInfoModifier
    {
        private readonly IDictionary<Type, IList<string>> _typeIgnoreNameDict = new Dictionary<Type, IList<string>>();

        private readonly IDictionary<Type, IList<MemberInfo>> _typeIgnoreMemberDict = new Dictionary<Type, IList<MemberInfo>>(); 
        
        public void ModifyJsonTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
                return;

            //根据属性或者字段去忽略
            if (_typeIgnoreMemberDict.TryGetValue(jsonTypeInfo.Type, out var memberInfos))
            {
                foreach (var memberInfo in memberInfos)
                {
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
                        jsonTypeInfo.Properties.Remove(propertyInfo);
                    }
                }
            }

            //根据JSON字段名称去忽略
            if (_typeIgnoreNameDict.TryGetValue(jsonTypeInfo.Type, out var ignoreProperties))
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
        /// 通过表达式，配置在序列化/反序列化的类型（<typeparamref name="TDestination"/>）的时候，其需要忽略的公开的属性/字段
        /// </summary>
        /// <typeparam name="TDestination">序列化/反序列化的类型</typeparam>
        /// <param name="destinationMemberLambdaExpression">指向其属性成员的Lambda表达式</param>
        public IgnorePropertyOrFieldModifier AddIgnorePropertyOrField<TDestination>(Expression<Func<TDestination, object>> destinationMemberLambdaExpression)
        {
            var memberInfo = ExpressionHelper.GetMemberInfo(destinationMemberLambdaExpression);

            if (_typeIgnoreMemberDict.ContainsKey(typeof(TDestination)))
            {
                _typeIgnoreMemberDict[typeof(TDestination)].Add(memberInfo);
            }
            else
            {
                _typeIgnoreMemberDict.Add(typeof(TDestination), new List<MemberInfo>() { memberInfo });
            }

            return this;
        }

        /// <summary>
        /// 配置在序列化/反序列化的类型（<paramref name="type"/>）的时候，其需要忽略的公开的属性/字段
        /// </summary>
        /// <param name="type">序列化/反序列化的类型</param>
        /// <param name="memberName">属性/字段名称</param>
        /// <returns></returns>
        public IgnorePropertyOrFieldModifier AddIgnorePropertyOrField(Type type, string memberName)
        {
            var memberInfo = type.GetPublicPropertyOrField(memberName);

            if (memberInfo == null)
            {
                throw new ArgumentException($@"Cannot find property or field named [{memberName}] on [{type}]", nameof(memberName));
            }
            
            if (_typeIgnoreMemberDict.TryGetValue(type, out var memberInfos))
            {
                if (!memberInfos.Contains(memberInfo))
                {
                    memberInfos.Add(memberInfo);
                }
            }
            else
            {
                _typeIgnoreMemberDict.Add(type, new List<MemberInfo>() { memberInfo });
            }

            return this;
        }

        /// <summary>
        /// 配置在序列化/反序列化的类型（<paramref name="type"/>）的时候，其需要忽略的JSON字段名称,
        /// 这个主要针对一些前面改名的，或者自定义插入JsonPropertyInfo等。
        /// </summary>
        /// <param name="type">序列化/反序列化的类型</param>
        /// <param name="name">要忽略的JSON字段名称</param>
        /// <returns></returns>
        public IgnorePropertyOrFieldModifier AddIgnoreName(Type type, string name)
        {
            if (_typeIgnoreNameDict.TryGetValue(type, out var memberInfos))
            {
                if (!memberInfos.Contains(name))
                {
                    memberInfos.Add(name);
                }
            }
            else
            {
                _typeIgnoreNameDict.Add(type, new List<string>() { name });
            }

            return this;
        }
    }
}
