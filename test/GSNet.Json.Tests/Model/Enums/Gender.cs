using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NewtonsoftJson
namespace GSNet.Json.NewtonsoftJson.Tests.Model.Enums
#else
namespace GSNet.Json.Tests.Model.Enums
#endif
{
    /// <summary>
    /// 枚举 - 性别
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// 男
        /// </summary>
        Male,

        /// <summary>
        /// 女
        /// </summary>
        Female
    }
}
