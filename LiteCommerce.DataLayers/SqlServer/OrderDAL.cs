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
    /// <summary>
    /// 
    /// </summary>
    public class OrderDAL : IOrderDAL
    {
        /// <summary>
        /// 
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public OrderDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Order data)
        {
            int orderId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Orders
                                    (
	                                    CustomerID,
	                                    ShipperID,
                                        OrderDate,
                                        RequiredDate,
                                        ShippedDate,
                                        Freight,
                                        ShipAddress,
                                        ShipCity,
                                        ShipCountry,
                                        EmployeeID
                                    )
                                    VALUES
                                    (
	                                    @CustomerID,	                                    
	                                    @ShipperID,
                                        @OrderDate,
                                        @RequiredDate,
                                        @ShippedDate,
                                        @Freight,
                                        @ShipAddress,
                                        @ShipCity,
                                        @ShipCountry,
                                        @EmployeeID
                                    );
                                    SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@OrderDate", data.OrderDate);
                cmd.Parameters.AddWithValue("@RequiredDate", data.RequiredDate);
                cmd.Parameters.AddWithValue("@ShippedDate", data.ShippedDate);
                cmd.Parameters.AddWithValue("@Freight", data.Freight);
                cmd.Parameters.AddWithValue("@ShipAddress", data.ShipAddress);
                cmd.Parameters.AddWithValue("@ShipCity", data.ShipCity);
                cmd.Parameters.AddWithValue("@ShipCountry", data.ShipCountry);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                orderId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return orderId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderIDs"></param>
        /// <returns></returns>
        public bool Delete(string[] orderIDs)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Orders
                                            WHERE(OrderID = @orderId)
                                              AND(OrderID NOT IN(SELECT OrderID FROM OrderDetails))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@orderId", SqlDbType.Int);
                foreach (string orderId in orderIDs)
                {
                    cmd.Parameters["@shipperId"].Value = Convert.ToInt32(orderId);
                    result += cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            return result > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public List<OrderDetails> GetDetail(string orderID)
        {
            List<OrderDetails> data = new List<OrderDetails>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT a.OrderID OrderID,
		                            a.ProductID ProductID,
		                            b.ProductName ProductName,
		                            a.UnitPrice UnitPrice,
		                            a.Quantity Quantity,
		                            a.Discount Discount,
		                            ROUND((a.UnitPrice*a.Quantity - a.UnitPrice*a.Quantity*a.Discount),1) Amount,
		                            c.Freight Freight
                            FROM dbo.OrderDetails a
                            JOIN dbo.Products b ON a.ProductID = b.ProductID
                            JOIN dbo.Orders c ON a.OrderID = c.OrderID
                            WHERE a.OrderID = @orderId";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@orderId", orderID);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new OrderDetails()
                            {
                                OrderID = Convert.ToInt32(dbReader["OrderID"]),
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                ProductName = Convert.ToString(dbReader["ProductName"]),
                                UnitPrice = Convert.ToDouble(dbReader["UnitPrice"]),
                                Quantity = Convert.ToInt32(dbReader["Quantity"]),
                                Discount = Convert.ToDouble(dbReader["Discount"]),
                                Amount = Convert.ToDouble(dbReader["Amount"]),
                                Freight = Convert.ToDouble(dbReader["Freight"]),
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public List<Order> List(int page, int pageSize, string searchValue,string shipperId, string customerId, string country)
        {
            List<Order> data = new List<Order>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT * FROM (
		                SELECT DISTINCT 
			                a.OrderID OrderID,
			                t.PriceProduct PriceProduct,
			                a.CustomerID CustomerID,
			                a.OrderDate OrderDate,
			                a.RequiredDate RequiredDate,
			                a.ShippedDate ShippedDate,
			                a.ShipperID ShipperID,
			                b.CompanyName ShipperName,
			                c.CompanyName CustomerName,
			                d.EmployeeID EmployeeID,
			                a.Freight Freight,
			                a.ShipAddress ShipAddress,
			                a.ShipCity ShipCity,
			                a.ShipCountry ShipCountry,
			                (d.FirstName + ' ' + d.LastName) EmployeeName,
			                ROW_NUMBER() OVER(ORDER BY a.OrderID) AS RowNumber
		                    FROM dbo.Orders AS a
		                JOIN (
			                SELECT	OrderID,
					                ROUND(SUM((UnitPrice*Quantity)-UnitPrice*Quantity*Discount),1) AS PriceProduct
			                FROM dbo.OrderDetails
			                GROUP BY OrderID) AS t ON t.OrderID = a.OrderID
		                JOIN dbo.Shippers AS b ON a.ShipperID = b.ShipperID
		                JOIN dbo.Customers AS c ON a.CustomerID = c.CustomerID
		                JOIN dbo.Employees AS d ON a.EmployeeID = d.EmployeeID
		                WHERE ((@searchValue = N'') OR (c.CompanyName like @searchValue))
                        AND ((@CustomerID = N'') OR (@CustomerID = c.CustomerID))
                        AND ((@ShipperID = N'') OR (@ShipperID = b.ShipperID))
                        AND ((@Country = N'') OR (@Country = a.ShipCountry))
	                ) AS k 
	                    WHERE k.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize
                    ORDER BY k.RowNumber";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    cmd.Parameters.AddWithValue("@ShipperID", shipperId);
                    cmd.Parameters.AddWithValue("@Country", country);
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Order()
                            {
                                OrderID = Convert.ToInt32(dbReader["OrderID"]),
                                CustomerID = Convert.ToString(dbReader["CustomerID"]),
                                CustomerName = Convert.ToString(dbReader["CustomerName"]),
                                EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                                OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                                RequiredDate = Convert.ToDateTime(dbReader["RequiredDate"]),
                                ShippedDate = Convert.ToDateTime(dbReader["ShippedDate"]),
                                ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                                ShipperName = Convert.ToString(dbReader["ShipperName"]),
                                Freight = Convert.ToDouble(dbReader["Freight"]),
                                PriceProduct = Convert.ToDouble(dbReader["PriceProduct"]),
                                PaymentTotal = Convert.ToDouble(dbReader["PriceProduct"]) + Convert.ToDouble(dbReader["Freight"]),
                                ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                                ShipCity = Convert.ToString(dbReader["ShipCity"]),
                                ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                                OrderDetails = null,
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Order data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Orders
                                           SET  CustomerID = @CustomerID, 
                                                EmployeeID = @EmployeeID,
                                                OrderDate = @OrderDate,
                                                RequiredDate = @RequiredDate,
                                                ShippedDate = @ShippedDate,
                                                ShipperID = @ShipperID,
                                                Freight = @Freight,
                                                ShipAddress = @ShipAddress,
                                                ShipCity = @ShipCity,
                                                ShipCountry = @ShipCountry
                                          WHERE OrderID = @OrderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@OrderID", data.OrderID);
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@OrderDate", data.OrderDate);
                cmd.Parameters.AddWithValue("@RequiredDate", data.RequiredDate);
                cmd.Parameters.AddWithValue("@ShippedDate", data.ShippedDate);
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@Freight", data.Freight);
                cmd.Parameters.AddWithValue("@ShipAddress", data.ShipAddress);
                cmd.Parameters.AddWithValue("@ShipCity", data.ShipCity);
                cmd.Parameters.AddWithValue("@ShipCountry", data.ShipCountry);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue, string shipperId, string customerId, string country)
        {
            int dem;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT count(*)
                                        FROM dbo.Orders a
		                                JOIN dbo.Customers b ON a.CustomerID = b.CustomerID 
                                        JOIN dbo.Shippers c ON a.ShipperID = c.ShipperID
                                        
                                        WHERE ((@searchValue = N'') OR (b.CompanyName like @searchValue))
                                            AND ((@CustomerID = N'') OR (@CustomerID = b.CustomerID))
                                            AND ((@ShipperID = N'') OR (@ShipperID = c.ShipperID))
                                            AND ((@Country = N'') OR (@Country = a.ShipCountry))";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    cmd.Parameters.AddWithValue("@ShipperID", shipperId);
                    cmd.Parameters.AddWithValue("@Country", country);
                    dem = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return dem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order Get(string orderID)
        {
            Order data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Orders WHERE OrderID = @orderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@orderID", orderID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Order()
                        {
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                            RequiredDate = Convert.ToDateTime(dbReader["RequiredDate"]),
                            ShippedDate = Convert.ToDateTime(dbReader["ShippedDate"]),
                            Freight = Convert.ToDouble(dbReader["Freight"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                            ShipCity = Convert.ToString(dbReader["ShipCity"]),
                            ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_Product(OrderDetails model)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO OrderDetails
                                    (
	                                    OrderID,
                                        ProductID,
                                        UnitPrice,
                                        Quantity,
                                        Discount

                                    )
                                    VALUES
                                    (
	                                    @OrderID,
                                        @ProductID,
                                        @UnitPrice,
                                        @Quantity,
                                        @Discount
                                    );";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", model.ProductID);
                cmd.Parameters.AddWithValue("@UnitPrice", model.UnitPrice);
                cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                cmd.Parameters.AddWithValue("@Discount", (model.Discount)/100);
                cmd.Parameters.AddWithValue("@OrderID", model.OrderID);
                cmd.ExecuteScalar();
                connection.Close();
            }
            return Convert.ToString(model.OrderID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public bool Delete_Product(string orderID, string productID)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM OrderDetails
                                            WHERE(OrderID = @orderID)
                                              AND(ProductID = @productID)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@productID", productID);
                result = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return result > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public bool CheckOrder(string orderID,string productID)
        {
            OrderDetails data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM OrderDetails WHERE OrderID = @orderID AND ProductID = @productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@productID", productID);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new OrderDetails()
                        {
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            ProductID = Convert.ToInt32(dbReader["ProductID"])
                        };
                    }
                }
                connection.Close();
            }
            return (data!=null);
        }

        public string UpdateProduct(OrderDetails orderDetails, string method ="add")
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                if (method == "add")
                {
                    cmd.CommandText = @"UPDATE OrderDetails
                                           SET UnitPrice = @UnitPrice 
                                              ,Quantity = Quantity + @Quantity
                                                ,Discount = @Discount
                                          WHERE OrderID = @OrderID AND ProductID = @ProductID";
                }else
                {
                    cmd.CommandText = @"UPDATE OrderDetails
                                           SET  Quantity = @Quantity
                                                ,Discount = @Discount
                                          WHERE OrderID = @OrderID AND ProductID = @ProductID";
                }
                
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@UnitPrice", orderDetails.UnitPrice);
                cmd.Parameters.AddWithValue("@Quantity", orderDetails.Quantity);
                cmd.Parameters.AddWithValue("@Discount", orderDetails.Discount/100);
                cmd.Parameters.AddWithValue("@OrderID", orderDetails.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", orderDetails.ProductID);
                cmd.ExecuteNonQuery();

                connection.Close();
            }

            return Convert.ToString(orderDetails.OrderID);
        }
    }
}
