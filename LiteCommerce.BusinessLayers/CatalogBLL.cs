using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class CatalogBLL
    {
        /// <summary>
        /// SupplierDB
        /// </summary>
        private static ISupplierDAL SupplierDB { get; set; }
        /// <summary>
        /// CustomerDB
        /// </summary>
        private static ICustomerDAL CustomerDB { get; set; }
        /// <summary>
        /// ShipperDB
        /// </summary>
        private static IShipperDAL ShipperDB { get; set; }
        /// <summary>
        /// CategoryDB
        /// </summary>
        private static ICategoryDAL CategoryDB { get; set; }
        /// <summary>
        /// ProductDB
        /// </summary>
        private static IProductDAL ProductDB { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private static ICountryDAL CountryDB { get; set; }
        /// <summary>
        /// Hàm này phải được gọi để khởi tạo các chức năng tác nghiệp
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            SupplierDB = new DataLayers.SqlServer.SupplierDAL(connectionString);
            CustomerDB = new DataLayers.SqlServer.CustomerDAL(connectionString);
            ShipperDB = new DataLayers.SqlServer.ShipperDAL(connectionString);
            CategoryDB = new DataLayers.SqlServer.CategoryDAL(connectionString);
            ProductDB = new DataLayers.SqlServer.ProductDAL(connectionString);
            CountryDB = new DataLayers.SqlServer.CountryDAL(connectionString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Supplier> Supplier_List(int page, int pageSize, string searchValue)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 1)
                pageSize = 1;
            return SupplierDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int Supplier_Add(Supplier data)
        {
            return SupplierDB.Add(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        public static bool Supplier_Delete(int[] supplierIDs)
        {
            return SupplierDB.Delete(supplierIDs);
        }
        public static Supplier Supplier_Get(int supplierID)
        {
            return SupplierDB.Get(supplierID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Supplier> Supplier_ListNoPagination()
        {
            return SupplierDB.GetAll();
        }
        /// <summary>
        /// Đếm số Supplier
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Supplier_Count(string searchValue)
        {
            return SupplierDB.Count(searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Supplier_Update(Supplier data)
        {
            return SupplierDB.Update(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> Customer_List(int page, int pageSize, string searchValue)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 1)
                pageSize = 1;
            return CustomerDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int Customer_Add(Customer data)
        {
            return CustomerDB.Add(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static bool Customer_Delete(string[] customerID)
        {
            return CustomerDB.Delete(customerID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static Customer Customer_Get(string customerID)
        {
            return CustomerDB.Get(customerID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Customer_Count(string searchValue)
        {
            return CustomerDB.Count(searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Customer_Update(Customer data)
        {
            return CustomerDB.Update(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Shipper> Shipper_List(string searchValue)
        {
            return ShipperDB.List(searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int Shipper_Add(Shipper data)
        {
            return ShipperDB.Add(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static bool Shipper_Delete(int[] shipperID)
        {
            return ShipperDB.Delete(shipperID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static Shipper Shipper_Get(int shipperID)
        {
            return ShipperDB.Get(shipperID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Shipper_Count(string searchValue)
        {
            return ShipperDB.Count(searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Shipper_Update(Shipper data)
        {
            return ShipperDB.Update(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> Category_List(string searchValue)
        {            
            return CategoryDB.List(searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int Category_Add(Category data)
        {
            return CategoryDB.Add(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static bool Category_Delete(int[] categoryID)
        {
            return CategoryDB.Delete(categoryID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static Category Category_Get(int categoryID)
        {
            return CategoryDB.Get(categoryID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Category_Count(string searchValue)
        {
            return CategoryDB.Count(searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Category_Update(Category data)
        {
            return CategoryDB.Update(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Product> Product_List(int page, int pageSize, string searchValue, string categoryId,string supplierID)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 1)
                pageSize = 1;
            return ProductDB.List(page, pageSize, searchValue, categoryId, supplierID);
        }        
        /// <summary>
        /// Đếm số products
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Product_Count(string searchValue,string categoryID,string supplierID)
        {
            return ProductDB.Count(searchValue, categoryID, supplierID);
        }       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int Product_Add(Product data)
        {
            return ProductDB.Add(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Product_Update(Product data)
        {
            return ProductDB.Update(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Product Product_Get(string id)
        {
            return ProductDB.Get(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productIDs"></param>
        /// <returns></returns>
        public static bool Product_Delete(int[] productIDs)
        {
            return ProductDB.Delete(productIDs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static List<ProductAttributes> Product_GetAttribute(string productID)
        {
            return ProductDB.GetAttribute(productID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValues"></param>
        /// <param name="displayOrder"></param>
        /// <returns></returns>
        public static bool Product_UpdateAttribute(string productID, string[] attributeName, string[] attributeValues, string[] displayOrder)
        {
            return ProductDB.UpdateAttribute(productID, attributeName, attributeValues, displayOrder);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Countries> Country_List()
        {
            return CountryDB.List();
        }
    }
}