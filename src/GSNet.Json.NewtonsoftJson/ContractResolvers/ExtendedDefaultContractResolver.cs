using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Principal;
using GSNet.Common.Helper;
using GSNet.Json.SystemTextJson.Modifiers;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace GSNet.Json.NewtonsoftJson.ContractResolvers
{
    /// <summary>
    /// 基于 Newtonsoft.Json 的 <see cref="DefaultContractResolver"/> 扩充一些自定义的配置。
    /// </summary>
    public class ExtendedDefaultContractResolver : DefaultContractResolver
    {
        private readonly IList<MemberOptions> _memberIgnoreOptionsList = new List<MemberOptions>();

        private readonly IList<Tuple<MemberOptions, string>> _customPropertyNameList = new List<Tuple<MemberOptions, string>>();

        /// <summary>
        /// 当DefaultCreator为空时，是否采用创建无初始化对象的方式代替。
        /// </summary>
        public bool IsUseUninitializedObjectWhenDefaultCreatorIsNull { get; set; }

        /// <summary>
        /// 是否支持属性的私有Set
        /// </summary>
        public bool IsSupportPrivateSetter { get; set; }

        public ExtendedDefaultContractResolver()
        {

        }

        public ExtendedDefaultContractResolver(bool isUseUninitializedObjectWhenDefaultCreatorIsNull, bool isSupportPrivateSetter)
        {
            IsUseUninitializedObjectWhenDefaultCreatorIsNull = isUseUninitializedObjectWhenDefaultCreatorIsNull;
            IsSupportPrivateSetter = isSupportPrivateSetter;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var jsonObjectContract = base.CreateObjectContract(objectType);

            //NewtonsoftJson 会优先找 无参数构造（含私有），如果没有，则看当前对象是否只有一个 公开带参数的 构造方法，是则取它，如果存在多个公共的，并且没有属性去标记的，则DefaultCreator 为空
            if (jsonObjectContract.DefaultCreator == null && IsUseUninitializedObjectWhenDefaultCreatorIsNull)
            {
                jsonObjectContract.DefaultCreator = () => FormatterServices.GetUninitializedObject(objectType);
            }

            return jsonObjectContract;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            var objType = member.ReflectedType;
            var memberIgnoreInfo = _memberIgnoreOptionsList.FirstOrDefault(x => x.IsMatchTarget(member, objType));

            if (memberIgnoreInfo != null)
            {
                property.Ignored = true;
            }

            var customPropertyName = _customPropertyNameList.FirstOrDefault(x => x.Item1.IsMatchTarget(member, objType));

            if (customPropertyName != null)
            {
                property.PropertyName = customPropertyName.Item2;
            }

            if (!property.Writable && IsSupportPrivateSetter)
            {
                property.Writable = (member as PropertyInfo)?.SetMethod != null;
            }

            return property;
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            return base.GetSerializableMembers(objectType);
        }

        /// <summary>
        /// 通过表达式，配置在序列化/反序列化的类型（<typeparamref name="TDestination"/>）的时候，其需要忽略的公开的属性/字段
        /// </summary>
        /// <typeparam name="TDestination">序列化/反序列化的类型</typeparam>
        /// <param name="destinationMemberLambdaExpression">指向其属性成员的Lambda表达式</param>
        /// <param name="effectiveForSubclasses">是否对序列化/反序列化的类型（<typeparamref name="TDestination"/>）的子类都生效， 默认是true</param>
        /// <param name="effectiveForHideInheritedMember">当<paramref name="effectiveForSubclasses"/>为true的时候，该参数才有作用。是否作用于隐藏继承成员（子类的属性使用new修饰符）， 默认是false</param>
        public ExtendedDefaultContractResolver AddIgnorePropertyOrField<TDestination>(Expression<Func<TDestination, object>> destinationMemberLambdaExpression, bool effectiveForSubclasses = true, bool effectiveForHideInheritedMember = false)
        {
            var memberInfo = ExpressionHelper.GetMemberInfo(destinationMemberLambdaExpression);

            _memberIgnoreOptionsList.Add(new MemberOptions(memberInfo, typeof(TDestination), effectiveForSubclasses, effectiveForHideInheritedMember));

            return this;
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
        public ExtendedDefaultContractResolver ConfigCustomPropertyOrFieldName<TDestination>(
            Expression<Func<TDestination, object>> destinationMemberLambdaExpression, string name, bool effectiveForSubclasses = true, bool effectiveForHideInheritedMember = false)
        {
            var memberInfo = ExpressionHelper.GetMemberInfo(destinationMemberLambdaExpression);

            _customPropertyNameList.Add(new Tuple<MemberOptions, string>(new MemberOptions(memberInfo, typeof(TDestination), effectiveForSubclasses, effectiveForHideInheritedMember), name));

            return this;
        }
    }
}
