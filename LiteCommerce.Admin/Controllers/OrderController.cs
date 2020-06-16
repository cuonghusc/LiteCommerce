using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = WebUserRoles.Saleman)]
    public class OrderController : Controller
    {
        /// <summary>
        /// Danh sách đơn hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }
    }
}