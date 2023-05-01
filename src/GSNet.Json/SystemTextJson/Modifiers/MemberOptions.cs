using System.Reflection;

#if NewtonsoftJson
namespace GSNet.Json.NewtonsoftJson.ContractResolvers
#else
namespace GSNet.Json.SystemTextJson.Modifiers
#endif
{
    /// <summary>
    /// 成员的选项信息
    /// </summary>
    internal class MemberOptions
    {
        internal MemberOptions(MemberInfo memberInfo, bool isEffectiveForSubclasses = true, bool isEffectiveForHideInheritedMember = false)
        {
            MemberInfo = memberInfo;
            Type = memberInfo.ReflectedType;
            IsEffectiveForSubclasses = isEffectiveForSubclasses;
            IsEffectiveForHideInheritedMember = isEffectiveForHideInheritedMember;
        }

        internal MemberOptions(MemberInfo memberInfo, Type type, bool isEffectiveForSubclasses = true, bool isEffectiveForHideInheritedMember = false)
        {
            MemberInfo = memberInfo;
            Type = type;
            IsEffectiveForSubclasses = isEffectiveForSubclasses;
            IsEffectiveForHideInheritedMember = isEffectiveForHideInheritedMember;
        }

        /// <summary>
        /// 需要进行操作的成员（字段或者属性）
        /// </summary>
        internal MemberInfo MemberInfo { get; set; }

        /// <summary>
        /// 指定成员的类型，一般与属性<see cref="MemberInfo"/>的<see cref="MemberInfo.ReflectedType"/>的值是一样
        /// </summary>
        internal Type Type { get; set; }

        /// <summary>
        /// 是否作用于子类
        /// </summary>
        internal bool IsEffectiveForSubclasses { get; set; }

        /// <summary>
        /// 当属性值<see cref="IsEffectiveForSubclasses"/>为True，这个属性值才有效果。
        ///<para> 是否作用于隐藏继承成员（子类的属性使用new修饰符）</para>
        /// </summary>
        internal bool IsEffectiveForHideInheritedMember { get; set; }

        /// <summary>
        /// 判断是否匹配目标成员
        /// </summary>
        internal bool IsMatchTarget(MemberInfo targetMemberInfo, Type targetType)
        {
            //如果目标成员和本身的成员是一致的
            if (MemberInfo == targetMemberInfo)
            {
                return true;
            }

            //成员名称不一样，或者 类型不一样, 目标对象类型和配置的对象类型不是同一个也不是其子类
            if (targetMemberInfo.Name != MemberInfo.Name 
                || targetMemberInfo.MemberType != MemberInfo.MemberType 
                || !Type.IsAssignableFrom(targetType)) 
            {
                return false;
            }

            //是否不作用于 子类
            if (!this.IsEffectiveForSubclasses)
            {
                return false;
            }

            //判断目标成员是通过new修饰符，隐藏集成的
            if (targetMemberInfo.DeclaringType != MemberInfo.DeclaringType)
            {
                return IsEffectiveForHideInheritedMember;
            }

            return true;
        }
    }
}
