using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Extensions
{
    /// <summary>
    /// 构造方法信息类 <see cref="ConstructorInfo"/> 的相关扩展方法
    /// </summary>
    public static class ConstructorInfoExtensions
    {
        /// <summary>
        /// 根据指定的特性类型（类型参数 <typeparamref name="TAttribute"/>）， 获取构造方法上的特性对象（Attribute）。如果没有找到，则返回Null。
        /// </summary>
        /// <typeparam name="TAttribute">特性类型</typeparam>
        /// <param name="constructorInfo">方法对象 </param>
        /// <param name="inherit">是否包含继承了指定类型（类型参数 <typeparamref name="TAttribute"/>）的特性类型</param>
        /// <returns>返回特性对象，如果没有找到，则返回Null</returns>
        public static TAttribute GetSingleAttributeOrNull<TAttribute>(this ConstructorInfo constructorInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            var attrs = constructorInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToArray();
            if (attrs.Length > 0)
            {
                return (TAttribute)attrs[0];
            }

            return default;
        }
    }
}
