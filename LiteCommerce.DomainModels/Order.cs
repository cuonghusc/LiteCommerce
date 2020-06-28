using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// Thông tin về đơn hàng
    /// </summary>
    public class Order
    {
        private double paymentTotal;

        /// <summary>
        /// 
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string CustomerID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int EmployeeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RequiredDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ShippedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int ShipperID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipperName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Freight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double PriceProduct { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double PaymentTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ShipAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ShipCity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ShipCountry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OrderDetails> OrderDetails { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class OrderDetails
    {
        /// <summary>
        /// 
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double UnitPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Discount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Freight { get; set; }
    }
}
