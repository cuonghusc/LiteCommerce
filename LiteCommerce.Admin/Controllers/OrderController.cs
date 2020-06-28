using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                        ViewBag.Title = "Add New Order";
                        ViewBag.ConfirmButton = "Add";
                        return View(model);
                    }
                    else
                    {
                        ViewBag.Title = "Edit Order";
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
                    List<OrderDetails> data = OrderBLL.OrderDetail_Get(id);
                    double dem = 0;
                    foreach(var item in data)
                    {
                        dem += (item.Quantity*item.UnitPrice - item.Quantity*item.UnitPrice*item.Discount);
                    }
                    string total = dem.ToString("N1", CultureInfo.InvariantCulture);
                    var model = new Models.OrderDetailsResult
                    {
                        Data = data,
                        OrderID = id,
                        Total = total
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
            if (string.IsNullOrEmpty(orderId))
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(productId) || quantity < 0)
            {
                return Redirect("~/Order/OrderDetails/"+ orderId);
            }
            if (quantity < 1 || discount < 0 || discount > 100)
            {
                return Redirect("~/Order/OrderDetails/" + orderId);
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
                bool check = OrderBLL.CheckOrder(orderId, productId);
                string rs = "";
                if (check)
                {
                    rs = OrderBLL.Order_Update_Product(orderDetails,"add");
                }
                else
                {
                    rs = OrderBLL.Order_Add_Product(orderDetails);
                }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateProduct(string orderId, string productId, int quantity,int discount)
        {
            if(string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(productId))
            {
                return RedirectToAction("Index");
            }
            if (quantity < 1 || discount < 0 || discount > 100)
            {
                return Redirect("~/Order/OrderDetails/" + orderId);
            }
            else
            {
                OrderDetails order = new OrderDetails();
                order.OrderID = Convert.ToInt32(orderId);
                order.ProductID = Convert.ToInt32(productId);
                order.Quantity = Convert.ToInt32(quantity);
                order.Discount = Convert.ToInt32(discount);

                try
                {
                    string rs = OrderBLL.Order_Update_Product(order,"update");
                    return Redirect("~/Order/OrderDetails/" + rs);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");
                }
            }
        }
    }
}