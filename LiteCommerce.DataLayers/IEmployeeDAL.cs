using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IEmployeeDAL
    {
        /// <summary>
        /// Bổ sung thêm một Employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Employee data);
        /// <summary>
        /// Cập nhật một Employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Employee data);
        /// <summary>
        /// Xóa một hoặc nhiều Employee(s)
        /// </summary>
        /// <param name="EmployeeIDs"></param>
        /// <returns></returns>
        bool Delete(int[] employeeIDs);
        /// <summary>
        /// Lấy thông tin của một Employee
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        Employee Get(int employeeID);
        /// <summary>
        /// Danh sách kết quả tìm kiếm có phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Employee> List(int page, int pageSize, string searchValue, string country);
        /// <summary>
        /// Đếm số nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Count(string searchValue, string country);
        /// <summary>
        /// Check email khi add hoặc update nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        bool CheckEmail(int id, string email, string method);
    }
}
