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
    [Authorize(Roles = WebUserRoles.Saleman)]
    public class SupplierController : Controller
    {
        /// <summary>
        /// Trang hiển thị : danh sách suppliers, các "liên kết đến" các chức năng liên quan
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "")
        {          
            var model = new Models.SupplierPaginationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = CatalogBLL.Supplier_Count(searchValue),
                Data = CatalogBLL.Supplier_List(page, AppSettings.DefaultPageSize, searchValue),
                SearchValue = searchValue,
            };
            return View(model);

        }
        /// <summary>
        /// Add or Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add New Supplier";
                ViewBag.ConfirmButton = "Add";

                Supplier newSupplier = new Supplier();
                newSupplier.SupplierID = 0;
                return View(newSupplier);
            }
            else
            {
                ViewBag.Title = "Edit Supplier";
                ViewBag.ConfirmButton = "Save";
                try
                {
                    Supplier editSupplier = CatalogBLL.Supplier_Get(Convert.ToInt32(id));
                    if (editSupplier == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(editSupplier);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return RedirectToAction("Index");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input(Supplier model)
        {
            try
            {
                //Validation dữ liệu                
                if (string.IsNullOrEmpty(model.Fax))
                {                    
                    model.Fax = "";
                }
                if (string.IsNullOrEmpty(model.HomePage))
                {                    
                    model.HomePage = "";
                }
                //Kiểm tra có tồn tại bất kỳ lỗi nào hay không
                if (!ModelState.IsValid)
                {
                    if (model.SupplierID == 0)
                    {
                        ViewBag.Title = "Add New Supplier";
                        ViewBag.ConfirmButton = "Add";                        
                    }
                    else
                    {
                        ViewBag.Title = "Edit New Supplier";
                        ViewBag.ConfirmButton = "Save";
                    }
                    return View(model);
                }
                //Đưa dữ liệu vào CSDL
                if (model.SupplierID == 0)
                {
                    int supplierID = CatalogBLL.Supplier_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool rs = CatalogBLL.Supplier_Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                //ModelState.AddModelError("", e.Message + ":" + e.StackTrace);
                return View(model);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string method = "", int[] supplierIDs = null)
        {
            if (supplierIDs!=null)
            {
                CatalogBLL.Supplier_Delete(supplierIDs);
            }
            return RedirectToAction("Index");
        }
    }
}