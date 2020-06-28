using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public class OrderPaginationResult:PaginationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Order> Data;
        /// <summary>
        /// 
        /// </summary>
        public string CustomerID;
        /// <summary>
        /// 
        /// </summary>
        public string Country;
        /// <summary>
        /// 
        /// </summary>
        public string ShipperID;
    }
}