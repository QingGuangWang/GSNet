using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace GSNet.Json.SystemTextJson.Modifiers
{
    /// <summary>
    /// 针对具体类型配置特定的对象构造方式
    /// </summary>
    public class SpecificObjectConstructorModifier : IJsonTypeInfoModifier
    {
        private readonly IDictionary<Type, Func<object>> _createObjectFuncDict = new Dictionary<Type, Func<object>>();
 
        public void ModifyJsonTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            {
                return;
            }

            //判断是否需要配置
            if (_createObjectFuncDict.TryGetValue(jsonTypeInfo.Type, out var createObjectFunc))
            {
                jsonTypeInfo.CreateObject = createObjectFunc;
            }
        }

        /// <summary>
        ///  添加类型<typeparamref name="T"/>的构造方式。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="createObjectFunc">对象构造的委托</param>
        /// <returns></returns>
        public SpecificObjectConstructorModifier AddCreateObject<T>(Func<T> createObjectFunc) where T : class
        {
            if (!_createObjectFuncDict.ContainsKey(typeof(T)))
            {
                _createObjectFuncDict.Add(typeof(T), createObjectFunc);
            }
            else
            {
                _createObjectFuncDict[typeof(T)] = createObjectFunc;
            }

            return this;
        }

        /// <summary>
        ///  添加类型<paramref name="type"/>的构造方式。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="createObjectFunc">对象构造的委托</param>
        /// <returns></returns>
        public SpecificObjectConstructorModifier AddCreateObject(Type type, Func<object> createObjectFunc)
        {
            if (!_createObjectFuncDict.ContainsKey(type))
            {
                _createObjectFuncDict.Add(type, createObjectFunc);
            }
            else
            {
                _createObjectFuncDict[type] = createObjectFunc;
            }

            return this;
        }

        /// <summary>
        /// 添加类型<typeparamref name="T"/>的构造方式，基于无参的构造函数去构建。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allowNoPublic">是否运行私有构造函数，默认<see langword="true" /> </param>
        /// <returns></returns>
        public SpecificObjectConstructorModifier AddCreateObjectUseParameterlessConstructor<T>(bool allowNoPublic = true) where T : class
        {
            object Func() => Activator.CreateInstance(typeof(T), allowNoPublic);

            if (!_createObjectFuncDict.ContainsKey(typeof(T)))
            {
                _createObjectFuncDict.Add(typeof(T), Func);
            }
            else
            {
                _createObjectFuncDict[typeof(T)] = Func;
            }

            return this;
        }

        /// <summary>
        /// 添加类型<paramref name="type"/>的构造方式，基于无参的构造函数去构建。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="allowNoPublic">是否运行私有构造函数，默认<see langword="true" /> </param>
        /// <returns></returns>
        public SpecificObjectConstructorModifier AddCreateObjectUseParameterlessConstructor(Type type, bool allowNoPublic = true)
        {
            object Func() => Activator.CreateInstance(type, allowNoPublic);

            if (!_createObjectFuncDict.ContainsKey(type))
            {
                _createObjectFuncDict.Add(type, Func);
            }
            else
            {
                _createObjectFuncDict[type] = Func;
            }

            return this;
        }

        /// <summary>
        /// 添加类型<typeparamref name="T"/>的构造方式，创建无初始化的实例对象（不调用构造函数）。
        /// 通过使用 <see cref="FormatterServices.GetUninitializedObject"/> 的方式，适合有复杂构造函数而没有其余默认初始化的类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public SpecificObjectConstructorModifier AddCreateUninitializedObject<T>() where T : class
        {
            object Func() => FormatterServices.GetUninitializedObject(typeof(T));

            if (!_createObjectFuncDict.ContainsKey(typeof(T)))
            {
                _createObjectFuncDict.Add(typeof(T), Func);
            }
            else
            {
                _createObjectFuncDict[typeof(T)] = Func;
            }

            return this;
        }

        /// <summary>
        /// 添加类型<paramref name="type"/>的构造方式，通过创建无初始化的方式（不调用构造函数）。
        /// 通过使用 <see cref="FormatterServices.GetUninitializedObject"/> 的方式，适合有复杂构造函数而没有其余默认初始化的类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public SpecificObjectConstructorModifier AddCreateUninitializedObject(Type type)
        {
            object Func() => FormatterServices.GetUninitializedObject(type);

            if (!_createObjectFuncDict.ContainsKey(type))
            {
                _createObjectFuncDict.Add(type, Func);
            }
            else
            {
                _createObjectFuncDict[type] = Func;
            }

            return this;
        }
    }
}
