using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Constant.Predefine
{
    /// <summary>
    /// 预定义好的一些常用的区域语言文化的常量，可用于生成<see cref="CultureInfo"/>
    /// <para>
    ///     Windows 系统 和 Linux 系统上，.Net Culture 对于区域语言文化的识别 存在差异。如Linux 系統上 zh-CN 和 zh-HK 是“別名”,
    ///     .Net Core 不能正確處理別名，要改用 zh-Hans-CN，zh-Hant-HK 
    /// </para>
    /// <para>
    ///     由于浏览器的不同，Accept-Language的值可能也不同，如Chrome浏览器，Firefox浏览器 ，目前还是用zh-CN,zh，而微软的Edge 是用zh-Hans-CN, zh-Hans;这种标准。
    ///     如果做国际化的情况下，通过Accept-Language或者url传值（lang="zh"，lang="zh-cn"）等， 可能需要程序做个处理，如 zh-cn 替换成 zh-hans-cn 或者 zh-hans 。
    ///     参考：https://blog.7in0.me/2019/10/13/aspnet-core-mvc-chinese-localization-problem/ 
    /// </para>
    /// </summary>
    public static class CultureCode
    {
        /// <summary>
        /// 表示英文
        /// </summary>
        [Description("English")]
        public const string English = "en";

        /// <summary>
        /// 英文-美国
        /// </summary>
        [Description("English（US）")]
        public const string EnglishUnitedStates = "en-US";

        /// <summary>
        /// 英文-英国
        /// </summary>
        [Description("English（GB）")]
        public const string EnglishUnitedKingdom = "en-GB";

        /// <summary>
        /// 中文
        /// </summary>
        [Description("中文")]
        public const string Chinese = "zh";

        /// <summary>
        /// 中文-简体中文
        /// </summary>
        [Description("简体中文")]
        public const string SimplifiedChinese = "zh-Hans";

        /// <summary>
        /// 中文-繁体中文
        /// </summary>
        [Description("繁體中文")]
        public static string TraditionalChinese = "zh-Hant";

        /// <summary>
        /// 中文-简体中文（中国）
        /// </summary>
        [Description("简体中文（中国）")]
        public const string SimplifiedChineseChina = "zh-Hans-CN";

        /// <summary>
        /// 中文-简体中文（中国香港特别行政区）
        /// </summary>
        [Description("简体中文（中国香港特别行政区）")]
        public static string SimplifiedChineseHongKong = "zh-Hans-HK";

        /// <summary>
        /// 中文-简体中文（中国澳门特别行政区）
        /// </summary>
        [Description("简体中文（中国澳门特别行政区）")]
        public static string SimplifiedChineseMacao = "zh-Hans-MO";

        /// <summary>
        /// 中文-简体中文（新加坡）
        /// </summary>
        [Description("简体中文（新加坡）")]
        public static string SimplifiedChineseSingapore = "zh-Hans-SG";

        /// <summary>
        /// 中文-繁体中文（中国香港特别行政区）
        /// </summary>
        [Description("繁體中文（中国香港特别行政区）")]
        public static string TraditionalChineseHongKong = "zh-Hant-HK";

        /// <summary>
        /// 中文-繁体中文（中国澳门特别行政区）
        /// </summary>
        [Description("繁體中文（中国澳门特别行政区）")]
        public static string TraditionalChineseMacao = "zh-Hant-MO";

        /// <summary>
        /// 日语
        /// </summary>
        [Description("日语")]
        public const string Japanese = "ja";

        /// <summary>
        /// 日语（日本） 
        /// </summary>
        [Description("日语（日本）")]
        public const string JapaneseJapan = "ja-JP";
    }
}
