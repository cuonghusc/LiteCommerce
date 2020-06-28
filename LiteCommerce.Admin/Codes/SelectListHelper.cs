using LiteCommerce.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin
{
    public class SelectListHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> ListOfCountries(bool allowSelectAll = true) {
            List<SelectListItem> listCountries = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listCountries.Add(new SelectListItem() { Value = "", Text = "--- All Country ---" });
            }
            foreach (var item in CatalogBLL.Country_List())
            {
                listCountries.Add(new SelectListItem() { Value = item.Country,Text= item.Country });
            }
            return listCountries;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> ListOfCategories(bool allowSelectAll = true)
        {
            List<SelectListItem> listCategory = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listCategory.Add(new SelectListItem() { Value = "",Text ="--- All Category ---"});
            }
            foreach(var item in CatalogBLL.Category_List(""))
            {
                listCategory.Add(new SelectListItem() { Value = Convert.ToString(item.CategoryID), Text = item.CategoryName});
            }
            return listCategory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> ListOfSuppliers(bool allowSelectAll = true)
        {
            List<SelectListItem> listSupplier = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listSupplier.Add(new SelectListItem() { Value = "", Text = "--- All Supplier ---" });
            }
            foreach (var item in CatalogBLL.Supplier_ListNoPagination())
            {
                listSupplier.Add(new SelectListItem() { Value = Convert.ToString(item.SupplierID), Text = item.CompanyName });
            }
            return listSupplier;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowSelectAll"></param>
        /// <returns></returns>
        public static List<SelectListItem> ListOfCustomer(bool allowSelectAll = true)
        {
            List<SelectListItem> listCustomer = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listCustomer.Add(new SelectListItem() { Value = "", Text = "--- All Customer ---" });
            }
            foreach (var item in CatalogBLL.Customer_ListNoPagination())
            {
                listCustomer.Add(new SelectListItem() { Value = Convert.ToString(item.CustomerID), Text = item.CompanyName });
            }
            return listCustomer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowSelectAll"></param>
        /// <returns></returns>
        public static List<SelectListItem> ListOfShipper(bool allowSelectAll = true)
        {
            List<SelectListItem> listShipper = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listShipper.Add(new SelectListItem() { Value = "", Text = "--- All Shipper ---" });
            }
            foreach (var item in CatalogBLL.Shipper_List(""))
            {
                listShipper.Add(new SelectListItem() { Value = Convert.ToString(item.ShipperID), Text = item.CompanyName });
            }
            return listShipper;
        }
        public static List<SelectListItem> ListOfProduct(bool allowSelectAll = true)
        {
            List<SelectListItem> listProduct = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listProduct.Add(new SelectListItem() { Value = "", Text = "--- All Products ---" });
            }
            foreach (var item in CatalogBLL.Product_GetAll())
            {
                listProduct.Add(new SelectListItem() { Value = Convert.ToString(item.ProductID), Text = item.ProductName });
            }
            return listProduct;
        }
    }
}