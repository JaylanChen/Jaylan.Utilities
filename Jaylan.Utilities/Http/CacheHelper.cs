using System;
using System.Web;

namespace Jaylan.Utilities.Http
{
    public class CacheHelper
    {
        /// <summary>
        /// 根据缓存主键获取缓存数据
        /// </summary>
        /// <param name="cacheKey">缓存主键</param>
        public static object GetCache(string cacheKey)
        {
            var objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">缓存主键</param>
        /// <param name="objObject">缓存数据内容</param>
        public static void SetCache(string cacheKey, object objObject)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">缓存主键</param>
        /// <param name="objObject">缓存数据内容</param>
        /// <param name="timeout">滑动过期时间</param>
        public static void SetCache(string cacheKey, object objObject, TimeSpan timeout)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, DateTime.MaxValue, timeout);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">缓存主键</param>
        /// <param name="objObject">缓存数据内容</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">缓存主键</param>
        /// <param name="objObject">缓存数据内容</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="timeout">滑动过期时间</param>
        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration, TimeSpan timeout)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, timeout);
        }

        /// <summary>
        /// 移除指定主键的缓存
        /// </summary>
        /// <param name="cacheKey">缓存主键</param>
        public static void RemoveCache(string cacheKey)
        {
            var cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            var cache = HttpRuntime.Cache;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}