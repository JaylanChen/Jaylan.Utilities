using System;
using System.Collections.Specialized;
using System.Web;

namespace Jaylan.Utilities.Http
{
    public class CookieHelper
    {
        #region 添加简单Cookie，Key-Value
        /// <summary>
        /// 添加一个Cookie（长期有效）
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="value">cookie值</param>
        public static void SetCookie(string strCookieName, string value)
        {
            SetCookie(strCookieName, value, DateTime.MaxValue);
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="value">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void SetCookie(string strCookieName, string value, DateTime expires)
        {
            SetCookie(strCookieName, value, expires, null);
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="value">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        /// <param name="domain">cookie域</param>
        public static void SetCookie(string strCookieName, string value, DateTime expires, string domain)
        {
            SetCookie(strCookieName, value, expires, domain, true);
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="value">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        /// <param name="domain">cookie域</param>
        /// <param name="httpOnly">是否禁止客户端脚本访问</param>
        public static void SetCookie(string strCookieName, string value, DateTime expires, string domain, bool httpOnly)
        {
            var httpCookie = new HttpCookie(strCookieName)
            {
                Value = HttpContext.Current.Server.UrlEncode(value),
                Expires = expires,
                HttpOnly = true
            };
            if (!string.IsNullOrEmpty(domain))
            {
                httpCookie.Domain = domain;
            }
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }
        #endregion

        #region 获取Cookie的Value
        /// <summary>
        /// 获取指定Cookie值
        /// 如果不存在对应cookie记录，则返回string.Empty
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <returns>返回对应name的Cookie值</returns>
        public static string GetCookieValue(string strCookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[strCookieName];
            var cookieValue = string.Empty;
            if (cookie != null)
            {
                cookieValue = HttpContext.Current.Server.UrlDecode(cookie.Value);
            }
            return cookieValue;
        }
        #endregion

        #region 添加一个Cookie对象，键值对集合
        /// <summary>
        /// 添加一个Cookie对象，键值对组合
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="keyName">Key键名</param>
        /// <param name="keyValue">Key键值</param>
        public static void SetObjCookie(string strCookieName, string keyName, string keyValue)
        {
            SetObjCookie(strCookieName, new NameValueCollection() { { keyName, keyValue } });
        }
        /// <summary>
        /// 添加一个Cookie对象，键值对组合
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="keyValue">键/值对集合</param>
        public static void SetObjCookie(string strCookieName, NameValueCollection keyValue)
        {
            SetObjCookie(strCookieName, keyValue, DateTime.MaxValue);
        }

        /// <summary>
        /// 添加一个Cookie对象，键值对组合
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="keyValue">键/值对集合</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void SetObjCookie(string strCookieName, NameValueCollection keyValue, DateTime expires)
        {
            SetObjCookie(strCookieName, keyValue, expires, null);
        }

        /// <summary>
        /// 添加一个Cookie对象，键值对组合
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="keyValue">键/值对集合</param>
        /// <param name="expires">过期时间 DateTime</param>
        /// <param name="domain">cookie域</param>
        public static void SetObjCookie(string strCookieName, NameValueCollection keyValue, DateTime expires, string domain)
        {
            SetObjCookie(strCookieName, keyValue, expires, domain, true);
        }

        /// <summary>
        /// 添加一个Cookie对象，键值对组合
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="keyValue">键/值对集合</param>
        /// <param name="expires">过期时间 DateTime</param>
        /// <param name="domain">cookie域</param>
        /// <param name="httpOnly">是否禁止客户端脚本访问</param>
        public static void SetObjCookie(string strCookieName, NameValueCollection keyValue, DateTime expires, string domain, bool httpOnly)
        {
            var httpCookie = new HttpCookie(strCookieName)
            {
                Expires = expires,
                HttpOnly = httpOnly
            };
            if (string.IsNullOrEmpty(domain))
            {
                httpCookie.Domain = domain;
            }
            foreach (var key in keyValue.AllKeys)
            {
                httpCookie[key] = HttpContext.Current.Server.UrlEncode(keyValue[key].Trim());
            }
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }
        #endregion

        #region 获取Cookie对象对应Key的Value
        /// <summary>
        /// 获取指定Cookie值
        /// 如果不存在对应cookie记录，则返回string.Empty
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="keyName">Key键名</param>
        /// <returns>返回对应name的Cookie值</returns>
        public static string GetObjCookieValue(string strCookieName, string keyName)
        {
            var httpCookie = HttpContext.Current.Request.Cookies[strCookieName];
            if (httpCookie != null)
            {
                return HttpContext.Current.Server.UrlDecode(httpCookie[keyName]);
            }
            return string.Empty;
        }
        #endregion

        #region 更新Cookie对象的某个Key的Value
        /// <summary>
        /// 获取指定Cookie值
        /// 如果不存在对应cookie记录，则返回string.Empty
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="keyName">Key键名</param>
        /// <param name="keyValue">Key键值</param>
        /// <returns>返回对应name的Cookie值</returns>
        public static void UpdateObjCookieValue(string strCookieName, string keyName, string keyValue)
        {
            UpdateObjCookieValue(strCookieName, keyName, keyValue, DateTime.MaxValue);
        }

        /// <summary>
        /// 获取指定Cookie值
        /// 如果不存在对应cookie记录，则返回string.Empty
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="keyName">Key键名</param>
        /// <param name="keyValue">Key键值</param>
        /// <param name="expires">过期时间 DateTime</param>
        /// <returns>返回对应name的Cookie值</returns>
        public static void UpdateObjCookieValue(string strCookieName, string keyName, string keyValue, DateTime expires)
        {
            var httpCookie = HttpContext.Current.Request.Cookies[strCookieName];
            if (httpCookie != null)
            {
                httpCookie.Expires = expires;
                httpCookie[keyName] = HttpContext.Current.Server.UrlDecode(keyValue);
                HttpContext.Current.Response.Cookies.Add(httpCookie);
            }
        }
        #endregion

        #region 移除Cookies
        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        public static void RemoveCookie(string strCookieName)
        {
            var httpCookie = HttpContext.Current.Request.Cookies[strCookieName];
            if (httpCookie != null)
            {
                httpCookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(httpCookie);
            }
        }
        #endregion

        #region 移除Cookies
        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="keyName">Key键名</param>
        public static void RemoveObjCookieValue(string strCookieName, string keyName)
        {
            RemoveObjCookieValue(strCookieName, keyName, DateTime.MaxValue);
        }

        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="strCookieName">cookie名</param>
        /// <param name="keyName">Key键名</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void RemoveObjCookieValue(string strCookieName, string keyName, DateTime expires)
        {
            var httpCookie = HttpContext.Current.Request.Cookies[strCookieName];
            if (httpCookie != null)
            {
                httpCookie.Values.Remove(keyName);
                httpCookie.Expires = expires;
                HttpContext.Current.Response.Cookies.Add(httpCookie);
            }
        }
        #endregion
    }
}