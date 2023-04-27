using GSNet.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace GSNet.Json.SystemTextJson.Modifiers
{
    /// <summary>
    /// 用于配置为使用无参数构造进行对象的构造（含公共public或非公共的private）
    /// 针对没有配置过 CreateObject 委托的, 且类型存在无参数构造方法，否则忽略该类型的配置。
    /// </summary>
    public class ParameterlessConstructorModifier : IJsonTypeInfoModifier
    {
        public void ModifyJsonTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            //针对没有配置过 CreateObject 委托的
            if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object 
                && jsonTypeInfo.CreateObject is null
                && jsonTypeInfo.Type.IsExistParameterlessConstructor(false))
            {
                //基于公共或非公共的无参数构造函数
                jsonTypeInfo.CreateObject = () => Activator.CreateInstance(jsonTypeInfo.Type, true);
            }
        }
    }
}
