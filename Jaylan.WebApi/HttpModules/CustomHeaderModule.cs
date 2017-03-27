using System;
using System.Web;

namespace Jaylan.WebApi.HttpModules
{
    public class CustomHeaderModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        public void Dispose()
        {

        }

        private static void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Headers.Remove("Server");
            HttpContext.Current.Response.Headers.Set("Server", "Jaylan Server");
        }
    }
}