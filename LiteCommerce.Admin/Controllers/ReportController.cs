using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
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
    [Authorize(Roles = WebUserRoles.Accountant)]
    public class ReportController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string day = "")
        {
            List<TopProduct> data = ReportBLL.TopProduct(day);
            var model = new Models.ReportTopProduct(){
                Data = data
            };
            return View(model);
        }
    }
}