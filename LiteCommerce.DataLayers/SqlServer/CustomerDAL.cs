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
    public class CustomerDAL : ICustomerDAL
    {
        /// <summary>
        /// 
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public CustomerDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Customer data)
        {
            int customerId;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Customers
                                    (
                                        CustomerID,
	                                    CompanyName,
	                                    ContactName,
	                                    ContactTitle,
	                                    Address,
	                                    City,
	                                    Country,
	                                    Phone,
	                                    Fax
                                    )
                                    VALUES
                                    (
                                        @CustomerID,
	                                    @CompanyName,
	                                    @ContactName,
	                                    @ContactTitle,
	                                    @Address,
	                                    @City,
	                                    @Country,
	                                    @Phone,
	                                    @Fax
                                    );
                                    SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("@ContactTitle", data.ContactTitle);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                cmd.Parameters.AddWithValue("@Fax", data.Fax);

                //customerId = Convert.ToInt32(cmd.ExecuteScalar());
                var queryResult = cmd.ExecuteScalar();
                customerId = (queryResult != DBNull.Value) ? Convert.ToInt32(cmd.ExecuteScalar()) : 0;
                connection.Close();
            }
            
            return customerId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sustomerIDs"></param>
        /// <returns></returns>
        public bool Delete(string[] customerIDs)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Customers
                                            WHERE(CustomerID = @customerId)
                                              AND(CustomerID NOT IN(SELECT CustomerID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@customerId", SqlDbType.Int);
                foreach (string customerID in customerIDs)
                {
                    cmd.Parameters["@customerId"].Value = customerID;
                    result += cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            return result > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Customer Get(string customerID)
        {
            Customer data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Customers WHERE CustomerID = @customerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@customerID", customerID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Customer()
                        {
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            ContactName = Convert.ToString(dbReader["ContactName"]),
                            ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Phone = Convert.ToString(dbReader["Phone"]),
                            Fax = Convert.ToString(dbReader["Fax"]),
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
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public List<Customer> List(int page, int pageSize, string searchValue)
        {
            List<Customer> data = new List<Customer>();
            if(!string.IsNullOrEmpty(searchValue))
                searchValue = "%"+searchValue+"%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT *
                                        FROM(
                                            SELECT *, ROW_NUMBER() OVER(ORDER BY CustomerID) AS RowNumber
                                            FROM Customers
                                            WHERE(@searchValue = N'') OR (CompanyName like @searchValue)
                                        ) AS t WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize
                                        ORDER BY t.RowNumber";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    using(SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Customer()
                            {
                                CustomerID = Convert.ToString(dbReader["CustomerID"]),
                                CompanyName = Convert.ToString(dbReader["CompanyName"]),
                                ContactName = Convert.ToString(dbReader["ContactName"]),
                                ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                                Address = Convert.ToString(dbReader["Address"]),
                                City = Convert.ToString(dbReader["City"]),
                                Country = Convert.ToString(dbReader["Country"]),
                                Phone = Convert.ToString(dbReader["Phone"]),
                                Fax = Convert.ToString(dbReader["Fax"]),                         
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
        public bool Update(Customer data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Customers
                                    SET                                    
                                        CompanyName = @CompanyName,
                                        ContactName = @ContactName,
                                        ContactTitle = @ContactTitle,
                                        Address = @Address,
                                        City = @City,
                                        Country = @Country,
                                        Phone = @Phone,
                                        Fax = @Fax
                                    WHERE CustomerID = @CustomerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("@ContactTitle", data.ContactTitle);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                cmd.Parameters.AddWithValue("@Fax", data.Fax);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return rowsAffected > 0;
        }
        /// <summary>
        /// Đếm số Customer
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue)
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
                                        FROM Customers
                                        WHERE(@searchValue = N'') OR (CompanyName like @searchValue)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);

                    dem = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return dem;
        }

        public List<Customer> GetAll()
        {
            List<Customer> data = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT CustomerID, CompanyName FROM Customers";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Customer()
                            {
                                CustomerID = Convert.ToString(dbReader["CustomerID"]),
                                CompanyName = Convert.ToString(dbReader["CompanyName"]),                               
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
