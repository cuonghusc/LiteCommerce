using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class UserAccountBLL
    {
        /// <summary>
        /// 
        /// </summary>
        private static string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            UserAccountBLL.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static UserAccount Authorize(string userName, string password, UserAccountTypes userType)
        {
            IUserAccountDAL userAccountDB;
            switch (userType)
            {
                case UserAccountTypes.Employee:
                    userAccountDB = new EmployeeUserAccountDAL(connectionString);
                    break;
                case UserAccountTypes.Customer:
                    userAccountDB = new CustomerUserAccountDAL(connectionString);
                    break;
                default:
                    throw new Exception("Usertype is not valid");
            }
            return userAccountDB.Authorize(userName,password);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Account_ChangePwd(string newPassword, string email)
        {
            IUserAccountDAL UserAccountDB = new EmployeeUserAccountDAL(connectionString);
            return UserAccountDB.ChangePw(newPassword, email);
        }
        public static bool Account_Update(Employee model)
        {
            IUserAccountDAL UserAccountDB = new EmployeeUserAccountDAL(connectionString);
            return UserAccountDB.Update(model);
        }
    }
}
