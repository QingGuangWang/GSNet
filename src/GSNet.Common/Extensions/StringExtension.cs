using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GSNet.Common.Extensions
{
    /// <summary>
    /// 字符串常用的一些扩展方法
    /// </summary>
    public static class StringExtension
    {
        #region 字符串的通用校验判断

        /// <summary>
        /// 验证字符串是否是NULL值，或者是空字符串。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>如果通过验证值为True，否则为false</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 验证字符串是否是NULL值，或者是仅为空格组成。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>如果通过验证值为True，否则为false</returns>
        public static bool IsNullOrBlank(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 验证字符串是否是 非NULL值且非空字符串。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>如果通过验证值为True，否则为false</returns>
        public static bool NotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 验证字符串是否是 非NULL值且不止是空格组成。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>如果通过验证值为True，否则为false</returns>
        public static bool NotNullOrBlank(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 判断字符串是否相等（忽略大小写）
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="comparedStr">比较的字符串</param>
        public static bool EqualsIgnoreCase(this string str, string comparedStr)
        {
            return str.Equals(comparedStr, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 判断字符串是否以指定字符串开头（忽略大小写）
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="comparedStr">比较的字符串</param>
        public static bool StartsWithIgnoreCase(this string str, string comparedStr)
        {
            return str.StartsWith(comparedStr, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 判断字符串是否以指定字符串结尾（忽略大小写）
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="comparedStr">比较的字符串</param>
        public static bool EndsWithIgnoreCase(this string str, string comparedStr)
        {
            return str.EndsWith(comparedStr, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 检查是否包含指定的子串（忽略大小写）
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="subString">子字符串</param>
        /// <returns>检查结果</returns>
        public static bool ContainsIgnoreCase(this string str, string subString)
        {
            return str.ToUpper().Contains(subString.ToUpper());
        }

        /// <summary>
        /// 判断字符串是否匹配正则表达式。
        /// </summary>
        /// <param name="str">要搜索匹配项的字符串</param>
        /// <param name="regexPattern">要匹配的正则表达式模式(字符串形式表示)。</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
        public static bool IsMatch(this string str, string regexPattern)
        {
            if (str.IsNullOrBlank())
            {
                return false;
            }

            var regex = new Regex(regexPattern);
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 判断字符串是否匹配正则表达式。
        /// </summary>
        /// <param name="str">要搜索匹配项的字符串</param>
        /// <param name="regex">要匹配的正则表达式</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
        public static bool IsMatch(this string str, Regex regex)
        {
            if (str.IsNullOrBlank())
            {
                return false;
            }

            return regex.IsMatch(str);
        }

        /// <summary>
        /// 字符串所有字符是不是都是大写
        /// </summary>
        /// <param name="input">需要判断字符串</param>
        /// <returns>如果通过验证值为True，否则为false</returns>
        public static bool IsAllUpperCase(this string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region 字符串分割

        /// <summary>
        /// 基于string.Split方法，按给定分隔符拆分给定字符串
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="separator">分隔符</param>
        /// <returns>拆分的字符串数组</returns>
        public static string[] Split(this string input, string separator)
        {
            return input.Split(new[] { separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// 基于string.Split方法，按给定分隔符拆分给定字符串
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="separator">分隔符</param>
        /// <param name="options">字符串分割的参数选项</param>
        /// <returns>拆分的字符串数组</returns>
        public static string[] Split(this string input, string separator, StringSplitOptions options)
        {
            return input.Split(new[] { separator }, options);
        }

        /// <summary>
        /// 在由正则表达式模式定义的位置拆分输入字符串。
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <returns>拆分的字符串数组</returns>
        public static string[] Split(this string input, Regex regex)
        {
            return regex.Split(input);
        }

        /// <summary>
        /// 在由正则表达式模式定义的位置拆分输入字符串。
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="pattern">正则表达式模式字符串</param>
        /// <returns>拆分的字符串数组</returns>
        public static string[] SplitToRegex(this string input, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.Split(input);
        }

        /// <summary>
        /// 基于string.Split方法，按换行符（<see cref="Environment.NewLine"/>）拆分给定字符串。
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>拆分的字符串数组</returns>
        public static string[] SplitToLines(this string str)
        {
            return str.Split(Environment.NewLine);
        }

        /// <summary>
        /// 基于string.Split方法，按换行符（<see cref="Environment.NewLine"/>）拆分给定字符串。
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="options">字符串分割的参数选项</param>
        /// <returns>拆分的字符串数组</returns>
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return str.Split(Environment.NewLine, options);
        }


        #endregion

        #region 字符串拼接/追加

        /// <summary>
        /// 如果给定字符串不以指定的字符结尾，则在其结尾添加这个指定的字符。 
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="c">要结尾的字符</param>
        /// <param name="comparisonType">字符串比较类型，默认 StringComparison.Ordinal </param>
        /// <returns>处理后的字符串</returns>
        public static string EnsureEndsWith(this string str, char c, StringComparison comparisonType = StringComparison.Ordinal)
        {
            Check.Argument.IsNotNull(str, nameof(str));

            if (str.EndsWith(c.ToString(), comparisonType))
            {
                return str;
            }

            return str + c;
        }

        /// <summary>
        /// 如果给定字符串不以指定的字符开始，则在其开始添加这个指定的字符。 
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="c">要作为开始的字符</param>
        /// <param name="comparisonType">字符串比较类型，默认 StringComparison.Ordinal</param>
        /// <returns>处理后的字符串</returns>
        public static string EnsureStartsWith(this string str, char c, StringComparison comparisonType = StringComparison.Ordinal)
        {
            Check.Argument.IsNotNull(str, nameof(str));

            if (str.StartsWith(c.ToString(), comparisonType))
            {
                return str;
            }

            return c + str;
        }

        /// <summary>
        /// 串接字符
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="split">连接符</param>
        /// <param name="value">需要串接的字符</param>
        /// <returns>串接后的字符</returns>
        public static string JoinString(this string str, string split, string value)
        {
            if (value.IsNullOrBlank())
            {
                return str;
            }

            if (str.IsNullOrBlank())
            {
                str = value;
                return str;
            }

            str += split + value;

            return str;
        }

        #endregion

        #region 字符串截取

        /// <summary>
        /// 从字符串的开头开始计算, 截取指定的长度的子字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Left(this string str, int len)
        {
            Check.Argument.IsNotNull(str, nameof(str));

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

        /// <summary>
        /// Gets a substring of a string from end of the string.
        /// 从字符串的结尾开始计算, 截取指定的长度的子字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Right(this string str, int len)
        {
            Check.Argument.IsNotNull(str, nameof(str));

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// 如果字符串超过最大长度，则从该字符串的 开头 获取该字符串的子字符串。
        /// 如： maxLength 为 5， HelloWorld ，从开头 获取，最终 就会变成 Hello， 而World被截取掉了。
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Left(maxLength);
        }

        /// <summary>
        /// 如果字符串超过最大长度，则从该字符串的 结尾 获取该字符串的子字符串。
        /// 如： maxLength 为 5， HelloWorld ，从 结尾 获取，最终 就会变成 World， 而Hello被截取掉了。
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string TruncateFromBeginning(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Right(maxLength);
        }

        /// <summary>
        /// 如果字符串超过最大长度，则从该字符串的开头获取该字符串的子字符串。
        /// 如果字符串被截断，它会将给定的 结尾字符串（<paramref name="postfix"/>） 添加到字符串的末尾。
        /// 返回的字符串不能长于maxLength。 
        /// </summary>
        /// <param name="str">需要被截取的字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="postfix">结尾字符串</param>
        public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty || maxLength == 0)
            {
                return string.Empty;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (maxLength <= postfix.Length)
            {
                return postfix.Left(maxLength);
            }

            return str.Left(maxLength - postfix.Length) + postfix;
        }

        /// <summary>
        /// 从给定字符串的结尾移除给定后缀的第一个匹配项。
        /// </summary>
        /// <param name="str">要被处理的字符串</param>
        /// <param name="postFixes">一个或多个后缀值</param>
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            return str.RemovePostFix(StringComparison.Ordinal, postFixes);
        }

        /// <summary>
        /// 从给定字符串的结尾移除给定后缀的第一个匹配项。
        /// </summary>
        /// <param name="str">要被处理的字符串</param>
        /// <param name="comparisonType">比较类型，默认 StringComparison.Ordinal</param>
        /// <param name="postFixes">一个或多个后缀值</param>
        public static string RemovePostFix(this string str, StringComparison comparisonType, params string[] postFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix, comparisonType))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// 从给定字符串的开头移除给定前缀的第一个匹配项。
        /// </summary>
        /// <param name="str">要被处理的字符串</param>
        /// <param name="preFixes">一个或多个前缀值</param>
        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            return str.RemovePreFix(StringComparison.Ordinal, preFixes);
        }

        /// <summary>
        /// 从给定字符串的开头移除给定前缀的第一个匹配项。
        /// </summary>
        /// <param name="str">要被处理的字符串</param>
        /// <param name="comparisonType">比较类型，默认 StringComparison.Ordinal</param>
        /// <param name="preFixes">一个或多个前缀值</param>
        public static string RemovePreFix(this string str, StringComparison comparisonType, params string[] preFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix, comparisonType))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }

        #endregion

        #region 字符串格式化

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="target">需要被格式化的字符串</param>
        /// <param name="args">格式化替换的参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string target, params object[] args)
        {
            return string.Format(target, args);
        }

        /// <summary>
        /// 将字符串从 camelCase（驼峰命名法）转换为 PascalCase（帕斯卡命名法）
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="useCurrentCulture">是否使用当前文化Culture</param>
        public static string ToPascalCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return useCurrentCulture ? str.ToUpper() : str.ToUpperInvariant();
            }

            return (useCurrentCulture ? char.ToUpper(str[0]) : char.ToUpperInvariant(str[0])) + str.Substring(1);
        }

        /// <summary>
        /// 将字符串从PascalCase（帕斯卡命名法）转换为camelCase（驼峰命名法）
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="useCurrentCulture">是否使用当前文化Culture</param>
        /// <param name="handleAbbreviations">是否处理缩写，如'XYZ' to 'xyz'.</param>
        /// <returns>camelCase of the string</returns>
        public static string ToCamelCase(this string str, bool useCurrentCulture = false, bool handleAbbreviations = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return useCurrentCulture ? str.ToLower() : str.ToLowerInvariant();
            }

            if (handleAbbreviations && IsAllUpperCase(str))
            {
                return useCurrentCulture ? str.ToLower() : str.ToLowerInvariant();
            }

            return (useCurrentCulture ? char.ToLower(str[0]) : char.ToLowerInvariant(str[0])) + str.Substring(1);
        }

        /// <summary>
        /// 将 PascalCase（帕斯卡命名法）/camelCase（驼峰命名法） 字符串 转换成句子.
        /// 例子: "ThisIsSampleSentence" 转化为 "This is a sample sentence".
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="useCurrentCulture">是否使用当前文化Culture</param>
        public static string ToSentenceCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return useCurrentCulture
                ? Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]))
                : Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLowerInvariant(m.Value[1]));
        }

        /// <summary>
        ///  将 PascalCase（帕斯卡命名法）/camelCase（驼峰命名法） 字符串 转换成 kebab-case（(短横线隔开式命名法）.
        ///  例子: "SellingOrder" 转化为 "Selling-Order". 
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="useCurrentCulture">是否使用当前文化Culture</param>
        public static string ToKebabCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            str = str.ToCamelCase();

            return useCurrentCulture
                ? Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + "-" + char.ToLower(m.Value[1]))
                : Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + "-" + char.ToLowerInvariant(m.Value[1]));
        }

        /// <summary>
        /// 将 PascalCase（帕斯卡命名法）/camelCase（驼峰命名法） 字符串 转换成 snake case（蛇形命名法）.
        /// 例子: "ThisIsSampleSentence" is converted to "this_is_a_sample_sentence". 
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        public static string ToSnakeCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            var builder = new StringBuilder(str.Length + Math.Min(2, str.Length / 5));
            var previousCategory = default(UnicodeCategory?);

            for (var currentIndex = 0; currentIndex < str.Length; currentIndex++)
            {
                var currentChar = str[currentIndex];
                if (currentChar == '_')
                {
                    builder.Append('_');
                    previousCategory = null;
                    continue;
                }

                var currentCategory = char.GetUnicodeCategory(currentChar);
                switch (currentCategory)
                {
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                        if (previousCategory == UnicodeCategory.SpaceSeparator ||
                            previousCategory == UnicodeCategory.LowercaseLetter ||
                            previousCategory != UnicodeCategory.DecimalDigitNumber &&
                            previousCategory != null &&
                            currentIndex > 0 &&
                            currentIndex + 1 < str.Length &&
                            char.IsLower(str[currentIndex + 1]))
                        {
                            builder.Append('_');
                        }

                        currentChar = char.ToLower(currentChar);
                        break;

                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (previousCategory == UnicodeCategory.SpaceSeparator)
                        {
                            builder.Append('_');
                        }
                        break;

                    default:
                        if (previousCategory != null)
                        {
                            previousCategory = UnicodeCategory.SpaceSeparator;
                        }
                        continue;
                }

                builder.Append(currentChar);
                previousCategory = currentCategory;
            }

            return builder.ToString();
        }

        /// <summary>
        /// 符号过滤
        /// </summary>
        private static readonly string[] BASIC_SYMBOLS = { " ", ",", ".", "&", "%", "#", "@", "!", "*", "(", ")" };

        /// <summary>
        /// 消除基础的符号 (" ", ",", ".", "&", "%", "#", "@", "!", "*", "(", ")")
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>消除标点符号后的字符串</returns>
        public static string CleanBasicSymbol(this string str)
        {
            return BASIC_SYMBOLS.Aggregate(str, (current, s) => current.Replace(s, ""));
        }

        /// <summary>
        /// 消除常见的标点符号
        /// []^-_*×―(^)（^）$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;"‘’“”- 
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>消除标点符号后的字符串</returns>
        public static string CleanPunctuationSymbol(this string str)
        {
            return CommonRegex.PUNCTUATION.Replace(str, "");
        }

        /// <summary>
        /// 格式化字符串，清除前后空格，中间多个空格并成一个。
        /// 清除常见转义字符(\r \n \t)，多个空格变成一个
        /// </summary>
        public static string NormalizeSpace(this string str)
        {
            return CommonRegex.SPACE_MORE_THAN_ONE_CHAR.Replace(
                CommonRegex.LINE_FEED.Replace(str.Trim(), " "), " ");
        }

        /// <summary>
        /// 将字符串中的行尾转换为系统环境的换行符（<see cref="Environment.NewLine"/>）
        /// </summary>
        /// <param name="str">要被格式化处理的字符串</param>
        /// <returns>行尾处理好的字符串</returns>
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        #endregion

        #region 字符串替换

        /// <summary>
        /// 从输入字符串中的第一个字符开始，用替换字符串替换指定的正则表达式模式的所有匹配项。
        /// </summary>
        /// <param name="input">源字符串</param>EqualsIgnoreCase
        /// <param name="pattern">模式字符串</param>
        /// <param name="replacement">用于替换的字符串</param>
        /// <returns>返回被替换后的结果</returns>
        public static string ReplaceByRegex(this string input, string pattern, string replacement)
        {
            var regex = new Regex(pattern);
            return regex.Replace(input, replacement);
        }

        /// <summary>
        /// 替换字符串，只替换第一个匹配的
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="search">查找的字符串</param>
        /// <param name="replace">需要替换成的字符串</param>
        /// <param name="comparisonType">比较类型，默认 StringComparison.Ordinal</param>
        /// <returns></returns>
        public static string ReplaceFirst(this string str, string search, string replace, StringComparison comparisonType = StringComparison.Ordinal)
        {
            Check.Argument.IsNotNull(str, nameof(str));

            var pos = str.IndexOf(search, comparisonType);
            if (pos < 0)
            {
                return str;
            }

            return str.Substring(0, pos) + replace + str.Substring(pos + search.Length);
        }

        #endregion
    
        /// <summary>
        /// 获取字符串中第n个指定字符的索引。
        /// n 为3， 查找字符 '#', 则返回 第三个 '#'在字符串的索引。 若查找不到，或者 '#'没有3个，则返回 -1
        /// </summary>
        /// <param name="str">源字符串（被查找的字符串）</param>
        /// <param name="c">要在字符串（<paramref name="str"/>）查找的字符</param>
        /// <param name="n">字符的数量</param>
        public static int NthIndexOf(this string str, char c, int n)
        {
            Check.Argument.IsNotNull(str, nameof(str));

            var count = 0;
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != c)
                {
                    continue;
                }

                if ((++count) == n)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 计算字符串的字符长度，一个汉字字符将被计算为两个字符
        /// </summary>
        /// <param name="input">需要计算的字符串</param>
        /// <returns>返回字符串的长度</returns>
        public static int GetCount(this string input)
        {
            return Regex.Replace(input, @"[\u4e00-\u9fa5/g]", "aa").Length;
        }

        /// <summary>
        /// 判断字符串compare 在 input字符串中出现的次数
        /// 
        /// input.GetStringCount("::"); 返回::在字符串出现的次数
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="compare">用于比较的字符串</param>
        /// <returns>字符串compare 在 input字符串中出现的次数</returns>
        public static int GetStringCount(this string input, string compare)
        {
            if (input == null)
            {
                return 0;
            }

            var index = input.IndexOf(compare, StringComparison.Ordinal);
            if (index != -1)
            {
                return 1 + GetStringCount(input.Substring(index + compare.Length), compare);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 基于UTF-8的编码方式（<see cref="Encoding.UTF8"/>），字符串转化为一个字节Byte数组。
        /// </summary>
        public static byte[] GetBytes(this string str)
        {
            return str.GetBytes(Encoding.UTF8);
        }

        /// <summary>
        /// 基于给定的编码方式（<paramref name="encoding"/>），将字符串转化为一个字节Byte数组。
        /// </summary>
        public static byte[] GetBytes(this string str, Encoding encoding)
        {
            Check.Argument.IsNotNull(str, nameof(str));
            Check.Argument.IsNotNull(encoding, nameof(encoding));

            return encoding.GetBytes(str);
        }
    }
}