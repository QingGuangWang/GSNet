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
        private readonly IDictionary<Type, HashSet<string>> _typeIgnoreNameDict = new Dictionary<Type, HashSet<string>>();

        private readonly IList<MemberOptions> _memberIgnoreOptionsList = new List<MemberOptions>();

        public void ModifyJsonTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
                return;

            //查询出所有需要被忽略的属性
            var propertyInfosNeedIgnore = jsonTypeInfo.Properties.Where(x =>
            {
                //正常情况下AttributeProvider都是MemberInfo，除非用jsonTypeInfo.CreateJsonPropertyInfo等方式自己定义的
                if (x.AttributeProvider is MemberInfo attributeProvider)
                {
                    return _memberIgnoreOptionsList.Any(y => y.IsMatchTarget(attributeProvider, jsonTypeInfo.Type));
                }
                else
                {
                    //根据JSON字段名称去忽略
                    if (_typeIgnoreNameDict.TryGetValue(jsonTypeInfo.Type, out var ignoreNames))
                    {
                        return ignoreNames.Contains(x.Name);

                    }
                }

                return false;
            }).ToList();

            //移除需要忽略的
            foreach (var propertyInfo in propertyInfosNeedIgnore)
            {
                jsonTypeInfo.Properties.Remove(propertyInfo);
            }

        }

        /// <summary>
        /// 通过表达式，配置在序列化/反序列化的类型（<typeparamref name="TDestination"/>）的时候，其需要忽略的公开的属性/字段
        /// </summary>
        /// <typeparam name="TDestination">序列化/反序列化的类型</typeparam>
        /// <param name="destinationMemberLambdaExpression">指向其属性成员的Lambda表达式</param>
        /// <param name="effectiveForSubclasses">是否对序列化/反序列化的类型（<typeparamref name="TDestination"/>）的子类都生效， 默认是true</param>
        /// <param name="effectiveForHideInheritedMember">当<paramref name="effectiveForSubclasses"/>为true的时候，该参数才有作用。是否作用于隐藏继承成员（子类的属性使用new修饰符）， 默认是false</param>
        public IgnorePropertyOrFieldModifier AddIgnorePropertyOrField<TDestination>(Expression<Func<TDestination, object>> destinationMemberLambdaExpression, bool effectiveForSubclasses = true, bool effectiveForHideInheritedMember = false)
        {
            var memberInfo = ExpressionHelper.GetMemberInfo(destinationMemberLambdaExpression);

            _memberIgnoreOptionsList.Add(new MemberOptions(memberInfo, typeof(TDestination), effectiveForSubclasses, effectiveForHideInheritedMember));

            return this;
        }

        /// <summary>
        /// 配置在序列化/反序列化的类型（<paramref name="type"/>）的时候，其需要忽略的公开的属性/字段
        /// </summary>
        /// <param name="type">序列化/反序列化的类型</param>
        /// <param name="memberName">属性/字段名称</param>
        /// <param name="effectiveForSubclasses">是否对序列化/反序列化的类型（<paramref name="type"/>）的子类都生效， 默认是true</param>
        /// <param name="effectiveForHideInheritedMember">当<paramref name="effectiveForSubclasses"/>为true的时候，该参数才有作用。是否作用于隐藏继承成员（子类的属性使用new修饰符）， 默认是false</param>
        /// <returns></returns>
        public IgnorePropertyOrFieldModifier AddIgnorePropertyOrField(Type type, string memberName, bool effectiveForSubclasses = true, bool effectiveForHideInheritedMember = false)
        {
            var memberInfo = type.GetPublicPropertyOrField(memberName);

            if (memberInfo == null)
            {
                throw new ArgumentException($@"Cannot find property or field named [{memberName}] on [{type}]", nameof(memberName));
            }

            _memberIgnoreOptionsList.Add(new MemberOptions(memberInfo, type, effectiveForSubclasses, effectiveForHideInheritedMember));

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
                _typeIgnoreNameDict.Add(type, new HashSet<string>() { name });
            }

            return this;
        }
    }
}
