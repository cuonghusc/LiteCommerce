using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = WebUserRoles.Administrator)]
    public class EmployeeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "",string country = "")
        {
            //HttpCookie requestCookie = Request.Cookies["userInfo"];
            //int idCookie = Convert.ToInt32(requestCookie["AccountID"]);

            WebUserData userData = User.GetUserData();
            int idCookie = Convert.ToInt32(userData.UserID);
            var model = new Models.EmployeePaginationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = HumanResourceBLL.Employee_Count(searchValue,country,idCookie),
                Data = HumanResourceBLL.Employee_List(page, AppSettings.DefaultPageSize, searchValue, country, idCookie),
                SearchValue = searchValue,
                Country = country,
            };
            return View(model);
        }
        /// <summary>
        /// Hiển thị form thêm/sửa Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add New Employee";
                ViewBag.ConfirmButton = "Add";
                ViewBag.Method = "add";

                Employee newEmployee = new Employee();
                newEmployee.EmployeeID = 0;
                return View(newEmployee);
            }
            else
            {
                //HttpCookie requestCookie = Request.Cookies["userInfo"];
                //string idCookie = requestCookie["AccountID"];

                WebUserData userData = User.GetUserData();
                string idCookie = userData.UserID;

                if (id.Equals(idCookie))
                {
                    return Redirect("~/Account");
                }
                else
                {
                    ViewBag.Title = "Edit Employee";
                    ViewBag.ConfirmButton = "Save";
                    ViewBag.Method = "update";
                    try
                    {
                        Employee editEmployee = HumanResourceBLL.Employee_Get(Convert.ToInt32(id));
                        if (editEmployee == null)
                        {
                            return RedirectToAction("Index");
                        }
                        return View(editEmployee);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return RedirectToAction("Index");
                    }
                }                
            }
        }
        [HttpPost]
        public ActionResult Input(Employee model,HttpPostedFileBase uploadPhoto,string method)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Notes))
                {
                    model.Notes = "";
                }         
                //Check email
                try
                {
                    if (!HumanResourceBLL.Employee_CheckEmail(model.EmployeeID, model.Email, method))
                    {
                        ModelState.AddModelError("Email", "Email already exists");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message + ":" + e.StackTrace);
                }
                //Upload ảnh
                if (uploadPhoto != null && uploadPhoto.ContentLength > 0)
                {
                    string filePath = Path.Combine(Server.MapPath("~/Images"), uploadPhoto.FileName);
                    uploadPhoto.SaveAs(filePath);
                    model.PhotoPath = "Images/" + uploadPhoto.FileName;
                }
                else if (model.PhotoPath == null)
                {
                    model.PhotoPath = "";
                }
                DateTime hireDate = DateTime.Today;
                if ((hireDate.Year - (model.BirthDate).Year) < 18)
                {
                    ModelState.AddModelError("BirthDate", "You must be over 18 years old");
                }
                if (!ModelState.IsValid)
                {
                    if (model.EmployeeID == 0)
                    {
                        ViewBag.Title = "Add New Employee";
                        ViewBag.ConfirmButton = "Add";
                        ViewBag.Method = "add";
                        return View(model);
                    }
                    else
                    {
                        ViewBag.Title = "Edit Employee";
                        ViewBag.ConfirmButton = "Save";
                        ViewBag.Method = "update";
                        return View(model);
                    }                    
                }                
                //Đưa dữ liệu vào CSDL
                if (model.EmployeeID == 0)
                {
                    int shipperID = HumanResourceBLL.Employee_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool rs = HumanResourceBLL.Employee_Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message + ":" + e.StackTrace);
                return View(model);
            }
        }
    }
}