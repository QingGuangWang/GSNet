using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace GSNet.Json.SystemTextJson.Modifiers
{
    /// <summary>
    /// 用于配置 私有set的属性也可以反序列化
    /// </summary>
    public class PropertiesWithPrivateSetModifier : IJsonTypeInfoModifier
    {
        public void ModifyJsonTypeInfo(JsonTypeInfo jsonTypeInfo)
        {
            foreach (var prop in jsonTypeInfo.Properties)
            {
                //如果Set是空的，则获取其私有Set
                if (prop.Set is null
                    && prop.AttributeProvider is PropertyInfo propertyInfo
                    && propertyInfo.GetSetMethod(true) is { } setMethod)
                {
                    prop.Set = (target, value) => setMethod.Invoke(target, new[] { value });
                }
            }
        }
    }
}
