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
    public class OrderController : Controller
    {
        /// <summary>
        /// Danh sách đơn hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "", string shipperId = "", string customerId = "", string country = "")
        {
            List<Order> data = new List<Order>();
            try
            {
                data = OrderBLL.Order_List(page, AppSettings.DefaultPageSize, searchValue, shipperId, customerId, country);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
            var model = new Models.OrderPaginationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = OrderBLL.Order_Count(searchValue, shipperId, customerId, country),
                Data = data,
                SearchValue = searchValue,
                CustomerID = customerId,
                Country = country,
                ShipperID = shipperId,
            };
            return View(model);
        }
        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ajaxOrderDetail(string id)
        {
            List<OrderDetails> data = new List<OrderDetails>();
            data = OrderBLL.OrderDetail_Get(id);
            return Json(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add New Order";
                ViewBag.ConfirmButton = "Add";

                Order newOrder = new Order();
                newOrder.OrderID = 0;

                return View(newOrder);
            }
            else
            {
                ViewBag.Title = "Edit Order";
                ViewBag.ConfirmButton = "Save";
                try
                {
                    Order model = OrderBLL.Order_Get(id);
                    return View(model);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index");
                }

            }
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input(Order model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    if (model.OrderID == 0)
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
                WebUserData userData = User.GetUserData();
                model.EmployeeID = Convert.ToInt32(userData.UserID);
                //Đưa dữ liệu vào CSDL
                if (model.OrderID == 0)
                {
                    int orderID = OrderBLL.Order_Add(model);
                    return Redirect("~/Order/OrderDetails/"+ orderID);
                }
                else
                {
                    bool rs = OrderBLL.Order_Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message + ":" + e.StackTrace);
                return View(model);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OrderDetails(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }else
            {
                try
                {
                    var model = new Models.OrderDetailsResult
                    {
                        Data = OrderBLL.OrderDetail_Get(id),
                        OrderID = id
                    };
                    return View(model);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProduct(string orderId,string productId,int quantity = 0,double discount = 0)
        {
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(productId))
            {
                return RedirectToAction("Index");
            }
            else
            {
                double unitprice = CatalogBLL.Product_Get(productId).UnitPrice;
                
                OrderDetails orderDetails = new OrderDetails();
                orderDetails.OrderID = Convert.ToInt32(orderId);
                orderDetails.ProductID = Convert.ToInt32(productId);
                orderDetails.Quantity = quantity;
                orderDetails.Discount = discount;
                orderDetails.UnitPrice = unitprice;
                string rs = OrderBLL.Order_Add_Product(orderDetails);
                return Redirect("~/Order/OrderDetails/" + rs);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public ActionResult DeleteProduct(string OrderID,string ProductID)
        {
            bool rs = OrderBLL.Delete_Product(OrderID, ProductID);
            return Redirect("~/Order/OrderDetails/" + OrderID);
        }
        [HttpPost]
        public ActionResult Delete(string[] orderId)
        {
            if(orderId == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                bool rs = OrderBLL.Delete(orderId);
                return RedirectToAction("Index");
            }
        }
    }
}