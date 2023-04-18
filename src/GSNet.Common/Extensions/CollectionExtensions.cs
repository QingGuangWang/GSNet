using GSNet.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Extensions
{
    /// <summary>
    /// Collections 一些扩展方法.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 判断一个 ICollection 的实例，是否是空或者不包含任何数据项
        /// </summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        /// <summary>
        /// 判断一个 ICollection 的实例，是否是 非空且包含数据项
        /// </summary>
        public static bool NotNullOrEmpty<T>(this ICollection<T> source)
        {
            return !IsNullOrEmpty(source);
        }

        /// <summary>
        /// 将不在集合中的数据项添加到集合中
        /// </summary>
        /// <param name="source">集合对象</param>
        /// <param name="items">需要添加到集合中的数据项</param>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <returns>返回已新增的数据项（排除已有的）</returns>
        public static IEnumerable<T> AddIfNotContains<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            Check.Argument.IsNotNull(source, nameof(source));

            var addedItems = new List<T>();

            foreach (var item in items)
            {
                if (source.Contains(item))
                {
                    continue;
                }

                source.Add(item);
                addedItems.Add(item);
            }

            return addedItems;
        }

        /// <summary>
        /// 基于给定的判断方法（参数 <paramref name="predicate"/>），判断数据项是否已经存在集合中，若不存在， 将数据项（通过参数 <paramref name="itemFactory"/> 构造数据项对象）添加到集合中。
        /// </summary>
        /// <param name="source">集合对象</param>
        /// <param name="predicate">判断数据项是否已在集合中的条件方法委托</param>
        /// <param name="itemFactory">数据项对象的构造工厂方法的委托</param>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <returns>如果添加成功，返回true, 反之为false。</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> source, Func<T, bool> predicate, Func<T> itemFactory)
        {
            Check.Argument.IsNotNull(source, nameof(source));
            Check.Argument.IsNotNull(predicate, nameof(predicate));
            Check.Argument.IsNotNull(itemFactory, nameof(itemFactory));

            if (source.Any(predicate))
            {
                return false;
            }

            source.Add(itemFactory());
            return true;
        }
    }
}
