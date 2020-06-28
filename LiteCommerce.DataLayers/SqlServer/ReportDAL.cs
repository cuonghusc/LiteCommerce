using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;
using System.Data;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class ReportDAL : IReportDAL
    {
        /// <summary>
        /// 
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public ReportDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<TopProduct> TopProduct(string day)
        {
            List<TopProduct> data = new List<TopProduct>();
            if (!string.IsNullOrEmpty(day))
                day = "%" + day + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT TOP 10 a.ProductID,a.ProductName,b.Quantity FROM dbo.Products a JOIN (SELECT ProductID,COUNT(Quantity) Quantity FROM dbo.OrderDetails GROUP BY ProductID) AS b
ON b.ProductID = a.ProductID ORDER BY b.Quantity DESC";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@day", day);
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new TopProduct()
                            {
                                ProductID = Convert.ToString(dbReader["ProductID"]),
                                ProductName = Convert.ToString(dbReader["ProductName"]),
                                Quantity = Convert.ToDouble(dbReader["Quantity"]),

                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }
    }
}
