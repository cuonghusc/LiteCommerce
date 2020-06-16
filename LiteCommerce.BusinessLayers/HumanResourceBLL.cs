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
    public static class HumanResourceBLL
    {
        /// <summary>
        /// 
        /// </summary>
        private static IEmployeeDAL EmployeeDB { get; set; }
        /// <summary>
        /// Hàm này phải được gọi để khởi tạo các chức năng tác nghiệp
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            EmployeeDB = new DataLayers.SqlServer.EmployeeDAL(connectionString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Employee_Count(string searchValue,string country,int idCookie)
        {
            return EmployeeDB.Count(searchValue,country,idCookie);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> Employee_List(int page, int pageSize, string searchValue, string country, int idCookie)
        {            
            if (page < 1)
                page = 1;
            if (pageSize < 1)
                pageSize = 1;
            return EmployeeDB.List(page, pageSize, searchValue, country, idCookie);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int Employee_Add(Employee data)
        {
            return EmployeeDB.Add(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static Employee Employee_Get(int employeeID)
        {
            return EmployeeDB.Get(employeeID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Employee_Update(Employee data)
        {
            return EmployeeDB.Update(data);
        }
        /// <summary>
        /// Check email khi add hoặc update
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="email"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool Employee_CheckEmail(int employeeId,string email,string method)
        {
            return EmployeeDB.CheckEmail(employeeId, email, method);
        }
    }
}
