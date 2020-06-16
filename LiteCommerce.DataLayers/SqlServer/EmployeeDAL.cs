using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace LiteCommerce.DataLayers.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public class EmployeeDAL : IEmployeeDAL
    {
        /// <summary>
        /// 
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public EmployeeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Employee data)
        {
            string passwordDefault = "e10adc3949ba59abbe56e057f20f883e";
            int employeeId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                DateTime HireDate = DateTime.Now;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Employees
                                          (
	                                          LastName,
	                                          FirstName,
	                                          Title,
                                              BirthDate,
                                              HireDate,
                                              Email,       
	                                          Address,
	                                          City,
	                                          Country,
	                                          HomePhone,
                                              Notes,
                                              PhotoPath,
                                              Password
                                          )
                                          VALUES
                                          (
	                                          @LastName,
	                                          @FirstName,
	                                          @Title,
                                              @BirthDate,
                                              @HireDate,
                                              @Email,
	                                          @Address,
	                                          @City,
	                                          @Country,
	                                          @HomePhone,
                                              @Notes,
                                              @PhotoPath,
                                              @Password
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = data.BirthDate;
                cmd.Parameters.Add("@HireDate", SqlDbType.DateTime).Value = HireDate;
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Password", passwordDefault);
                employeeId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return employeeId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeIDs"></param>
        /// <returns></returns>
        public bool Delete(int[] employeeIDs)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Employees
                                            WHERE(EmployeeID = @employeeId)
                                              AND(EmployeeID NOT IN(SELECT EmployeeID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@employeeId", SqlDbType.Int);
                foreach (int employeeId in employeeIDs)
                {
                    cmd.Parameters["@employeeId"].Value = employeeId;
                    result += cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            return result > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public Employee Get(int employeeID)
        {
            Employee data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Employees WHERE EmployeeID = @employeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@employeeID", employeeID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
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
        public List<Employee> List(int page, int pageSize, string searchValue,string country,int idCookie)
        {
            List<Employee> data = new List<Employee>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT *
                                        FROM(
                                            SELECT *, ROW_NUMBER() OVER(ORDER BY EmployeeID) AS RowNumber
                                            FROM Employees
                                            WHERE
                                                (EmployeeID <> @idCookie)
			                                    AND (
				                                    (@searchValue = N'')
				                                    OR (FirstName like @searchValue)
				                                    OR (LastName like @searchValue)
				                                    )
                                                AND (
                                                    (@country = N'')
				                                    OR (Country = @country)
                                                    )
                                        ) AS t WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize
                                        ORDER BY t.RowNumber";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    
                    cmd.Parameters.AddWithValue("idCookie",idCookie);
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@country", country);
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Employee()
                            {
                                EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                                FirstName = Convert.ToString(dbReader["FirstName"]),
                                LastName = Convert.ToString(dbReader["LastName"]),
                                Title = Convert.ToString(dbReader["Title"]),
                                BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                                HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                                Email = Convert.ToString(dbReader["Email"]),
                                Address = Convert.ToString(dbReader["Address"]),
                                City = Convert.ToString(dbReader["City"]),
                                Country = Convert.ToString(dbReader["Country"]),
                                HomePhone = Convert.ToString(dbReader["HomePhone"]),
                                Notes = Convert.ToString(dbReader["Notes"]),
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
        public bool Update(Employee data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                    SET   
                                        FirstName = @FirstName,
                                        LastName = @LastName,
                                        Title = @Title,
                                        BirthDate = @BirthDate,
                                        Email = @Email,
                                        Address = @Address,
                                        City = @City,
                                        Country = @Country,
                                        HomePhone = @HomePhone,
                                        PhotoPath = @PhotoPath,
                                        Notes = @Notes
                                    WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                DateTime birthDate = DateTime.Parse(Convert.ToString(data.BirthDate), CultureInfo.CreateSpecificCulture("fr-FR"));
                cmd.Parameters.AddWithValue("@BirthDate", birthDate);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return rowsAffected > 0;
        }
        /// <summary>
        /// Đếm nhân viên
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue, string country, int idCookie)
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
                                        FROM Employees
                                        WHERE
                                            (EmployeeID <> @idCookie)
                                            AND(
                                                (@searchValue = N'')
                                                OR (FirstName like @searchValue)
                                                OR (LastName like @searchValue)
                                            )
                                            AND(
                                                (@country = N'')
                                                OR (Country = @country)
                                            )";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@idCookie", idCookie);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@country", country);

                    dem = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return dem;
        }
        /// <summary>
        /// Check email khi add hoặc update nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool CheckEmail(int employeeId, string email, string method)
        {
            int dem;
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using(SqlCommand cmd = new SqlCommand())
                {
                    if (method == "add")
                    {
                        cmd.CommandText = @"SELECT COUNT(*) FROM Employees WHERE (Email = @email)";
                    }
                    else if (method=="update")
                    {
                        cmd.CommandText = @"SELECT COUNT(*) FROM Employees WHERE (Email = @email) AND (EmployeeID <> @employeeId)";
                    }
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);

                    dem = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return (dem == 0);
        }

    }
}
