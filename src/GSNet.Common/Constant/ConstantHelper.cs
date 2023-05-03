using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Constant
{
    /// <summary>
    /// 常量的辅助方法
    /// </summary>
    public static class ConstantHelper
    {
        private static readonly IDictionary<Type, IList<ConstantInfo>> ConstantDict = new Dictionary<Type, IList<ConstantInfo>>();

        private static readonly IDictionary<Type, IDictionary<string, string>> ConstantDescDict = new Dictionary<Type, IDictionary<string, string>>();

        private static readonly object LockObj = new object();

        /// <summary>
        /// 获取常量列表
        /// </summary>
        public static IList<ConstantInfo> GetConstantInfoList(Type constantType)
        {
            if (ConstantDict.TryGetValue(constantType, out var constantInfoList))
            {
                return constantInfoList;
            }
            else
            {
                lock (LockObj)
                {
                    if (!ConstantDict.ContainsKey(constantType))
                    {
                        var result = new List<ConstantInfo>();
                        //获取所有常量字段
                        var fields = constantType.GetFields(BindingFlags.Static | BindingFlags.Public).ToList();

                        var descDict = new Dictionary<string, string>();

                        foreach (var field in fields)
                        {
                            //常量值
                            var value = field.GetValue(null)?.ToString();
                            //取常量字段的Description属性
                            var constantDescriptionAttr = field.GetCustomAttribute<DescriptionAttribute>(true);

                            result.Add(new ConstantInfo(value, constantDescriptionAttr?.Description ?? string.Empty));
                            descDict.Add(value, constantDescriptionAttr?.Description ?? string.Empty);
                        }

                        ConstantDict.Add(constantType, result);
                        ConstantDescDict.Add(constantType, descDict);
                    }
                }

                return ConstantDict[constantType];
            }
        }

        /// <summary>
        /// 获取常量描述
        /// </summary>
        public static string GetDescription(Type constantType, string value)
        {
            if (!ConstantDescDict.ContainsKey(constantType))
            {
                GetConstantInfoList(constantType);
            }

            return ConstantDescDict[constantType][value];
        }
    }
}
