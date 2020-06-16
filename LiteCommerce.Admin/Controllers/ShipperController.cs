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
    public class ShipperController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string searchValue = "")
        {
            var model = new Models.ShipperResult
            {
                RowCount = CatalogBLL.Shipper_Count(searchValue),
                Data = CatalogBLL.Shipper_List(searchValue),
            };
            //var listOfSuppliers = CatalogBLL.Supplier_List(page, 10, searchValue);
            //int rowCount = CatalogBLL.Supplier_Count(searchValue);
            //ViewBag.rc = rowCount;
            return View(model);
        }
        /// <summary>
        /// Hiển thị form thêm/sửa Shipper
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add New Shipper";
                ViewBag.ConfirmButton = "Add";

                Shipper newShipper = new Shipper();
                newShipper.ShipperID = 0;
                return View(newShipper);
            }
            else
            {
                ViewBag.Title = "Edit Shipper";
                ViewBag.ConfirmButton = "Save";
                try
                {
                    Shipper editShipper = CatalogBLL.Shipper_Get(Convert.ToInt32(id));
                    if (editShipper == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(editShipper);
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
        public ActionResult Input(Shipper model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    if(model.ShipperID == 0)
                    {
                        ViewBag.Title = "Add New Shipper";
                        ViewBag.ConfirmButton = "Add";
                    }
                    else
                    {
                        ViewBag.Title = "Edit Shipper";
                        ViewBag.ConfirmButton = "Save";
                    }
                    return View(model);
                }
                //Đưa dữ liệu vào CSDL
                if (model.ShipperID == 0)
                {
                    int shipperID = CatalogBLL.Shipper_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool rs = CatalogBLL.Shipper_Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return View(model);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] shipperIDs)
        {
            if (shipperIDs != null)
            {
                CatalogBLL.Shipper_Delete(shipperIDs);
            }
            return RedirectToAction("Index");
        }
    }
}