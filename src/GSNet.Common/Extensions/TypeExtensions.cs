using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Extensions
{
    /// <summary>
    /// 类型信息类 <see cref="Type"/> 的相关扩展方法
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 根据指定的类型（类型参数 <typeparamref name="TAttribute"/>）， 获取类型上的特性对象（Attribute）。如果没有找到，则返回Null。
        /// </summary>
        /// <typeparam name="TAttribute">特性类型</typeparam>
        /// <param name="type">类型信息对象 </param>
        /// <param name="inherit">是否包含继承了指定类型（类型参数 <typeparamref name="TAttribute"/>）的特性类型</param>
        /// <returns>返回特性对象，如果没有找到，则返回Null</returns>
        public static TAttribute GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(this Type type,
            bool inherit = true)
            where TAttribute : Attribute
        {
            var attr = type.GetTypeInfo().GetSingleAttributeOrNull<TAttribute>();

            if (attr != null)
            {
                return attr;
            }

            if (type.GetTypeInfo().BaseType == null)
            {
                return null;
            }

            return type.GetTypeInfo().BaseType.GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(inherit);
        }

        /// <summary>
        /// 获取类型的名称，同时加上程序集名称。
        /// 格式是  类型名称全名 + ", " + 程序集名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        /// <summary>
        /// 判断当前指定类型（参数 <paramref name="type"/>），是否是从 <typeparamref name="TTarget"></typeparamref> 直接或间接派生的，或者类型相同。 
        /// 方法实现基于 <see cref="Type.IsAssignableTo"/>.
        /// <para>
        /// 例如: <typeparamref name="TTarget"></typeparamref> 类型是 Object, 参数 <paramref name="type"/> 是String的类型， String是继承Object，该方法判断 String 是不是从 Object 直接派生或者间接派生，结果为True。
        /// </para>
        /// </summary>
        /// <typeparam name="TTarget">目标匹配类型</typeparam> (as reverse).
        public static bool IsAssignableTo<TTarget>(this Type type)
        {
            Check.Argument.IsNotNull(type, nameof(type));

            return type.IsAssignableTo(typeof(TTarget));
        }

        /// <summary>
        /// 判断 <typeparamref name="TTarget"></typeparamref> 类型，是否是从当前指定类型（参数 <paramref name="type"/>）直接或间接派生的，或者类型相同。 
        /// 方法实现基于 <see cref="Type.IsAssignableFrom"/>.
        /// <para>
        /// 例如: <typeparamref name="TTarget"></typeparamref> 类型是 Object, 参数 <paramref name="type"/> 是String的类型， String是继承Object，该方法判断 Object 是不是从 String 直接派生或者间接派生，结果为False。
        /// </para>
        /// </summary>
        /// <typeparam name="TTarget">目标匹配类型</typeparam> (as reverse).
        public static bool IsAssignableFrom<TTarget>(this Type type)
        {
            Check.Argument.IsNotNull(type, nameof(type));

            return type.IsAssignableFrom(typeof(TTarget));
        }

        /// <summary>
        /// 判断当前指定类型（参数 <paramref name="type"/>），是否是一个具体类型，非抽象，接口和通用类型
        /// </summary>
        /// <param name="type">类型</param>
        public static bool IsRealClass(this Type type)
        {
            return type.IsAbstract == false
                   && type.IsGenericTypeDefinition == false
                   && type.IsInterface == false;
        }

        /// <summary>
        /// 判断当前指定类型（参数 <paramref name="type"/>），是否是一个泛型，且匹配指定的泛型定义（参数 <paramref name="genericTypeDefinition"/>）
        /// 如参数 <paramref name="type"/> 为 IEventHandler&lt;Object&gt;， 参数 <paramref name="genericTypeDefinition"/> 为 IEventHandler&lt;&gt; 则返回true.
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="genericTypeDefinition">泛型定义</param>
        /// <returns></returns>
        public static bool IsGenericTypeAndMatchDefinition(this Type type, Type genericTypeDefinition)
        {
            if (!genericTypeDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentException(
                    $"The param named {nameof(genericTypeDefinition)} must be a GenericTypeDefinition.");
            }

            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericTypeDefinition;
        }

        /// <summary>
        /// 判断当前指定类型（参数 <paramref name="type"/>），是否实现了指定的泛型定义（参数 <paramref name="genericTypeDefinition"/>）
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="genericTypeDefinition">泛型定义</param>
        /// <returns></returns>
        public static bool ImplementsGenericTypeDefinition(this Type type, Type genericTypeDefinition)
        {
            if (!genericTypeDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentException(
                    $"The param named {nameof(genericTypeDefinition)} must be a GenericTypeDefinition.");
            }

            if (genericTypeDefinition.IsInterface)
            {
                //判断type是不是直接是泛型且其泛型定义是和指定的一样，如果不是判断其实现的接口，是否符合。
                return type.IsGenericTypeAndMatchDefinition(genericTypeDefinition) ||
                       type.GetTypeInfo().ImplementedInterfaces.Any(@interface =>
                           @interface.IsGenericTypeAndMatchDefinition(genericTypeDefinition));
            }
            else
            {
                //如果是class
                return type.IsGenericTypeAndMatchDefinition(genericTypeDefinition) ||
                       type.GetBaseClasses(false).Any(@class =>
                           @class.IsGenericTypeAndMatchDefinition(genericTypeDefinition));
            }
        }


        /// <summary>
        /// 判断当前指定类型（参数 <paramref name="type"/>），是否包含指定接口类型（参数 <paramref name="interfaceType"/>）
        /// </summary>
        /// <param name="type">当前类型</param>
        /// <param name="interfaceType">接口类型</param>
        /// <returns>如果当前类型实现了该接口，返回true,否则返回false。</returns>
        public static bool IsTypeSupportInterface(this Type type, Type interfaceType)
        {
            if (!interfaceType.IsInterface)
            {
                return false;
            }

            if (interfaceType.IsAssignableFrom(type))
            {
                return true;
            }

            if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取当前指定类型（参数 <paramref name="type"/>）下，符合指定的泛型定义（参数 <paramref name="genericTypeDefinition"/>）的接口类型。
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="genericTypeDefinition">泛型定义</param>
        /// <returns></returns>
        public static Type[] GetMatchingDefinitionInterfaces(this Type type, Type genericTypeDefinition)
        {
            if (!genericTypeDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentException(
                    $"The param named {nameof(genericTypeDefinition)} must be a GenericTypeDefinition.");
            }

            var interfaces = type.GetInterfaces()
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericTypeDefinition).ToArray();

            return interfaces;
        }

        /// <summary>
        /// 获取类型的所有基类/父类类型
        /// </summary>
        /// <param name="type">要获取基类/父类的类型</param>
        /// <param name="includeObject">是否包含顶级父类  <see cref="object"/> ，如果是，则包含在返回的数组中。</param>
        public static Type[] GetBaseClasses(this Type type, bool includeObject = true)
        {
            Check.Argument.IsNotNull(type, nameof(type));

            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject);
            return types.ToArray();
        }

        /// <summary>
        /// 获取类型的所有基类/父类类型
        /// </summary>
        /// <param name="type">要获取基类/父类的类型</param>
        /// <param name="stoppingType">停止进入更深层次基类的类型，此类型将包含在返回的数组中。</param>
        /// <param name="includeObject">是否包含顶级父类  <see cref="object"/> ，如果是，则包含在返回的数组中。</param>
        public static Type[] GetBaseClasses(this Type type, Type stoppingType, bool includeObject = true)
        {
            Check.Argument.IsNotNull(type, nameof(type));

            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
            return types.ToArray();
        }

        /// <summary>
        /// 递归添加基类
        /// </summary>
        private static void AddTypeAndBaseTypesRecursively(
            List<Type> types, Type type, bool includeObject, Type stoppingType = null)
        {
            if (type == null || type == stoppingType)
            {
                return;
            }

            if (!includeObject && type == typeof(object))
            {
                return;
            }

            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
            types.Add(type);
        }

        /// <summary>
        /// 获取所有实现了指定接口类型(参数<paramref name="interfaceType"/>)的所有类型。
        /// 如果指定的类型参数<paramref name="interfaceType"/>，不是一个接口，则返回空数组。
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="assemblies">扫描的程序集，如果不指定，则扫描整个程序域</param>
        /// <returns>实现指定接口类型的所有类型的数组集合</returns>
        public static Type[] GetAllTypesImplementingInterface(this Type interfaceType, params Assembly[] assemblies)
        {
            Check.Argument.IsNotNull(interfaceType, nameof(interfaceType));

            if (!interfaceType.IsInterface)
            {
                return new Type[0];
            }

            var assembliesToScan = (assemblies != null && assemblies.Any())
                ? assemblies
                : AppDomain.CurrentDomain.GetAssemblies().ToArray();

            var types = assembliesToScan.SelectMany(a => a.GetTypes())
                //.Where(type => type.GetInterfaces().Contains(interfaceType))) //用这个方法判断，会少了很多
                .Where(type => IsTypeSupportInterface(type, interfaceType))
                .ToArray();

            return types;
        }

        /// <summary>
        /// 获取指定的程序集中（参数 <paramref name="assemblyInfo"/>），所有实现了指定接口类型(参数<paramref name="interfaceType"/>)的所有类型。
        /// 如果指定的类型参数<paramref name="interfaceType"/>，不是一个接口，则返回空数组。
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="assemblyInfo">程序集</param>
        /// <returns>实现指定接口类型的所有类型的数组集合</returns>
        public static Type[] GetAllTypesImplementingInterface(this Type interfaceType, Assembly assemblyInfo)
        {
            Check.Argument.IsNotNull(interfaceType, nameof(interfaceType));

            if (interfaceType.IsInterface)
            {
                return new Type[0];
            }

            var types = assemblyInfo.GetTypes()
                //.Where(type => type.GetInterfaces().Contains(interfaceType)).ToArray(); //用这个方法判断，会少了很多
                .Where(type => IsTypeSupportInterface(type, interfaceType)).ToArray();

            return types;
        }

        /// <summary>
        /// 判断当前指定类型（参数 <paramref name="type"/>），是否存在无参数构造方法
        /// </summary>
        /// <param name="type">当前类型</param>
        /// <param name="isOnlyPublic">是否只判断公共的无参构造方法</param>
        /// <returns>如果当前类型存在无参数构造方法，返回true,否则返回false。</returns>
        public static bool IsExistParameterlessConstructor(this Type type, bool isOnlyPublic = true)
        {
            if (type.IsInterface)
            {
                return false;
            }

            var bindingAttr = isOnlyPublic
                ? BindingFlags.Instance | BindingFlags.Public
                : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var constructorInfo = type.GetConstructor(bindingAttr, Type.EmptyTypes);

            return constructorInfo != null;
        }
    }
}
