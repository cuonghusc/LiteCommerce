using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace LiteCommerce.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BusinessLayerConfig.Initialize();
        }
        /// <summary>
        ///
        /// </summary>
        protected void Application_AuthenticateRequest()
        {
            if (IgnoreAuthenticateRequest())
                return;

            try
            {
                WebUserPrincipal principal;
                if (User != null && User.Identity.IsAuthenticated && String.Compare(User.Identity.AuthenticationType, "Forms", false) == 0)
                {
                    WebUserData userData = WebUserData.FromCookieString(User.Identity.Name);
                    if (userData != null)
                    {
                        principal = new WebUserPrincipal(User.Identity, userData);
                    }
                    else
                    {
                        principal = WebUserPrincipal.Anonymous;
                    }
                    HttpContext.Current.User = Thread.CurrentPrincipal = principal;
                }
                else
                {
                    HttpContext.Current.User = Thread.CurrentPrincipal = WebUserPrincipal.Anonymous;
                }
            }
            catch
            {
                FormsAuthentication.SignOut();
                HttpContext.Current.User = Thread.CurrentPrincipal = WebUserPrincipal.Anonymous;
            }
        }

        /// <summary>
        /// Phần mở rộng của những file không cần authenticate
        /// </summary>
        private static readonly string[] IgnoredExtensions = new[]
        {
            ".js", ".css", ".txt", ".html", ".htm", ".xml", ".png", ".gif", ".jpg", ".ico", ".zip" , ".woff2"
        };
        /// <summary>
        /// Check xem có cần authenticate không?
        /// </summary>
        /// <returns></returns>
        private static bool IgnoreAuthenticateRequest()
        {
            var url = HttpContext.Current.Request.Url.AbsolutePath.ToLowerInvariant();
            return IgnoredExtensions.Any(url.EndsWith);
        }
    }
}
