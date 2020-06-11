using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace LiteCommerce.Admin
{
    public static class WebUserExtensions
    {
        /// <summary>
        /// Lấy thông tin liên quan đến phiên đăng nhập của tài khoản
        /// </summary>
        /// <param name="userPrincipal"></param>
        /// <returns></returns>
        public static WebUserData GetUserData(this IPrincipal userPrincipal)
        {
            try
            {
                if (!userPrincipal.Identity.IsAuthenticated)
                    return null;

                WebUserPrincipal principal = userPrincipal as WebUserPrincipal;
                if (principal == null)
                {
                    return null;
                }
                else
                {
                    return principal.UserData;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}