using System;
using System.Collections.Generic;
using System.Linq;

namespace Jaylan.Utilities
{
    public static class LinqExtension
    {
        /// <summary>
        /// 根据条件去重的Linq扩展方法
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var hash = new HashSet<TKey>();

            return (from p in source
                    where hash.Add(keySelector(p))
                    select p);
        }
    }
}
