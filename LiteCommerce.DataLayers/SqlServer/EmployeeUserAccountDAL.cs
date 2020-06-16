using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace LiteCommerce.DataLayers.SqlServer
{
    /// <summary>
    /// Kiểm tra thông tin đăng nhập nhân viên
    /// </summary>
    public class EmployeeUserAccountDAL : IUserAccountDAL
    {
        /// <summary>
        /// 
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public EmployeeUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">Địa chỉ email của nhân viên</param>
        /// <param name="password">Mật khẩu đã MD5</param>
        /// <returns></returns>
        public UserAccount Authorize(string userName, string password)
        {
            //Mã hóa MD5 mật khẩu
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            string md5Password = sb.ToString();
            // Xong mã hóa MD5
            UserAccount data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT Email,EmployeeID,LastName,FirstName,PhotoPath,GroupName
                                    FROM Employees WHERE Email = @email AND Password = @password";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@email", userName);
                cmd.Parameters.AddWithValue("@password", md5Password);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new UserAccount()
                        {
                            UserID = Convert.ToString(dbReader["EmployeeID"]),
                            FullName = Convert.ToString(dbReader["LastName"]) + " " + Convert.ToString(dbReader["FirstName"]),
                            Photo = Convert.ToString(dbReader["PhotoPath"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            GroupName = Convert.ToString(dbReader["GroupName"]),
                        };
                    }
                }
                connection.Close();
            }
            return data;
            //TODO: Kiểm tra thông tin đăng nhập của Employee
            //return new UserAccount()
            //{
            //    UserID = userName,
            //    FullName = "fff",
            //    Photo = "/Images/pepsi.jpg"
            //};
        }
        public bool ChangePw(string newPassword, string email) {
            int rowsAffected;
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(newPassword);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            string md5Password = sb.ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                    SET                                    
                                        Password = @Password
                                    WHERE Email = @Email";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", md5Password);
                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return rowsAffected > 0;
        }

        public bool CheckPassword(string oldPassword, string email)
        {
            throw new NotImplementedException();
        }
        public bool Update(Employee data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                    SET                                    
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
    }
}
