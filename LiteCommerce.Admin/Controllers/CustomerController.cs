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
    public class CustomerController : Controller
    {
        /// <summary>
        /// Hiển thị danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            var model = new Models.CustomerPaginationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = CatalogBLL.Customer_Count(searchValue),
                Data = CatalogBLL.Customer_List(page, AppSettings.DefaultPageSize, searchValue),
            };
            return View(model);
        }
        /// <summary>
        /// Hiển thị form thêm/sửa thông tin Khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add New Customer";
                ViewBag.ConfirmButton = "Add";
                ViewBag.Method = "add";

                Customer newCustomer = new Customer();
                newCustomer.CustomerID = "";
                return View(newCustomer);
            }
            else
            {
                ViewBag.Title = "Edit Customer";
                ViewBag.ConfirmButton = "Save";
                ViewBag.Method = "update";
                try
                {
                    Customer editCustomer = CatalogBLL.Customer_Get(id);
                    if (editCustomer == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(editCustomer);
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input(Customer model,string method,string tempID)
        {
            
            if (string.IsNullOrEmpty(method))
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (method == "add")
                {
                    if (!string.IsNullOrEmpty(model.CustomerID))
                    {
                        if (CatalogBLL.Customer_Get(model.CustomerID) != null)
                        {
                            ModelState.AddModelError("CustomerID", "Customer ID ready exist");
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(model.Fax))
            {
                model.Fax = "";
            }
            //Nếu không có trường method thì chuyển hướng về Index
            if (string.IsNullOrEmpty(method))
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (string.IsNullOrEmpty(model.Fax))
                {
                    model.Fax = "";
                }

                //Kiểm tra có tồn tại bất kỳ lỗi nào hay không
                if (!ModelState.IsValid)
                {
                    if (method == "add")
                    {
                        ViewBag.Title = "Add New Customer";
                        ViewBag.ConfirmButton = "Add";
                        ViewBag.Method = "add";
                        return View(model);
                    }
                    else
                    {
                        ViewBag.Title = "Edit Customer";
                        ViewBag.ConfirmButton = "Save";
                        ViewBag.Method = "update";
                        return View(model);
                    }

                }
                //Đưa dữ liệu vào CSDL
                if (method == "add")
                {
                    int customerID = CatalogBLL.Customer_Add(model);
                }
                else if (method == "update")
                {
                    bool rs = CatalogBLL.Customer_Update(model);
                }
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string[] customerIDs)
        {
            if (customerIDs != null)
                CatalogBLL.Customer_Delete(customerIDs);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult ajaxGet(string id = "")
        {
            var data =  CatalogBLL.Customer_Get(id);
            return Json(data);
        }
    }
}