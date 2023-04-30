using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NewtonsoftJson
namespace GSNet.Json.NewtonsoftJson.Tests.Model
#else
namespace GSNet.Json.Tests.Model
#endif
{
    /// <summary>
    /// 项目
    /// </summary>
    public class Project
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目经费
        /// </summary>
        public decimal Funds { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// 项目耗时
        /// </summary>
        public TimeSpan TimeConsuming { get; set; }
    }
}
