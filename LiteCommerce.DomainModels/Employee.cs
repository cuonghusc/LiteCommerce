using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// 
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int EmployeeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string LastName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Email is valid")]
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string City { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string HomePhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PhotoPath { get; set; }
    }
}
