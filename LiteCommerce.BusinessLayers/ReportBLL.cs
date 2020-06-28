using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public class ReportBLL
    {
        /// <summary>
        /// 
        /// </summary>
        private static IReportDAL ReportDB { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            ReportDB = new DataLayers.SqlServer.ReportDAL(connectionString);
        }
        public static List<TopProduct> TopProduct(string day){
            return ReportDB.TopProduct(day);
        }
    }
}
