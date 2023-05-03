using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Constant
{
    /// <summary>
    /// 常量信息
    /// </summary>
    public class ConstantInfo
    {
        /// <summary>
        /// </summary>
        public ConstantInfo(string value, string description)
        {
            Value = value;
            Description = description;
        }

        /// <summary>
        /// 常量值
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }
    }
}
