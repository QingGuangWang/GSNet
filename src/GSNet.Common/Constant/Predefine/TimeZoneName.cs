using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Constant.Predefine
{
    /// <summary>
    /// 预定义的时区常量集, 基于windows系统 的时区的名称叫法。
    ///
    /// <para>
    /// 可以通过<see cref="TimeZoneInfo.FindSystemTimeZoneById"/>方法，传入常量参数(如 "Central Standard Time")获取对应的时区信息。
    /// 但是如果程序在Linux上运行，则会出现问题，因为Linux系统通过 Internet Assigned Numbers Authority (IANA)维护的时区数据库， 命名和叫法 和 Windows 上不同。
    /// 可以使用Nuget安装第三方库 TimeZoneConverter ，调用 <see cref="TZConvert.GetTimeZoneInfo"/>方法,传入常量参数(如 "Central Standard Time") ，则在Linux环境或者Windows运行环境都没问题。
    /// </para>
    /// </summary>
    public static class TimeZoneName
    {
        /// <summary>
        /// (UTC-12:00) 国际日期变更线西
        /// </summary>
        [Description("(UTC-12:00) 国际日期变更线西")] 
        public const string DatelineStandardTime = "Dateline Standard Time";

        /// <summary>
        /// (UTC-11:00) 协调世界时-11
        /// </summary>
        [Description("(UTC-11:00) 协调世界时-11")]
        public const string Utc11 = "UTC-11";

        /// <summary>
        /// (UTC-10:00) 夏威夷
        /// </summary>
        [Description("(UTC-10:00) 夏威夷")]
        public const string HawaiianStandardTime = "Hawaiian Standard Time";

        /// <summary>
        /// (UTC-10:00) 阿留申群岛
        /// </summary>
        [Description("(UTC-10:00) 阿留申群岛")]
        public const string AleutianStandardTime = "Aleutian Standard Time";

        /// <summary>
        /// (UTC-09:30) 马克萨斯群岛
        /// </summary>
        [Description("(UTC-09:30) 马克萨斯群岛")]
        public const string MarquesasStandardTime = "Marquesas Standard Time";

        /// <summary>
        /// (UTC-09:00) 协调世界时-09
        /// </summary>
        [Description("(UTC-09:00) 协调世界时-09")] 
        public const string Utc09 = "UTC-09";

        /// <summary>
        /// (UTC-09:00) 阿拉斯加
        /// </summary>
        [Description("(UTC-09:00) 阿拉斯加")]
        public const string AlaskanStandardTime = "Alaskan Standard Time";

        /// <summary>
        /// (UTC-08:00) 下加利福尼亚州
        /// </summary>
        [Description("(UTC-08:00) 下加利福尼亚州")]
        public const string PacificStandardTimeMexico = "Pacific Standard Time (Mexico)";

        /// <summary>
        /// (UTC-08:00) 协调世界时-08
        /// </summary>
        [Description("(UTC-08:00) 协调世界时-08")]
        public const string Utc08 = "UTC-08";

        /// <summary>
        /// (UTC-08:00) 太平洋时间(美国和加拿大)
        /// </summary>
        [Description("(UTC-08:00) 太平洋时间(美国和加拿大)")]
        public const string PacificStandardTime = "Pacific Standard Time";

        /// <summary>
        /// (UTC-07:00) 亚利桑那
        /// </summary>
        [Description("(UTC-07:00) 亚利桑那")]
        public const string UsMountainStandardTime = "US Mountain Standard Time";

        /// <summary>
        /// (UTC-07:00) 奇瓦瓦，拉巴斯，马萨特兰
        /// </summary>
        [Description("(UTC-07:00) 奇瓦瓦，拉巴斯，马萨特兰")]
        public const string MountainStandardTimeMexico = "Mountain Standard Time (Mexico)";

        /// <summary>
        /// (UTC-07:00) 山地时间(美国和加拿大)
        /// </summary>
        [Description("(UTC-07:00) 山地时间(美国和加拿大)")]
        public const string MountainStandardTime = "Mountain Standard Time";

        /// <summary>
        /// (UTC-06:00) 中美洲
        /// </summary>
        [Description("(UTC-06:00) 中美洲")]
        public const string CentralAmericaStandardTime = "Central America Standard Time";

        /// <summary>
        /// (UTC-06:00) 中部时间(美国和加拿大)
        /// </summary>
        [Description("(UTC-06:00) 中部时间(美国和加拿大)")]
        public const string CentralStandardTime = "Central Standard Time";

        /// <summary>
        /// (UTC-06:00) 复活节岛
        /// </summary>
        [Description("(UTC-06:00) 复活节岛")]
        public const string EasterIslandStandardTime = "Easter Island Standard Time";

        /// <summary>
        /// (UTC-06:00) 瓜达拉哈拉，墨西哥城，蒙特雷
        /// </summary>
        [Description("(UTC-06:00) 瓜达拉哈拉，墨西哥城，蒙特雷")]
        public const string CentralStandardTimeMexico = "Central Standard Time (Mexico)";

        /// <summary>
        /// (UTC-06:00) 萨斯喀彻温
        /// </summary>
        [Description("(UTC-06:00) 萨斯喀彻温")]
        public const string CanadaCentralStandardTime = "Canada Central Standard Time";

        /// <summary>
        /// (UTC-05:00) 东部时间(美国和加拿大)
        /// </summary>
        [Description("(UTC-05:00) 东部时间(美国和加拿大)")]
        public const string EasternStandardTime = "Eastern Standard Time";

        /// <summary>
        /// (UTC-05:00) 切图马尔
        /// </summary>
        [Description("(UTC-05:00) 切图马尔")]
        public const string EasternStandardTimeMexico = "Eastern Standard Time (Mexico)";

        /// <summary>
        /// (UTC-05:00) 印地安那州(东部)
        /// </summary>
        [Description("(UTC-05:00) 印地安那州(东部)")]
        public const string UsEasternStandardTime = "US Eastern Standard Time";

        /// <summary>
        /// (UTC-05:00) 哈瓦那
        /// </summary>
        [Description("(UTC-05:00) 哈瓦那")]
        public const string CubaStandardTime = "Cuba Standard Time";

        /// <summary>
        /// (UTC-05:00) 波哥大，利马，基多，里奥布朗库
        /// </summary>
        [Description("(UTC-05:00) 波哥大，利马，基多，里奥布朗库")]
        public const string SaPacificStandardTime = "SA Pacific Standard Time";

        /// <summary>
        /// (UTC-05:00) 海地
        /// </summary>
        [Description("(UTC-05:00) 海地")]
        public const string HaitiStandardTime = "Haiti Standard Time";

        /// <summary>
        /// (UTC-05:00) 特克斯和凯科斯群岛
        /// </summary>
        [Description("(UTC-05:00) 特克斯和凯科斯群岛")]
        public const string TurksAndCaicosStandardTime = "Turks And Caicos Standard Time";

        /// <summary>
        /// (UTC-04:00) 乔治敦，拉巴斯，马瑙斯，圣胡安
        /// </summary>
        [Description("(UTC-04:00) 乔治敦，拉巴斯，马瑙斯，圣胡安")]
        public const string SaWesternStandardTime = "SA Western Standard Time";

        /// <summary>
        /// (UTC-04:00) 亚松森
        /// </summary>
        [Description("(UTC-04:00) 亚松森")]
        public const string ParaguayStandardTime = "Paraguay Standard Time";

        /// <summary>
        /// (UTC-04:00) 加拉加斯
        /// </summary>
        [Description("(UTC-04:00) 加拉加斯")]
        public const string VenezuelaStandardTime = "Venezuela Standard Time";

        /// <summary>
        /// (UTC-04:00) 圣地亚哥
        /// </summary>
        [Description("(UTC-04:00) 圣地亚哥")]
        public const string PacificSaStandardTime = "Pacific SA Standard Time";

        /// <summary>
        /// (UTC-04:00) 大西洋时间(加拿大)
        /// </summary>
        [Description("(UTC-04:00) 大西洋时间(加拿大)")]
        public const string AtlanticStandardTime = "Atlantic Standard Time";

        /// <summary>
        /// (UTC-04:00) 库亚巴
        /// </summary>
        [Description("(UTC-04:00) 库亚巴")]
        public const string CentralBrazilianStandardTime = "Central Brazilian Standard Time";

        /// <summary>
        /// (UTC-03:30) 纽芬兰
        /// </summary>
        [Description("(UTC-03:30) 纽芬兰")] 
        public const string NewfoundlandStandardTime = "Newfoundland Standard Time";

        /// <summary>
        /// (UTC-03:00) 卡宴，福塔雷萨
        /// </summary>
        [Description("(UTC-03:00) 卡宴，福塔雷萨")]
        public const string SaEasternStandardTime = "SA Eastern Standard Time";

        /// <summary>
        /// (UTC-03:00) 圣皮埃尔和密克隆群岛
        /// </summary>
        [Description("(UTC-03:00) 圣皮埃尔和密克隆群岛")]
        public const string SaintPierreStandardTime = "Saint Pierre Standard Time";

        /// <summary>
        /// (UTC-03:00) 巴西利亚
        /// </summary>
        [Description("(UTC-03:00) 巴西利亚")]
        public const string ESouthAmericaStandardTime = "E. South America Standard Time";

        /// <summary>
        /// (UTC-03:00) 布宜诺斯艾利斯
        /// </summary>
        [Description("(UTC-03:00) 布宜诺斯艾利斯")]
        public const string ArgentinaStandardTime = "Argentina Standard Time";

        /// <summary>
        /// (UTC-03:00) 格陵兰
        /// </summary>
        [Description("(UTC-03:00) 格陵兰")]
        public const string GreenlandStandardTime = "Greenland Standard Time";

        /// <summary>
        /// (UTC-03:00) 萨尔瓦多
        /// </summary>
        [Description("(UTC-03:00) 萨尔瓦多")]
        public const string BahiaStandardTime = "Bahia Standard Time";

        /// <summary>
        /// (UTC-03:00) 蒙得维的亚
        /// </summary>
        [Description("(UTC-03:00) 蒙得维的亚")]
        public const string MontevideoStandardTime = "Montevideo Standard Time";

        /// <summary>
        /// (UTC-03:00) 蓬塔阿雷纳斯
        /// </summary>
        [Description("(UTC-03:00) 蓬塔阿雷纳斯")]
        public const string MagallanesStandardTime = "Magallanes Standard Time";

        /// <summary>
        /// (UTC-03:00) 阿拉瓜伊纳
        /// </summary>
        [Description("(UTC-03:00) 阿拉瓜伊纳")]
        public const string TocantinsStandardTime = "Tocantins Standard Time";

        /// <summary>
        /// (UTC-02:00) 中大西洋 - 旧用
        /// </summary>
        [Description("(UTC-02:00) 中大西洋 - 旧用")]
        public const string MidAtlanticStandardTime = "Mid-Atlantic Standard Time";

        /// <summary>
        /// (UTC-02:00) 协调世界时-02
        /// </summary>
        [Description("(UTC-02:00) 协调世界时-02")] 
        public const string Utc02 = "UTC-02";

        /// <summary>
        /// (UTC-01:00) 亚速尔群岛
        /// </summary>
        [Description("(UTC-01:00) 亚速尔群岛")] 
        public const string AzoresStandardTime = "Azores Standard Time";

        /// <summary>
        /// (UTC-01:00) 佛得角群岛
        /// </summary>
        [Description("(UTC-01:00) 佛得角群岛")] 
        public const string CapeVerdeStandardTime = "Cape Verde Standard Time";

        /// <summary>
        /// (UTC) 协调世界时
        /// </summary>
        [Description("(UTC) 协调世界时")]
        public const string Utc = "UTC";

        /// <summary>
        /// (UTC+00:00) 圣多美
        /// </summary>
        [Description("(UTC+00:00) 圣多美")]
        public const string SaoTomeStandardTime = "Sao Tome Standard Time";

        /// <summary>
        /// (UTC+00:00) 蒙罗维亚，雷克雅未克
        /// </summary>
        [Description("(UTC+00:00) 蒙罗维亚，雷克雅未克")]
        public const string GreenwichStandardTime = "Greenwich Standard Time";

        /// <summary>
        /// (UTC+00:00) 都柏林，爱丁堡，里斯本，伦敦
        /// </summary>
        [Description("(UTC+00:00) 都柏林，爱丁堡，里斯本，伦敦")]
        public const string GmtStandardTime = "GMT Standard Time";

        /// <summary>
        /// (UTC+01:00) 卡萨布兰卡
        /// </summary>
        [Description("(UTC+01:00) 卡萨布兰卡")] 
        public const string MoroccoStandardTime = "Morocco Standard Time";

        /// <summary>
        /// (UTC+01:00) 中非西部
        /// </summary>
        [Description("(UTC+01:00) 中非西部")]
        public const string WCentralAfricaStandardTime = "W. Central Africa Standard Time";

        /// <summary>
        /// (UTC+01:00) 布鲁塞尔，哥本哈根，马德里，巴黎
        /// </summary>
        [Description("(UTC+01:00) 布鲁塞尔，哥本哈根，马德里，巴黎")]
        public const string RomanceStandardTime = "Romance Standard Time";

        /// <summary>
        /// (UTC+01:00) 萨拉热窝，斯科普里，华沙，萨格勒布
        /// </summary>
        [Description("(UTC+01:00) 萨拉热窝，斯科普里，华沙，萨格勒布")]
        public const string CentralEuropeanStandardTime = "Central European Standard Time";

        /// <summary>
        /// (UTC+01:00) 贝尔格莱德，布拉迪斯拉发，布达佩斯，卢布尔雅那，布拉格
        /// </summary>
        [Description("(UTC+01:00) 贝尔格莱德，布拉迪斯拉发，布达佩斯，卢布尔雅那，布拉格")]
        public const string CentralEuropeStandardTime = "Central Europe Standard Time";

        /// <summary>
        /// (UTC+01:00) 阿姆斯特丹，柏林，伯尔尼，罗马，斯德哥尔摩，维也纳
        /// </summary>
        [Description("(UTC+01:00) 阿姆斯特丹，柏林，伯尔尼，罗马，斯德哥尔摩，维也纳")]
        public const string WEuropeStandardTime = "W. Europe Standard Time";

        /// <summary>
        /// (UTC+02:00) 加沙，希伯伦
        /// </summary>
        [Description("(UTC+02:00) 加沙，希伯伦")]
        public const string WestBankStandardTime = "West Bank Standard Time";

        /// <summary>
        /// (UTC+02:00) 加里宁格勒
        /// </summary>
        [Description("(UTC+02:00) 加里宁格勒")]
        public const string KaliningradStandardTime = "Kaliningrad Standard Time";

        /// <summary>
        /// (UTC+02:00) 哈拉雷，比勒陀利亚
        /// </summary>
        [Description("(UTC+02:00) 哈拉雷，比勒陀利亚")]
        public const string SouthAfricaStandardTime = "South Africa Standard Time";

        /// <summary>
        /// (UTC+02:00) 喀土穆
        /// </summary>
        [Description("(UTC+02:00) 喀土穆")]
        public const string SudanStandardTime = "Sudan Standard Time";

        /// <summary>
        /// (UTC+02:00) 基希讷乌
        /// </summary>
        [Description("(UTC+02:00) 基希讷乌")]
        public const string EEuropeStandardTime = "E. Europe Standard Time";

        /// <summary>
        /// (UTC+02:00) 大马士革
        /// </summary>
        [Description("(UTC+02:00) 大马士革")] 
        public const string SyriaStandardTime = "Syria Standard Time";

        /// <summary>
        /// (UTC+02:00) 安曼
        /// </summary>
        [Description("(UTC+02:00) 安曼")]
        public const string JordanStandardTime = "Jordan Standard Time";

        /// <summary>
        /// (UTC+02:00) 开罗
        /// </summary>
        [Description("(UTC+02:00) 开罗")]
        public const string EgyptStandardTime = "Egypt Standard Time";

        /// <summary>
        /// (UTC+02:00) 温得和克
        /// </summary>
        [Description("(UTC+02:00) 温得和克")]
        public const string NamibiaStandardTime = "Namibia Standard Time";

        /// <summary>
        /// (UTC+02:00) 的黎波里
        /// </summary>
        [Description("(UTC+02:00) 的黎波里")]
        public const string LibyaStandardTime = "Libya Standard Time";

        /// <summary>
        /// (UTC+02:00) 耶路撒冷
        /// </summary>
        [Description("(UTC+02:00) 耶路撒冷")] 
        public const string IsraelStandardTime = "Israel Standard Time";

        /// <summary>
        /// (UTC+02:00) 贝鲁特
        /// </summary>
        [Description("(UTC+02:00) 贝鲁特")]
        public const string MiddleEastStandardTime = "Middle East Standard Time";

        /// <summary>
        /// (UTC+02:00) 赫尔辛基，基辅，里加，索非亚，塔林，维尔纽斯
        /// </summary>
        [Description("(UTC+02:00) 赫尔辛基，基辅，里加，索非亚，塔林，维尔纽斯")]
        public const string FleStandardTime = "FLE Standard Time";

        /// <summary>
        /// (UTC+02:00) 雅典，布加勒斯特
        /// </summary>
        [Description("(UTC+02:00) 雅典，布加勒斯特")]
        public const string GtbStandardTime = "GTB Standard Time";

        /// <summary>
        /// (UTC+03:00) 伊斯坦布尔
        /// </summary>
        [Description("(UTC+03:00) 伊斯坦布尔")] 
        public const string TurkeyStandardTime = "Turkey Standard Time";

        /// <summary>
        /// (UTC+03:00) 内罗毕
        /// </summary>
        [Description("(UTC+03:00) 内罗毕")]
        public const string EAfricaStandardTime = "E. Africa Standard Time";

        /// <summary>
        /// (UTC+03:00) 巴格达
        /// </summary>
        [Description("(UTC+03:00) 巴格达")
        ] public const string ArabicStandardTime = "Arabic Standard Time";

        /// <summary>
        /// (UTC+03:00) 明斯克
        /// </summary>
        [Description("(UTC+03:00) 明斯克")]
        public const string BelarusStandardTime = "Belarus Standard Time";

        /// <summary>
        /// (UTC+03:00) 科威特，利雅得
        /// </summary>
        [Description("(UTC+03:00) 科威特，利雅得")] 
        public const string ArabStandardTime = "Arab Standard Time";

        /// <summary>
        /// (UTC+03:00) 莫斯科，圣彼得堡
        /// </summary>
        [Description("(UTC+03:00) 莫斯科，圣彼得堡")]
        public const string RussianStandardTime = "Russian Standard Time";

        /// <summary>
        /// (UTC+03:30) 德黑兰
        /// </summary>
        [Description("(UTC+03:30) 德黑兰")]
        public const string IranStandardTime = "Iran Standard Time";

        /// <summary>
        /// (UTC+04:00) 伊热夫斯克，萨马拉
        /// </summary>
        [Description("(UTC+04:00) 伊热夫斯克，萨马拉")] 
        public const string RussiaTimeZone3 = "Russia Time Zone 3";

        /// <summary>
        /// (UTC+04:00) 伏尔加格勒
        /// </summary>
        [Description("(UTC+04:00) 伏尔加格勒")]
        public const string VolgogradStandardTime = "Volgograd Standard Time";

        /// <summary>
        /// (UTC+04:00) 埃里温
        /// </summary>
        [Description("(UTC+04:00) 埃里温")] 
        public const string CaucasusStandardTime = "Caucasus Standard Time";

        /// <summary>
        /// (UTC+04:00) 巴库
        /// </summary>
        [Description("(UTC+04:00) 巴库")]
        public const string AzerbaijanStandardTime = "Azerbaijan Standard Time";

        /// <summary>
        /// (UTC+04:00) 第比利斯
        /// </summary>
        [Description("(UTC+04:00) 第比利斯")]
        public const string GeorgianStandardTime = "Georgian Standard Time";

        /// <summary>
        /// (UTC+04:00) 萨拉托夫
        /// </summary>
        [Description("(UTC+04:00) 萨拉托夫")]
        public const string SaratovStandardTime = "Saratov Standard Time";

        /// <summary>
        /// (UTC+04:00) 路易港
        /// </summary>
        [Description("(UTC+04:00) 路易港")] 
        public const string MauritiusStandardTime = "Mauritius Standard Time";

        /// <summary>
        /// (UTC+04:00) 阿布扎比，马斯喀特
        /// </summary>
        [Description("(UTC+04:00) 阿布扎比，马斯喀特")]
        public const string ArabianStandardTime = "Arabian Standard Time";

        /// <summary>
        /// (UTC+04:00) 阿斯特拉罕，乌里扬诺夫斯克
        /// </summary>
        [Description("(UTC+04:00) 阿斯特拉罕，乌里扬诺夫斯克")]
        public const string AstrakhanStandardTime = "Astrakhan Standard Time";

        /// <summary>
        /// (UTC+04:30) 喀布尔
        /// </summary>
        [Description("(UTC+04:30) 喀布尔")]
        public const string AfghanistanStandardTime = "Afghanistan Standard Time";

        /// <summary>
        /// (UTC+05:00) 伊斯兰堡，卡拉奇
        /// </summary>
        [Description("(UTC+05:00) 伊斯兰堡，卡拉奇")]
        public const string PakistanStandardTime = "Pakistan Standard Time";

        /// <summary>
        /// (UTC+05:00) 克孜洛尔达
        /// </summary>
        [Description("(UTC+05:00) 克孜洛尔达")] 
        public const string QyzylordaStandardTime = "Qyzylorda Standard Time";

        /// <summary>
        /// (UTC+05:00) 叶卡捷琳堡
        /// </summary>
        [Description("(UTC+05:00) 叶卡捷琳堡")]
        public const string EkaterinburgStandardTime = "Ekaterinburg Standard Time";

        /// <summary>
        /// (UTC+05:00) 阿什哈巴德，塔什干
        /// </summary>
        [Description("(UTC+05:00) 阿什哈巴德，塔什干")]
        public const string WestAsiaStandardTime = "West Asia Standard Time";

        /// <summary>
        /// (UTC+05:30) 斯里加亚渥登普拉
        /// </summary>
        [Description("(UTC+05:30) 斯里加亚渥登普拉")] 
        public const string SriLankaStandardTime = "Sri Lanka Standard Time";

        /// <summary>
        /// (UTC+05:30) 钦奈，加尔各答，孟买，新德里
        /// </summary>
        [Description("(UTC+05:30) 钦奈，加尔各答，孟买，新德里")]
        public const string IndiaStandardTime = "India Standard Time";

        /// <summary>
        /// (UTC+05:45) 加德满都
        /// </summary>
        [Description("(UTC+05:45) 加德满都")]
        public const string NepalStandardTime = "Nepal Standard Time";

        /// <summary>
        /// (UTC+06:00) 达卡
        /// </summary>
        [Description("(UTC+06:00) 达卡")] 
        public const string BangladeshStandardTime = "Bangladesh Standard Time";

        /// <summary>
        /// (UTC+06:00) 鄂木斯克
        /// </summary>
        [Description("(UTC+06:00) 鄂木斯克")] 
        public const string OmskStandardTime = "Omsk Standard Time";

        /// <summary>
        /// (UTC+06:00) 阿斯塔纳
        /// </summary>
        [Description("(UTC+06:00) 阿斯塔纳")] 
        public const string CentralAsiaStandardTime = "Central Asia Standard Time";

        /// <summary>
        /// (UTC+06:30) 仰光
        /// </summary>
        [Description("(UTC+06:30) 仰光")]
        public const string MyanmarStandardTime = "Myanmar Standard Time";

        /// <summary>
        /// (UTC+07:00) 克拉斯诺亚尔斯克
        /// </summary>
        [Description("(UTC+07:00) 克拉斯诺亚尔斯克")]
        public const string NorthAsiaStandardTime = "North Asia Standard Time";

        /// <summary>
        /// (UTC+07:00) 巴尔瑙尔，戈尔诺-阿尔泰斯克
        /// </summary>
        [Description("(UTC+07:00) 巴尔瑙尔，戈尔诺-阿尔泰斯克")]
        public const string AltaiStandardTime = "Altai Standard Time";

        /// <summary>
        /// (UTC+07:00) 托木斯克
        /// </summary>
        [Description("(UTC+07:00) 托木斯克")] 
        public const string TomskStandardTime = "Tomsk Standard Time";

        /// <summary>
        /// (UTC+07:00) 新西伯利亚
        /// </summary>
        [Description("(UTC+07:00) 新西伯利亚")]
        public const string NCentralAsiaStandardTime = "N. Central Asia Standard Time";

        /// <summary>
        /// (UTC+07:00) 曼谷，河内，雅加达
        /// </summary>
        [Description("(UTC+07:00) 曼谷，河内，雅加达")] 
        public const string SeAsiaStandardTime = "SE Asia Standard Time";

        /// <summary>
        /// (UTC+07:00) 科布多
        /// </summary>
        [Description("(UTC+07:00) 科布多")]
        public const string WMongoliaStandardTime = "W. Mongolia Standard Time";

        /// <summary>
        /// (UTC+08:00) 乌兰巴托
        /// </summary>
        [Description("(UTC+08:00) 乌兰巴托")] 
        public const string UlaanbaatarStandardTime = "Ulaanbaatar Standard Time";

        /// <summary>
        /// (UTC+08:00) 伊尔库茨克
        /// </summary>
        [Description("(UTC+08:00) 伊尔库茨克")]
        public const string NorthAsiaEastStandardTime = "North Asia East Standard Time";

        /// <summary>
        /// (UTC+08:00) 北京，重庆，香港特别行政区，乌鲁木齐
        /// </summary>
        [Description("(UTC+08:00) 北京，重庆，香港特别行政区，乌鲁木齐")]
        public const string ChinaStandardTime = "China Standard Time";

        /// <summary>
        /// (UTC+08:00) 台北
        /// </summary>
        [Description("(UTC+08:00) 台北")] 
        public const string TaipeiStandardTime = "Taipei Standard Time";

        /// <summary>
        /// (UTC+08:00) 吉隆坡，新加坡
        /// </summary>
        [Description("(UTC+08:00) 吉隆坡，新加坡")]
        public const string SingaporeStandardTime = "Singapore Standard Time";

        /// <summary>
        /// (UTC+08:00) 珀斯
        /// </summary>
        [Description("(UTC+08:00) 珀斯")]
        public const string WAustraliaStandardTime = "W. Australia Standard Time";

        /// <summary>
        /// (UTC+08:45) 尤克拉
        /// </summary>
        [Description("(UTC+08:45) 尤克拉")]
        public const string AusCentralWStandardTime = "Aus Central W. Standard Time";

        /// <summary>
        /// (UTC+09:00) 大阪，札幌，东京
        /// </summary>
        [Description("(UTC+09:00) 大阪，札幌，东京")] 
        public const string TokyoStandardTime = "Tokyo Standard Time";

        /// <summary>
        /// (UTC+09:00) 平壤
        /// </summary>
        [Description("(UTC+09:00) 平壤")]
        public const string NorthKoreaStandardTime = "North Korea Standard Time";

        /// <summary>
        /// (UTC+09:00) 赤塔市
        /// </summary>
        [Description("(UTC+09:00) 赤塔市")] 
        public const string TransbaikalStandardTime = "Transbaikal Standard Time";

        /// <summary>
        /// (UTC+09:00) 雅库茨克
        /// </summary>
        [Description("(UTC+09:00) 雅库茨克")]
        public const string YakutskStandardTime = "Yakutsk Standard Time";

        /// <summary>
        /// (UTC+09:00) 首尔
        /// </summary>
        [Description("(UTC+09:00) 首尔")]
        public const string KoreaStandardTime = "Korea Standard Time";

        /// <summary>
        /// (UTC+09:30) 达尔文
        /// </summary>
        [Description("(UTC+09:30) 达尔文")] 
        public const string AusCentralStandardTime = "AUS Central Standard Time";

        /// <summary>
        /// (UTC+09:30) 阿德莱德
        /// </summary>
        [Description("(UTC+09:30) 阿德莱德")]
        public const string CenAustraliaStandardTime = "Cen. Australia Standard Time";

        /// <summary>
        /// (UTC+10:00) 关岛，莫尔兹比港
        /// </summary>
        [Description("(UTC+10:00) 关岛，莫尔兹比港")]
        public const string WestPacificStandardTime = "West Pacific Standard Time";

        /// <summary>
        /// (UTC+10:00) 堪培拉，墨尔本，悉尼
        /// </summary>
        [Description("(UTC+10:00) 堪培拉，墨尔本，悉尼")]
        public const string AusEasternStandardTime = "AUS Eastern Standard Time";

        /// <summary>
        /// (UTC+10:00) 布里斯班
        /// </summary>
        [Description("(UTC+10:00) 布里斯班")] public const string EAustraliaStandardTime = "E. Australia Standard Time";

        /// <summary>
        /// (UTC+10:00) 符拉迪沃斯托克
        /// </summary>
        [Description("(UTC+10:00) 符拉迪沃斯托克")]
        public const string VladivostokStandardTime = "Vladivostok Standard Time";

        /// <summary>
        /// (UTC+10:00) 霍巴特
        /// </summary>
        [Description("(UTC+10:00) 霍巴特")] public const string TasmaniaStandardTime = "Tasmania Standard Time";

        /// <summary>
        /// (UTC+10:30) 豪勋爵岛
        /// </summary>
        [Description("(UTC+10:30) 豪勋爵岛")] public const string LordHoweStandardTime = "Lord Howe Standard Time";

        /// <summary>
        /// (UTC+11:00) 乔库尔达赫
        /// </summary>
        [Description("(UTC+11:00) 乔库尔达赫")] public const string RussiaTimeZone10 = "Russia Time Zone 10";

        /// <summary>
        /// (UTC+11:00) 布干维尔岛
        /// </summary>
        [Description("(UTC+11:00) 布干维尔岛")]
        public const string BougainvilleStandardTime = "Bougainville Standard Time";

        /// <summary>
        /// (UTC+11:00) 所罗门群岛，新喀里多尼亚
        /// </summary>
        [Description("(UTC+11:00) 所罗门群岛，新喀里多尼亚")]
        public const string CentralPacificStandardTime = "Central Pacific Standard Time";

        /// <summary>
        /// (UTC+11:00) 萨哈林
        /// </summary>
        [Description("(UTC+11:00) 萨哈林")] public const string SakhalinStandardTime = "Sakhalin Standard Time";

        /// <summary>
        /// (UTC+11:00) 诺福克岛
        /// </summary>
        [Description("(UTC+11:00) 诺福克岛")] public const string NorfolkStandardTime = "Norfolk Standard Time";

        /// <summary>
        /// (UTC+11:00) 马加丹
        /// </summary>
        [Description("(UTC+11:00) 马加丹")] public const string MagadanStandardTime = "Magadan Standard Time";

        /// <summary>
        /// (UTC+12:00) 协调世界时+12
        /// </summary>
        [Description("(UTC+12:00) 协调世界时+12")] public const string Utc12 = "UTC+12";

        /// <summary>
        /// (UTC+12:00) 奥克兰，惠灵顿
        /// </summary>
        [Description("(UTC+12:00) 奥克兰，惠灵顿")]
        public const string NewZealandStandardTime = "New Zealand Standard Time";

        /// <summary>
        /// (UTC+12:00) 彼得罗巴甫洛夫斯克-堪察加 - 旧用
        /// </summary>
        [Description("(UTC+12:00) 彼得罗巴甫洛夫斯克-堪察加 - 旧用")]
        public const string KamchatkaStandardTime = "Kamchatka Standard Time";

        /// <summary>
        /// (UTC+12:00) 斐济
        /// </summary>
        [Description("(UTC+12:00) 斐济")] public const string FijiStandardTime = "Fiji Standard Time";

        /// <summary>
        /// (UTC+12:00) 阿纳德尔，堪察加彼得罗巴甫洛夫斯克
        /// </summary>
        [Description("(UTC+12:00) 阿纳德尔，堪察加彼得罗巴甫洛夫斯克")]
        public const string RussiaTimeZone11 = "Russia Time Zone 11";

        /// <summary>
        /// (UTC+12:45) 查塔姆群岛
        /// </summary>
        [Description("(UTC+12:45) 查塔姆群岛")]
        public const string ChathamIslandsStandardTime = "Chatham Islands Standard Time";

        /// <summary>
        /// (UTC+13:00) 努库阿洛法
        /// </summary>
        [Description("(UTC+13:00) 努库阿洛法")] public const string TongaStandardTime = "Tonga Standard Time";

        /// <summary>
        /// (UTC+13:00) 协调世界时+13
        /// </summary>
        [Description("(UTC+13:00) 协调世界时+13")] public const string Utc13 = "UTC+13";

        /// <summary>
        /// (UTC+13:00) 萨摩亚群岛
        /// </summary>
        [Description("(UTC+13:00) 萨摩亚群岛")] public const string SamoaStandardTime = "Samoa Standard Time";

        /// <summary>
        /// (UTC+14:00) 圣诞岛
        /// </summary>
        [Description("(UTC+14:00) 圣诞岛")] public const string LineIslandsStandardTime = "Line Islands Standard Time";
    }
}