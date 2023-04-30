#if NewtonsoftJson
using GSNet.Json.NewtonsoftJson.Tests.Model.Enums;
#else
using  GSNet.Json.Tests.Model.Enums;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

#if NewtonsoftJson
namespace GSNet.Json.NewtonsoftJson.Tests.Model
#else
namespace GSNet.Json.Tests.Model
#endif
{
    /// <summary>
    /// 表示人
    /// </summary>
    public class Person
    {
        /// <summary>
        /// 中文名
        /// </summary>
        public string ZhName { get; set; }

        /// <summary>
        /// 英文
        /// </summary>
        public string EnName { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
