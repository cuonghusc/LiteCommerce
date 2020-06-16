using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class CustomerUserAccountDAL : IUserAccountDAL
    {
        /// <summary>
        /// 
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public CustomerUserAccountDAL(string connectionString)
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
            
            //TODO: Kiểm tra thông tin đăng nhập của Employee
            return new UserAccount()
            {
                UserID = userName,
                FullName = "",
                Photo = "/Images/pepsi.jpg"
            };
        }

        public bool ChangePw(string newPassword, string email)
        {
            throw new NotImplementedException();
        }

        public bool CheckPassword(string oldPassword, string email)
        {
            throw new NotImplementedException();
        }

        public bool Update(Employee model)
        {
            throw new NotImplementedException();
        }
    }
}
