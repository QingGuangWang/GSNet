using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace GSNet.Json.SystemTextJson.Modifiers
{
    /// <summary>
    /// 初始协定的修改器的定义
    /// </summary>
    public interface IJsonTypeInfoModifier
    {
        /// <summary>
        /// 修改JSON序列化的相关初始协定
        /// </summary>
        /// <param name="jsonTypeInfo"> JSON 序列化相关元数据</param>
        void ModifyJsonTypeInfo(JsonTypeInfo jsonTypeInfo);
    }
}
