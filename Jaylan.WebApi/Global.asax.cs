using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace Jaylan.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            #region Autofac注入
            //var builder = new ContainerBuilder();
            //builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where(a => a.FullName.StartsWith("Jaylan")).ToArray();
            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(t => t.Name.EndsWith("Repository"))
            //    .AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(t => t.Name.EndsWith("Service"))
            //    .AsImplementedInterfaces()
            //    .InstancePerRequest();

            //builder.RegisterAssemblyTypes(Assembly.Load("Jaylan.Service")).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            //builder.RegisterFilterProvider();

            //var container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //如果Controller中不想使用构造函数注入，或者只某个方法使用可以使用如下方法注入Service
            //var studentService = DependencyResolver.Current.GetService<IStudentService>();

            //如果WebApi ApiController中不想使用构造函数注入，或者只某个方法使用可以使用如下方法注入Service
            //IDependencyScope dependencyScope = this.Request.GetDependencyScope();
            //ILifetimeScope requestLifetimeScope = dependencyScope.GetRequestLifetimeScope();
            //var studentService = requestLifetimeScope.Resolve<IStudentService>();
            #endregion

            #region 只查找.cshtml视图
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine()
            {
                AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" },
                AreaMasterLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" },
                AreaPartialViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" },
                ViewLocationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml" },
                MasterLocationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml" },
                PartialViewLocationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml" },
                FileExtensions = new string[] { "cshtml" }
            });
            #endregion


            //Remove X-AspNetMvc-Version
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
