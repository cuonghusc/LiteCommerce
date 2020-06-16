using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public class EmployeePaginationResult:PaginationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Employee> Data;
        /// <summary>
        /// 
        /// </summary>
        public string Country;
    }
}