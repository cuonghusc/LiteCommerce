using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;

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
            UserAccount data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT TOP 1 * FROM Employees WHERE Email = @email AND Password = @password";
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
                            Photo = Convert.ToString(dbReader["PhotoPath"])
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
    }
}
