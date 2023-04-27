using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Extensions
{
    /// <summary>
    /// 成员信息类 <see cref="MemberInfo"/> 的相关扩展方法
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// 根据指定的类型（类型参数 <typeparamref name="TAttribute"/>）， 获取方法上的特性对象（Attribute）。如果没有找到，则返回Null。
        /// </summary>
        /// <typeparam name="TAttribute">特性类型</typeparam>
        /// <param name="memberInfo">方法对象 </param>
        /// <param name="inherit">是否包含继承了指定类型（类型参数 <typeparamref name="TAttribute"/>）的特性类型</param>
        /// <returns>返回特性对象，如果没有找到，则返回Null</returns>
        public static TAttribute GetSingleAttributeOrNull<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            var attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToArray();
            if (attrs.Length > 0)
            {
                return (TAttribute)attrs[0];
            }

            return default;
        }
    }
}
