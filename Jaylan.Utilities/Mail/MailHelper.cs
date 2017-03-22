using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Jaylan.Utilities.Mail
{
    /// <summary>
    /// 邮件操作类
    /// </summary>
    public class MailHelper
    {
        /// <summary>
        /// 邮件发送
        /// 对收件人、发件人、抄送人、密送人的邮箱地址
        /// 不作是否是邮箱地址校验，请调用前做好校验
        /// </summary>
        /// <param name="mailSubjct">邮件主题</param>
        /// <param name="mailBody">邮件正文</param>
        /// <param name="isBodyHtml">是否是html</param>
        /// <param name="mailFrom">发件人</param>
        /// <param name="disPlayName">发件人显示名称</param>
        /// <param name="toMailAddressList">收件人地址列表</param>
        /// <param name="hostIp">邮件主机主机Ip</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="sendOk">是否发送成功</param>
        /// <returns>返回发送错误信息</returns>
        public static string SendEmail(string mailSubjct, string mailBody, bool isBodyHtml, string mailFrom, string disPlayName, List<string> toMailAddressList, string hostIp, string userName, string passWord, out bool sendOk)
        {
            var errorStr = SendEmail(mailSubjct, mailBody, isBodyHtml, mailFrom, disPlayName, toMailAddressList, hostIp, 25, userName, passWord, out sendOk);
            return errorStr;
        }

        /// <summary>
        /// 邮件发送
        /// 对收件人、发件人、抄送人、密送人的邮箱地址
        /// 不作是否是邮箱地址校验，请调用前做好校验
        /// </summary>
        /// <param name="mailSubjct">邮件主题</param>
        /// <param name="mailBody">邮件正文</param>
        /// <param name="isBodyHtml">是否是html</param>
        /// <param name="mailFrom">发件人</param>
        /// <param name="disPlayName">发件人显示名称</param>
        /// <param name="toMailAddressList">收件人地址列表</param>
        /// <param name="hostIp">邮件主机主机Ip</param>
        /// <param name="port">端口号</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="sendOk">是否发送成功</param>
        /// <returns>返回发送错误信息</returns>
        public static string SendEmail(string mailSubjct, string mailBody, bool isBodyHtml, string mailFrom, string disPlayName, List<string> toMailAddressList, string hostIp, int port, string userName, string passWord, out bool sendOk)
        {
            var errorStr = SendEmail(mailSubjct, mailBody, isBodyHtml, mailFrom, disPlayName, null, toMailAddressList, hostIp, port, userName, passWord, false, true, out sendOk);
            return errorStr;
        }

        /// <summary>
        /// 邮件发送
        /// 对收件人、发件人、抄送人、密送人的邮箱地址
        /// 不作是否是邮箱地址校验，请调用前做好校验
        /// </summary>
        /// <param name="mailSubjct">邮件主题</param>
        /// <param name="mailBody">邮件正文</param>
        /// <param name="isBodyHtml">是否是html</param>
        /// <param name="mailFrom">发件人</param>
        /// <param name="disPlayName">发件人显示名称</param>
        /// <param name="replyTo">回复邮箱地址</param>
        /// <param name="toMailAddressList">收件人地址列表</param>
        /// <param name="hostIp">邮件主机主机Ip</param>
        /// <param name="port">端口号</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="enableSsl">是否启用ssl加密</param>
        /// <param name="isEnablePwdAuthentication">是否对发件人邮箱进行密码验证</param>
        /// <param name="sendOk">是否发送成功</param>
        /// <returns>返回发送错误信息</returns>
        public static string SendEmail(string mailSubjct, string mailBody, bool isBodyHtml, string mailFrom, string disPlayName, string replyTo, List<string> toMailAddressList, string hostIp, int port, string userName, string passWord, bool enableSsl, bool isEnablePwdAuthentication, out bool sendOk)
        {
            var errorStr = SendEmail(mailSubjct, mailBody, isBodyHtml, mailFrom, disPlayName, null, toMailAddressList, new List<string>(), new List<string>(), hostIp, port, userName, passWord, enableSsl, isEnablePwdAuthentication, out sendOk);
            return errorStr;
        }

        /// <summary>
        /// 邮件发送
        /// 对收件人、发件人、抄送人、密送人的邮箱地址
        /// 不作是否是邮箱地址校验，请调用前做好校验
        /// </summary>
        /// <param name="mailSubjct">邮件主题</param>
        /// <param name="mailBody">邮件正文</param>
        /// <param name="isBodyHtml">是否是html</param>
        /// <param name="mailFrom">发件人</param>
        /// <param name="disPlayName">发件人显示名称</param>
        /// <param name="replyTo">回复邮箱地址</param>
        /// <param name="toMailAddressList">收件人地址列表</param>
        /// <param name="carbonCopyList">抄送列表</param>
        /// <param name="blindCarbonCopyList">密送列表</param>
        /// <param name="hostIp">邮件主机主机Ip</param>
        /// <param name="port">端口号</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="enableSsl">是否启用ssl加密</param>
        /// <param name="isEnablePwdAuthentication">是否对发件人邮箱进行密码验证</param>
        /// <param name="sendOk">是否发送成功</param>
        /// <returns>返回发送错误信息</returns>
        public static string SendEmail(string mailSubjct, string mailBody, bool isBodyHtml, string mailFrom, string disPlayName, string replyTo, List<string> toMailAddressList, List<string> carbonCopyList, List<string> blindCarbonCopyList, string hostIp, int port, string userName, string passWord, bool enableSsl, bool isEnablePwdAuthentication, out bool sendOk)
        {
            sendOk = false;
            string errorStr = null;
            try
            {
                using (var mailMessage = new MailMessage
                {
                    IsBodyHtml = isBodyHtml,
                    Subject = mailSubjct,
                    Body = mailBody,
                    BodyEncoding = Encoding.UTF8,
                    From = new MailAddress(mailFrom, disPlayName, Encoding.UTF8)
                })
                {
                    if (!string.IsNullOrEmpty(replyTo))
                    {
                        var replyToAddress = new MailAddress(replyTo);
                        mailMessage.ReplyToList.Add(replyToAddress);
                    }
                    foreach (var to in toMailAddressList)
                    {
                        mailMessage.To.Add(to);
                    }
                    foreach (var cc in carbonCopyList)
                    {
                        mailMessage.CC.Add(cc);
                    }
                    foreach (var bcc in blindCarbonCopyList)
                    {
                        mailMessage.Bcc.Add(bcc);
                    }
                    if (mailMessage.To.Count == 0)
                    {
                        return "收件人为空";
                    }
                    using (var smtpClient = new SmtpClient
                    {
                        EnableSsl = enableSsl,
                        UseDefaultCredentials = false,
                        Host = hostIp,
                        Port = port,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    })
                    {
                        var indexOf = userName.IndexOf('@');
                        if (indexOf > 0)
                        {
                            userName = userName.Substring(0, indexOf);
                        }
                        if (isEnablePwdAuthentication)
                        {
                            var credential = new NetworkCredential(userName, passWord);
                            //NTLM: Secure Password Authentication in Microsoft Outlook Express
                            smtpClient.Credentials = credential.GetCredential(smtpClient.Host, smtpClient.Port, "NTLM");
                        }
                        else
                        {
                            smtpClient.Credentials = new NetworkCredential(userName, passWord);
                        }
                        smtpClient.Send(mailMessage);
                        sendOk = true;
                    }
                }
            }
            catch (Exception exception)
            {
                errorStr = exception.Message;
                sendOk = false;
            }
            return errorStr;
        }
    }
}
