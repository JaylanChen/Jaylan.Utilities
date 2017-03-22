using System;
using System.Collections.Generic;
using System.Configuration;
using Jaylan.Utilities.Json;

namespace Jaylan.Utilities.Encrypt
{
    public class AuthenticationHelper
    {
        /// <summary>
        /// 根据请求参数生成加密Key
        /// 参数部分转小写
        /// </summary>
        /// <param name="parmList">请求参数</param>
        /// <param name="signKey">加密Key</param>
        /// <param name="isToLower">参数部分是否转小写</param>
        /// <returns></returns>
        public static string GetMD5Sign(List<string> parmList, string signKey, bool isToLower)
        {
            parmList.Sort();
            var tempStr = string.Join("&", parmList);
            if (isToLower)
            {
                tempStr = tempStr.ToLower();
            }
            tempStr = tempStr + signKey;
            var mySign = MD5Helper.GetStringMD5(tempStr);
            return mySign;
        }

        /// <summary>
        /// 验证请求参数（默认加密Key）
        /// </summary>
        /// <param name="parmList">请求参数</param>
        /// <param name="sign">请求签名</param>
        /// <returns></returns>
        public static bool MD5_Verify(List<string> parmList, string sign)
        {
            var signKey = ConfigurationManager.AppSettings["Default_SignKey"];
            return MD5_Verify(parmList, sign, signKey);
        }


        /// <summary>
        /// 验证请求参数
        /// </summary>
        /// <param name="parmList">请求参数</param>
        /// <param name="sign">请求签名</param>
        /// <param name="signKey">加密Key</param>
        /// <param name="isToLower">参数是否转小写</param>
        /// <returns></returns>
        public static bool MD5_Verify(List<string> parmList, string sign, string signKey, bool isToLower=true)
        {
            var mySign = GetMD5Sign(parmList, signKey, isToLower);
            if (string.Equals(mySign, sign, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }


        #region 待修改
        public class MessageModel
        {
            //TODO:消息Model待完善

            /// <summary>
            /// 时间戳
            /// </summary>
            public long TimeStamp { get; set; }
        }


        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="messageMode"></param>
        /// <param name="key">加密Key</param>
        /// <returns></returns>
        public static string GetToken(MessageModel messageMode, string key)
        {
            messageMode.TimeStamp = TimeStampHelper.ConvertToTimeStamp();
            //对msg部分进行base64编码。
            var msg = EncryptHelper.EncodeBase64(JsonHelper.SerializeObject(messageMode));
            //进行sha256哈希
            var signature = EncryptHelper.HMACSHA256Encrypt(msg, key);
            return msg + "." + signature;
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="key">解密Key</param>
        /// <param name="messageMode"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool ValidateToken(string token, string key, out MessageModel messageMode, out string errorMsg)
        {
            messageMode = null;
            //对输入数据的判空操作
            if (string.IsNullOrEmpty(token))
            {
                errorMsg = "token不能为空";
                return false;
            }
            var match = token.Split('.');
            var msg = match[0];
            var signature = match[1];
            if (string.IsNullOrEmpty(msg))
            {
                errorMsg = "token格式错误";
                return false;
            }
            //对解包后msg，按照相同的加密key:"testkey"，重新进行sha256哈希，比对signature，
            //如果不一致，说明这个token中的数据有问题，无效的token。
            if (signature != EncryptHelper.HMACSHA256Encrypt(msg, key))
            {
                errorMsg = "token无效";
                return false;
            }
            //对msg进行base64解码，判断其中的key和传入的key是否一致。
            //如果不一致说明token也是无效的。
            messageMode = JsonHelper.DeserializeJsonToObject<MessageModel>(EncryptHelper.DecodeBase64(msg));
            if (messageMode == null)
            {
                errorMsg = "无效的token";
                return false;
            }
            if (messageMode.TimeStamp < TimeStampHelper.ConvertToTimeStamp() - 60)
            {
                errorMsg = "token已过期";
                return false;
            }
            errorMsg = null;
            return true;
        }
        #endregion
    }
}
