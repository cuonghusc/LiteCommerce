using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = WebUserRoles.Accountant)]
    public class ProductController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "", string categoryId = "",string supplierId = "")
        {
            List<Product> data = new List<Product>();
            try
            {
                data = CatalogBLL.Product_List(page, AppSettings.DefaultPageSize, searchValue, categoryId, supplierId);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
            var model = new Models.ProductPaginationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = CatalogBLL.Product_Count(searchValue,categoryId,supplierId),
                Data = data,
                SearchValue = searchValue,
                CategoryID = categoryId,
                SupplierID = supplierId
            };
            return View(model);
        }
        /// <summary>
        /// Hiển thị form thêm/sửa Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add New Product";
                ViewBag.ConfirmButton = "Add";

                Product newProduct = new Product();
                newProduct.ProductID = 0;

                return View(newProduct);
            }
            else
            {
                ViewBag.Title = "Edit Product";
                ViewBag.ConfirmButton = "Save";
                try
                {
                    Product model = CatalogBLL.Product_Get(id);
                    return View(model);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index");
                }
                
            }            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input(Product model, HttpPostedFileBase uploadPhoto)
        {
            if (string.IsNullOrEmpty(model.Descriptions))
            {
                model.Descriptions = "";
            }
            //Upload ảnh
            if (uploadPhoto != null && uploadPhoto.ContentLength > 0)
            {
                string filePath = Path.Combine(Server.MapPath("~/Images"), uploadPhoto.FileName);
                uploadPhoto.SaveAs(filePath);
                model.PhotoPath = "/Images/" + uploadPhoto.FileName;
            }
            else if (model.PhotoPath == null)
            {
                model.PhotoPath = "";
            }
            //
            if (!ModelState.IsValid)
            {
                if (model.ProductID == 0)
                {
                    ViewBag.Title = "Add New Product";
                    ViewBag.ConfirmButton = "Add";
                    return View(model);
                }
                else
                {
                    ViewBag.Title = "Edit Product";
                    ViewBag.ConfirmButton = "Save";
                    return View(model);
                }               
            }
            //Đưa dữ liệu vào CSDL
            if (model.ProductID == 0)
            {
                int productId = CatalogBLL.Product_Add(model);
                return Redirect("~/Product/Attribute/" + productId);
            }
            else
            {
                bool rs = CatalogBLL.Product_Update(model);
                return RedirectToAction("Index");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            try
            {
                Product model = CatalogBLL.Product_Get(id);
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] productIDs = null)
        {
            if (productIDs != null)
                CatalogBLL.Product_Delete(productIDs);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Attribute(string id = "")
        {
            try
            {
                Product checkProduct = CatalogBLL.Product_Get(id);
                if(checkProduct==null)
                {
                    return RedirectToAction("Index");
                }
                var model = new Models.AttributeResult
                {
                    Data = CatalogBLL.Product_GetAttribute(id),
                    ProductID = Convert.ToInt32(id)
                };
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult ajaxAttribute(string id = "")
        {
            try
            {
                Product checkProduct = CatalogBLL.Product_Get(id);
                if (checkProduct == null)
                {
                    return RedirectToAction("Index");
                }
                var model = new Models.AttributeResult
                {
                    Data = CatalogBLL.Product_GetAttribute(id),
                };
                return Json(new { model.Data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AttributeName"></param>
        /// <param name="AttributeValues"></param>
        /// <param name="DisplayOrder"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Attribute(string[] AttributeName, string[] AttributeValues, string[] DisplayOrder, string ProductID)
        {
            //string[] newAttributeName;
            //string[] newAttributeValues;
            //string[] newDisplayOrder;
            List<string> newAttributeName = new List<string>();
            List<string> newAttributeValues = new List<string>();
            List<string> newDisplayOrder = new List<string>();
            for (int i = 0; i < AttributeName.Length; i++)
            {
                if(string.IsNullOrEmpty(AttributeName[i]) || string.IsNullOrEmpty(AttributeName[i]))
                {
                    continue;
                }
                else
                {
                    newAttributeName.Add(AttributeName[i]);
                    newAttributeValues.Add(AttributeValues[i]);
                    newDisplayOrder.Add(DisplayOrder[i]);
                }
            }
            string[] arrayAttributeName = newAttributeName.ToArray();
            string[] arrayAttributeValues = newAttributeValues.ToArray();
            string[] arrayDisplayOrder = newDisplayOrder.ToArray();

            bool rs = CatalogBLL.Product_UpdateAttribute(ProductID, arrayAttributeName, arrayAttributeValues, arrayDisplayOrder);

            return RedirectToAction("Detail", new { id = ProductID });
        }
    }
}