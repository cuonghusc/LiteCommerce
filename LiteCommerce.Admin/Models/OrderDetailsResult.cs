using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public class OrderDetailsResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<OrderDetails> Data;
        /// <summary>
        /// 
        /// </summary>
        public string OrderID;
        /// <summary>
        /// 
        /// </summary>
        public string Total;
    }
}