using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOrderDAL
    {
        /// <summary>
        /// Bổ sung thêm một Order
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Order data);
        /// <summary>
        /// Cập nhật một Order
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Order data);
        /// <summary>
        /// Xóa một hoặc nhiều Order(s)
        /// </summary>
        /// <param name="OrderIDs"></param>
        /// <returns></returns>
        bool Delete(string[] orderIDs);
        /// <summary>
        /// Lấy thông tin của một Order
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        List<OrderDetails> GetDetail(string orderID);
        /// <summary>
        /// Danh sách kết quả tìm kiếm có phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Order> List(int page, int pageSize, string searchValue,string shipperId, string customerId, string country);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue, string shipperId, string customerId, string country);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Order Get(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_Product(OrderDetails model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        bool Delete_Product(string orderID, string productID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool CheckOrder(string orderId, string productId);
        string UpdateProduct(OrderDetails orderDetails,string method);
    }
}
