using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Helper
{
    /// <summary>
    /// 比较字符串相识度辅助器
    /// </summary>
    public static class StringSimilarityHelper
    {
        /// <summary>
        /// 计算两个字符串的编辑距离， 基于Levenshtein Distance算法实现
        /// 
        /// 编辑距离（Edit Distance），又称Levenshtein距离，是指两个字串之间，由一个转成另一个所需的最少编辑操作次数,
        /// 许可的编辑操作包括将一个字符替换成另一个字符，插入一个字符，删除一个字符。
        /// 一般来说，编辑距离越小，两个串的相似度越大。
        /// </summary>
        /// <param name="str1">字符串1</param>
        /// <param name="str2">字符串2</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns>编辑距离</returns>
        public static int CalcEditDistance(string str1, string str2, bool ignoreCase = false)
        {
            if (str1 == null || str2 == null)
            {
                throw new ArgumentNullException();
            }

            var source = ignoreCase ? str1.ToLower() : str1;
            var target = ignoreCase ? str2.ToLower() : str2;

            #region

            var sourceLength = source.Length;
            var targetLength = target.Length;

            var d = new int[sourceLength + 1, targetLength + 1];

            //step1
            if (sourceLength == 0)
                return targetLength;

            if (targetLength == 0)
                return sourceLength;

            //step2
            for (var i = 0; i <= sourceLength; d[i, 0] = i++)
            {
            }
            for (var j = 0; j <= targetLength; d[0, j] = j++)
            {
            }

            //step3
            for (var i = 1; i <= sourceLength; i++)
            {
                for (var j = 1; j <= targetLength; j++)
                {
                    var cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[sourceLength, targetLength];

            #endregion
        }

        /// <summary>
        ///  计算最长公共子序列（Longest Common Subsequence）
        /// 
        ///  一个序列，如果是两个或多个已知序列的子序列，且是所有子序列中最长的，则为最长公共子序列。
        /// </summary>
        public static int CalcLongestCommonSubsequence(string source, string target, bool ignoreCase = true)
        {
            if (source == null || target == null || source.Length == 0 || target.Length == 0)
                return 0;

            if (ignoreCase)
            {
                source = source.ToLower();
                target = source.ToLower();
            }

            var len = Math.Max(target.Length, source.Length);
            var subsequence = new int[len + 1, len + 1];

            for (var i = 0; i < source.Length; i++)
            {
                for (var j = 0; j < target.Length; j++)
                {
                    if (source[i].Equals(target[j]))
                        subsequence[i + 1, j + 1] = subsequence[i, j] + 1;
                    else
                        subsequence[i + 1, j + 1] = 0;
                }
            }

            var maxSubsequenceLength = (from sq in subsequence.Cast<int>() select sq).Max<int>();
            return maxSubsequenceLength;
        }

        /// <summary>
        /// 计算两字符串的相似度，结合Levenshtein Distance + LCS算法
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="target">目标字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns>字符串的相似度（越大越相识）</returns>
        public static float CalcStringSimilarity(string source, string target, bool ignoreCase = true)
        {
            var ld = CalcEditDistance(source, target, ignoreCase);
            var lcs = CalcLongestCommonSubsequence(source, target);
            return ((float)lcs) / (ld + lcs); ;
        }

        /// <summary>
        /// 从一批字符串值中<paramref name="sources"/>，计算相似度最接近目标字符串<paramref name="target"/>的一个
        /// </summary>
        /// <param name="sources">源字符串值</param>
        /// <param name="target">目标字符串</param>
        /// <param name="acceptableSimilarity">最低的相似度</param>
        /// <returns>最相似的</returns>
        public static string CalcMostSimilarityString(List<string> sources, string target, float acceptableSimilarity = 0.8f)
        {
            var keywords = new Dictionary<string, float>();

            for (int i = 0; i < sources.Count; i++)
            {
                float matchValue = CalcStringSimilarity(t, target);
                if (matchValue >= acceptableSimilarity)
                {
                    keywords.Add(t, matchValue);
                }
            }

            if (keywords.Any())
            {
                return keywords.OrderByDescending(p => p.Value).Select(x => x.Key).First();
            }

            return null;
        }
    }
}
