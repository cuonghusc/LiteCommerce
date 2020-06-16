using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
        /// Giao diện thông tin người dùng
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            WebUserData userData = User.GetUserData();
            Employee model = HumanResourceBLL.Employee_Get(Convert.ToInt32(userData.UserID));
            return View(model);
        }
        /// <summary>
        /// Thay đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangePwd()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="reNewPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePwd(string oldPassword,string newPassword,string reNewPassword)
        {
            WebUserData userData = User.GetUserData();
            string email = Convert.ToString(userData.Email);
            if (string.IsNullOrEmpty(oldPassword))
            {
                ModelState.AddModelError("OldPassword", "OldPassword is required");
            }
            else
            {
                if (!AccountBLL.Account_CheckPass(oldPassword, email))
                {
                    ModelState.AddModelError("OldPassword", "Old password incorrect");
                }
                ViewBag.OldPassword = oldPassword;
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("NewPassword", "NewPassword is required");
            }
            else
            {
                if(newPassword.Length < 6)
                {
                    ModelState.AddModelError("NewPassword", "Password must over 6 characters");
                }
                ViewBag.NewPassword = newPassword;
            }
            if (string.IsNullOrEmpty(reNewPassword))
            {
                ModelState.AddModelError("ReNewPassword", "ReNewPassword is required");
            }
            else
            {
                if (!newPassword.Equals(reNewPassword))
                {
                    ModelState.AddModelError("ReNewPassword", "The new password does not match the old password");
                }
                ViewBag.ReNewPassword = reNewPassword;
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (UserAccountBLL.Account_ChangePwd(newPassword, email))
            {
                ModelState.AddModelError("success", "Cập nhật thành công");
                ViewBag.OldPassword = "";
                ViewBag.NewPassword = "";
                ViewBag.ReNewPassword = "";
                return View();
            }
            ModelState.AddModelError("error", "Cập nhật không thành công");
            return RedirectToAction("ChangePwd");
        }
        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Đăng nhập người dùng
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string email = "",string password = "")
        {
            if(Request.HttpMethod == "GET")
            {
                return View();
            }
            else
            {
                var userAccount = UserAccountBLL.Authorize(email, password, UserAccountTypes.Employee);
                if(userAccount != null)
                {
                    WebUserData cookieData = new WebUserData()
                    {
                        UserID = userAccount.UserID,
                        FullName = userAccount.FullName,
                        GroupName = userAccount.GroupName,
                        SessionID = Session.SessionID,
                        ClientIP = Request.UserHostAddress,
                        Photo = userAccount.Photo,
                        Email = userAccount.Email
                    };
                    FormsAuthentication.SetAuthCookie(cookieData.ToCookieString(), false);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("error", "Đăng nhập thất bại");
                    ViewBag.email = email;
                    return View();
                }
            }            
        }
        /// <summary>
        /// Hiển thị form chỉnh sửa thông tin người dùng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit()
        {
            WebUserData userData = User.GetUserData();
            Employee model = HumanResourceBLL.Employee_Get(Convert.ToInt32(userData.UserID));
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Employee model, HttpPostedFileBase uploadPhoto)
        {
            if (string.IsNullOrEmpty(model.Notes))
            {
                model.Notes = "";
            }
            WebUserData userData = User.GetUserData();
            model.EmployeeID = Convert.ToInt32(userData.UserID);
            string emailCookie = userData.Email;

            if (!HumanResourceBLL.Employee_CheckEmail(model.EmployeeID, model.Email, "update") && (model.Email != emailCookie))
            {
                ModelState.AddModelError("Email", "Email ready exist");
            }
            //Upload ảnh
            if (uploadPhoto != null && uploadPhoto.ContentLength > 0)
            {
                string filePath = Path.Combine(Server.MapPath("~/Images"), uploadPhoto.FileName);
                uploadPhoto.SaveAs(filePath);
                model.PhotoPath = "/Images/" + uploadPhoto.FileName;
            }
            else if (model.PhotoPath == null)
            {
                model.PhotoPath = userData.Photo;
            }
            DateTime hireDate = DateTime.Today;
            if ((hireDate.Year - (model.BirthDate).Year) < 18)
            {
                ModelState.AddModelError("BirthDate", "You must be over 18 years old");
            }
            //Kiểm tra có tồn tại bất kỳ lỗi nào hay không
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                WebUserData userDatas = User.GetUserData();
                string role = userDatas.GroupName;
                bool rs = UserAccountBLL.Account_Update(model);
                if((model.Email != userData.Email) || (model.PhotoPath != userData.Photo))
                {
                    //
                    WebUserData cookieData = new WebUserData()
                    {
                        UserID = model.EmployeeID.ToString(),
                        FullName = model.LastName + " " + model.FirstName,
                        GroupName = role,
                        SessionID = Session.SessionID,
                        ClientIP = Request.UserHostAddress,
                        Photo = model.PhotoPath,
                        Email = model.Email
                    };
                    FormsAuthentication.SetAuthCookie(cookieData.ToCookieString(), false);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message + ":" + e.StackTrace);
                return View(model);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(string Email = "")
        {
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError("Email", "Email is required");
            }
            try
            {
                if (AccountBLL.Account_Get(Email) != null)
                {
                    string code = "1997";
                    return Redirect("/Account/ChangeForgotPassword?email=" + Email+"&code="+code.GetHashCode());

                }
                ModelState.AddModelError("Email", "Email is not exist");
                return View();
            }
            catch(Exception e)
            {
                ModelState.AddModelError("Email", "Email is not exist");
                return View();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ChangeForgotPassword(string email = "")
        {
            ViewBag.Email = email;
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChangeForgotPassword(string Email = "", string Password = "")
        {
            if (AccountBLL.Account_Get(Email) == null)
            {
                ModelState.AddModelError("error", "Cập nhật không thành công");
                return RedirectToAction("ChangePwd");
            }
            if (UserAccountBLL.Account_ChangePwd(Password, Email))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}