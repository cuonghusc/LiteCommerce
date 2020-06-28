using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface ICustomerDAL
    {
        /// <summary>
        /// Bổ sung thêm một Customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Customer data);
        /// <summary>
        /// Cập nhật một Customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Customer data);
        /// <summary>
        /// Xóa một hoặc nhiều Customer(s)
        /// </summary>
        /// <param name="CustomerIDs"></param>
        /// <returns></returns>
        bool Delete(string[] customerIDs);
        /// <summary>
        /// Lấy thông tin của một Customer
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        Customer Get(string customerID);
        /// <summary>
        /// Danh sách kết quả tìm kiếm có phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Customer> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Customer> GetAll();
    }
}
