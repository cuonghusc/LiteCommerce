using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public class OrderBLL
    {
        /// <summary>
        /// 
        /// </summary>
        private static IOrderDAL OrderDB { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            OrderDB = new DataLayers.SqlServer.OrderDAL(connectionString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="shipperId"></param>
        /// <param name="customerId"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static List<Order> Order_List(int page, int pageSize, string searchValue, string shipperId, string customerId, string country)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 1)
                pageSize = 1;
            return OrderDB.List(page, pageSize, searchValue, shipperId, customerId, country);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="shipperId"></param>
        /// <param name="customerId"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static int Order_Count(string searchValue, string shipperId, string customerId, string country)
        {
            return OrderDB.Count(searchValue, shipperId, customerId, country);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<OrderDetails> OrderDetail_Get(string id)
        {
            return OrderDB.GetDetail(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Order Order_Get(string id)
        {
            return OrderDB.Get(id);
        }

        public static int Order_Add(Order model)
        {
            return OrderDB.Add(model);
        }

        public static bool Order_Update(Order model)
        {
            return OrderDB.Update(model);
        }

        public static string Order_Add_Product(OrderDetails model)
        {
            return OrderDB.Add_Product(model);
        }

        public static bool Delete_Product(string orderID, string productID)
        {
            return OrderDB.Delete_Product(orderID, productID);
        }

        public static bool Delete(string[] orderId)
        {
            return OrderDB.Delete(orderId);
        }

        public static bool CheckOrder(string orderId, string productId)
        {
            return OrderDB.CheckOrder(orderId, productId);
        }

        public static string Order_Update_Product(OrderDetails orderDetails,string method)
        {
            return OrderDB.UpdateProduct(orderDetails,method);
        }
    }
}
