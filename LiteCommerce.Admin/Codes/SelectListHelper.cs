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
        public static List<SelectListItem> ListOfCountries() {
            List<SelectListItem> listCountries = new List<SelectListItem>();
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
    }
}