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
    public class ProductDAL : IProductDAL
    {
        /// <summary>
        /// 
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Product data)
        {
            int productId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Products
                                          (
                                                ProductName,
                                                SupplierID,
                                                CategoryID,
                                                QuantityPerUnit,
                                                UnitPrice,
                                                Descriptions,
                                                PhotoPath
                                          )
                                          VALUES
                                          (
                                                @ProductName,
                                                @SupplierID,
                                                @CategoryID,
                                                @QuantityPerUnit,
                                                @UnitPrice,
                                                @Descriptions,
                                                @PhotoPath
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);

                productId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return productId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productIDs"></param>
        /// <returns></returns>
        public bool Delete(int[] productIDs)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Products
                                            WHERE(ProductID = @productID)
                                              AND(ProductID NOT IN(SELECT ProductID FROM OrderDetails))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@productID", SqlDbType.Int);
                foreach (int productId in productIDs)
                {
                    cmd.Parameters["@productID"].Value = productId;
                    result += cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            return result > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public Product Get(string productID)
        {
                Product data = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"SELECT	a.ProductID ProductID,a.ProductName ProductName,
                                            a.QuantityPerUnit QuantityPerUnit,a.UnitPrice UnitPrice,
                                            a.Descriptions Descriptions,a.PhotoPath PhotoPath,
		                                    a.SupplierID SupplierID, c.CompanyName CompanyName,
		                                    a.CategoryID CategoryID, b.CategoryName CategoryName
                                    FROM dbo.Products AS a
	                                JOIN dbo.Categories AS b ON a.CategoryID = b.CategoryID
	                                JOIN dbo.Suppliers AS c ON c.SupplierID = a.SupplierID
                                    WHERE a.ProductID = @productID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@productID", productID);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (dbReader.Read())
                        {
                            data = new Product()
                            {
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                ProductName = Convert.ToString(dbReader["ProductName"]),
                                SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                                CompanyName = Convert.ToString(dbReader["CompanyName"]),
                                CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                                CategoryName = Convert.ToString(dbReader["CategoryName"]),
                                QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                                UnitPrice = Convert.ToDouble(dbReader["UnitPrice"]),
                                Descriptions = Convert.ToString(dbReader["Descriptions"]),
                                PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                                ProductAttributes = null,
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
        public List<Product> List(int page, int pageSize, string searchValue,string categoryID,string supplierID)
        {
            List<Product> data = new List<Product>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT *
                                        FROM(
                                            SELECT	a.ProductID ProductID,a.ProductName ProductName,
                                                    a.QuantityPerUnit QuantityPerUnit,a.UnitPrice UnitPrice,
                                                    a.Descriptions Descriptions,a.PhotoPath PhotoPath,
                                                    a.SupplierID SupplierID, c.CompanyName CompanyName,
                                                    a.CategoryID CategoryID, b.CategoryName CategoryName,
				                                     ROW_NUMBER() OVER(ORDER BY a.ProductID) AS RowNumber
                                            FROM dbo.Products AS a
	                                        JOIN dbo.Categories AS b ON a.CategoryID = b.CategoryID
	                                        JOIN dbo.Suppliers AS c ON c.SupplierID = a.SupplierID
                                            WHERE ((@supplierID = N'') OR (a.SupplierID = @supplierID)) AND ((@categoryId = N'') OR (a.CategoryID=@categoryId)) AND ((@searchValue = N'') OR (ProductName like @searchValue))
                                        ) AS t WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize
                                        ORDER BY t.RowNumber";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@supplierID", supplierID);
                    cmd.Parameters.AddWithValue("@categoryId", categoryID);
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Product()
                            {
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                ProductName = Convert.ToString(dbReader["ProductName"]),
                                CompanyName = Convert.ToString(dbReader["CompanyName"]),
                                CategoryName = Convert.ToString(dbReader["CategoryName"]),
                                QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                                UnitPrice = Convert.ToDouble(dbReader["UnitPrice"]),
                                Descriptions = Convert.ToString(dbReader["Descriptions"]),
                                PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
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
        public bool Update(Product data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Products
                                    SET   
                                        ProductName = @ProductName,
                                        SupplierID = @SupplierID,
                                        CategoryID = @CategoryID,
                                        QuantityPerUnit = @QuantityPerUnit,
                                        UnitPrice = @UnitPrice,
                                        Descriptions = @Descriptions,
                                        PhotoPath = @PhotoPath
                                    WHERE ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return rowsAffected > 0;
        }
        /// <summary>
        /// Đếm số products
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue,string categoryID,string supplierID)
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
                                        FROM Products
                                        WHERE ((@supplierID = N'') OR (SupplierID = @supplierID)) AND ((@categoryId = N'') OR (CategoryID=@categoryId)) AND ((@searchValue = N'') OR (ProductName like @searchValue))";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@supplierID", supplierID);
                    cmd.Parameters.AddWithValue("@categoryId", categoryID);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    dem = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return dem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public List<ProductAttributes> GetAttribute(string productID){
            List<ProductAttributes> data = new List<ProductAttributes>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT * FROM ProductAttributes WHERE ProductID = @productID ORDER BY DisplayOrder";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@productID", Convert.ToInt32(productID));
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new ProductAttributes()
                            {
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                                AttributeName = Convert.ToString(dbReader["AttributeName"]),
                                AttributeValues = Convert.ToString(dbReader["AttributeValues"]),
                                DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool UpdateAttribute(string productID, string[] attributeName, string[] attributeValues, string[] displayOrder)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM ProductAttributes WHERE ProductID = @productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@productID", SqlDbType.Int).Value = Convert.ToInt32(productID);
                cmd.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = @"INSERT INTO ProductAttributes (ProductID,AttributeName,AttributeValues,DisplayOrder) VALUES (@ProductID,@AttributeName,@AttributeValues,@DisplayOrder)";
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = connection;
                cmd2.Parameters.AddWithValue("@ProductID", productID);
                cmd2.Parameters.Add("@AttributeName", SqlDbType.NVarChar);
                cmd2.Parameters.Add("@AttributeValues", SqlDbType.NVarChar);
                cmd2.Parameters.Add("@DisplayOrder", SqlDbType.Int);
                for (int i = 0;i<attributeName.Length;i++)
                {
                    cmd2.Parameters["@AttributeName"].Value = attributeName[i];
                    cmd2.Parameters["@AttributeValues"].Value = attributeValues[i];
                    cmd2.Parameters["@DisplayOrder"].Value = string.IsNullOrEmpty(displayOrder[i])?1: Convert.ToInt32(displayOrder[i]);
                    result += cmd2.ExecuteNonQuery();
                }
                connection.Close();
            }
            return result > 0;
        }

        public List<Product> GetAll()
        {
            List<Product> data = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Products";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Product()
                            {
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                ProductName = Convert.ToString(dbReader["ProductName"]),
                                QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                                UnitPrice = Convert.ToDouble(dbReader["UnitPrice"]),
                                Descriptions = Convert.ToString(dbReader["Descriptions"]),
                                PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
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