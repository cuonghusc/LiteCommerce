using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    public class Report
    {
        public List<TopProduct> TopProduct;
    }
    public class TopProduct
    {
        /// <summary>
        /// 
        /// </summary>
        public string ProductID;
        /// <summary>
        /// 
        /// </summary>
        public string ProductName;
        /// <summary>
        /// 
        /// </summary>
        public double Quantity;
    }
}
