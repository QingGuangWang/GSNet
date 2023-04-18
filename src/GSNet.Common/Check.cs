using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GSNet.Common
{
    /// <summary>
    /// 检查器
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// 用于检查和验证方法参数
        /// </summary>
        public static class Argument
        {
            /// <summary>
            /// 断言字符串参数既不为null，也不为空字符串。
            /// </summary>
            /// <param name="argument">字符串类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <exception cref="ArgumentException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsNotNullOrEmpty(string argument, string argumentName)
            {
                if (String.IsNullOrEmpty(argument))
                {
                    throw new ArgumentException(string.Format(CheckResources.ArgumentCannotBeNullOrEmptyString, argumentName), argumentName);
                }
            }

            /// <summary>
            /// 断言给定字符串参数具有指定的长度。
            /// </summary>
            /// <param name="argument">字符串类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <param name="expectedLength">字符串预期的长度</param>
            /// <exception cref="ArgumentOutOfRangeException">不符合则抛出此异常</exception>
            public static void HasExactLength(string argument, string argumentName, int expectedLength)
            {
                if (argument == null || argument.Length != expectedLength)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            /// <summary>
            /// 断言字节数组既不为null，也不为空数组。
            /// </summary>
            /// <param name="argument">字节数组类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <exception cref="ArgumentException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsNotNullOrEmpty(byte[] argument, string argumentName)
            {
                if (argument == null || argument.Length <= 0)
                {
                    throw new ArgumentException(string.Format(CheckResources.ArgumentCannotBeNullOrEmptyArray, argumentName), argumentName);
                }
            }

            /// <summary>
            /// 断言字符数组既不为null，也不为空数组。
            /// </summary>
            /// <param name="argument">字符数组类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <exception cref="ArgumentException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsNotNullOrEmpty(char[] argument, string argumentName)
            {
                if (argument == null || argument.Length <= 0)
                {
                    throw new ArgumentException(string.Format(CheckResources.ArgumentCannotBeNullOrEmptyArray, argumentName), argumentName);
                }
            }

            /// <summary>
            /// 断言列表既不为null，也不为空列表。
            /// </summary>
            /// <param name="argument">列表类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <exception cref="ArgumentException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsNotNullOrEmpty<T>(IList<T> argument, string argumentName)
            {
                if (argument == null || argument.Count <= 0)
                {
                    throw new ArgumentException(string.Format(CheckResources.ArgumentCannotBeNullOrEmptyCollection, argumentName), argumentName);
                }
            }

            /// <summary>
            /// 断言一个对象类型既不为null
            /// </summary>
            /// <param name="argument">对象类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <exception cref="ArgumentException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsNotNull(object argument, string argumentName)
            {
                if (argument == null)
                {
                    throw new ArgumentException(string.Format(CheckResources.ArgumentCannotBeNull, argumentName), argumentName);
                }
            }

            /// <summary>
            ///  断言一个整型类型参数的值是在给定的数字区间里面。
            /// </summary>
            /// <param name="argument">整型类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <param name="min">最小值</param>
            /// <param name="max">最大值</param>
            /// <exception cref="ArgumentOutOfRangeException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsInRange(int argument, string argumentName, int min, int max)
            {
                if (argument < min || argument > max)
                {
                    throw new ArgumentOutOfRangeException(argumentName, string.Format(CheckResources.ArgumentCannotBeNull, argumentName, min, max));
                }
            }

            /// <summary>
            /// 断言一个时间日期参数的值不是小于当前时间的（服务器时区时间，非UTC时间）。
            /// </summary>
            /// <param name="argument">日期时间类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <exception cref="ArgumentOutOfRangeException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsNotInPast(DateTime argument, string argumentName)
            {
                if (argument < DateTime.Now)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            /// <summary>
            /// 断言一个时间跨度类型的值大于零。
            /// </summary>
            /// <param name="argument">时间跨度类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <exception cref="ArgumentOutOfRangeException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsNotNegativeOrZero(TimeSpan argument, string argumentName)
            {
                if (argument <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            /// <summary>
            ///  断言一个整型类型参数的值是大于0。
            /// </summary>
            /// <param name="argument">整型类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <exception cref="ArgumentOutOfRangeException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsNotNegativeOrZero(int argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            /// <summary>
            ///  断言一个数字类型参数的值是大于0。
            /// </summary>
            /// <param name="argument">数字类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <exception cref="ArgumentOutOfRangeException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsNotNegativeOrZero(decimal argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            /// <summary>
            ///  断言一个数字类型参数的值是大于或者等于0。
            /// </summary>
            /// <param name="argument">数字类型参数</param>
            /// <param name="argumentName">参数名称</param>
            /// <exception cref="ArgumentOutOfRangeException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsNotNegative(decimal argument, string argumentName)
            {
                if (argument < 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            /// <summary>
            /// 断言原参数值和目标参数值相等
            /// </summary>
            /// <param name="sourceAruemntValue">原参数值</param>
            /// <param name="destAruemntValue">目标参数值</param>
            /// <param name="sourceArgumentName">原参数名</param>
            /// <param name="destArgumentName">目标参数值</param>
            /// <exception cref="ArgumentOutOfRangeException">不符合则抛出此异常</exception>
            [DebuggerStepThrough]
            public static void IsEqual(int sourceAruemntValue, int destAruemntValue, string sourceArgumentName, string destArgumentName)
            {
                if (sourceAruemntValue != destAruemntValue)
                {
                    throw new ArgumentOutOfRangeException(sourceArgumentName, string.Format("{0}  must be equal {1}", sourceArgumentName, destArgumentName));
                }
            }
        }
    }
}
