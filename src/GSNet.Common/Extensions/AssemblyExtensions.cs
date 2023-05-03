using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Extensions
{
    /// <summary>
    /// 程序集信息类 <see cref="Assembly"/> 的相关扩展方法
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// 获取程序集版本信息
        /// </summary>
        /// <param name="assembly">程序集对象</param>
        /// <returns>程序集版本信息</returns>
        public static string GetFileVersion(this Assembly assembly)
        {
            return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
        }

        /// <summary>
        /// 获取程序集已加载的类型
        /// </summary>
        /// <param name="assembly">程序集对象</param>
        /// <returns>已加载的类型</returns>
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null).ToArray();
            }
        }
    }
}
