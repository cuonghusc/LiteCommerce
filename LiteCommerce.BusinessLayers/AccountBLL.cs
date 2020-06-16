using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountBLL
    {
        /// <summary>
        /// 
        /// </summary>
        private static IAccountDAL AccountDB { get; set; }
        /// <summary>
        /// Hàm này phải được gọi để khởi tạo các chức năng tác nghiệp
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            AccountDB = new DataLayers.SqlServer.AccountDAL(connectionString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Account Account_Login(string email, string password)
        {
            return AccountDB.Login(email, password);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static Account Account_Get(string email) {
            return AccountDB.Get(email);
        }
        public static bool Account_Update(Account model)
        {
            return AccountDB.Update(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Account_CheckPass(string oldPassword, string email)
        {
            return AccountDB.CheckPassword(oldPassword, email);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Account_ChangePwd(string newPassword, string email)
        {
            return AccountDB.ChangePw(newPassword, email);
        }
    }
}
