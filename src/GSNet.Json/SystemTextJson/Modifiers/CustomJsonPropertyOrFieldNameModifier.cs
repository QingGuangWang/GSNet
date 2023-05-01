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
        private readonly IList<Tuple<MemberOptions, string>> _customPropertyNameList = new List<Tuple<MemberOptions, string>>();
        
        public void ModifyJsonTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
                return;

            foreach (var propertyInfo in jsonTypeInfo.Properties)
            {
                //正常情况下AttributeProvider都是MemberInfo，除非用jsonTypeInfo.CreateJsonPropertyInfo等方式自己定义的
                if (propertyInfo.AttributeProvider is MemberInfo attributeProvider)
                {
                    var customNameTuple = _customPropertyNameList.FirstOrDefault(x => x.Item1.IsMatchTarget(attributeProvider, jsonTypeInfo.Type));

                    if (customNameTuple != null)
                    {
                        propertyInfo.Name = customNameTuple.Item2;
                    }
                }
            }
        }

        /// <summary>
        /// 配置自定义的公开的属性/字段名称
        /// </summary>
        /// <typeparam name="TDestination">序列化/反序列化的对象</typeparam>
        /// <param name="destinationMemberLambdaExpression">指向其成员的Lambda表达式</param>
        /// <param name="name">新名称</param>
        /// <param name="effectiveForSubclasses">是否对序列化/反序列化的类型（<typeparamref name="TDestination"/>）的子类都生效， 默认是true</param>
        /// <param name="effectiveForHideInheritedMember">当<paramref name="effectiveForSubclasses"/>为true的时候，该参数才有作用。是否作用于隐藏继承成员（子类的属性使用new修饰符）， 默认是false</param>
        /// <returns></returns>
        public CustomJsonPropertyOrFieldNameModifier ConfigCustomPropertyOrFieldName<TDestination>(
            Expression<Func<TDestination, object>> destinationMemberLambdaExpression, string name, bool effectiveForSubclasses = true, bool effectiveForHideInheritedMember = false)
        {
            var memberInfo = ExpressionHelper.GetMemberInfo(destinationMemberLambdaExpression);

            _customPropertyNameList.Add(new Tuple<MemberOptions, string>(new MemberOptions(memberInfo, typeof(TDestination), effectiveForSubclasses, effectiveForHideInheritedMember), name));

            return this;
        }
    }
}
