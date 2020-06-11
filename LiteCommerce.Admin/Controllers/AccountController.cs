using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            //HttpCookie requestCookies = Request.Cookies["userInfo"];
            //string email = Convert.ToString(requestCookies["Email"]);
            //Account model = AccountBLL.Account_Get(email);
            //return View(model);
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
            HttpCookie requestCookie = Request.Cookies["userInfo"];
            int id = Convert.ToInt32(requestCookie["AccountID"]);
            if (string.IsNullOrEmpty(oldPassword))
            {
                ModelState.AddModelError("OldPassword", "OldPassword is required");
            }
            else
            {
                if (!AccountBLL.Account_CheckPass(oldPassword,id))
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
            if (AccountBLL.Account_ChangePwd(newPassword, id)){
                return RedirectToAction("SignOut");
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
            //foreach (string key in Request.Cookies.AllKeys)
            //{
            //    HttpCookie requestCookies = Request.Cookies["userInfo"];
            //    requestCookies.Expires = DateTime.Now.AddMonths(-1);
            //    Response.AppendCookie(requestCookies);
            //}
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
                //var newUser = AccountBLL.Account_Login(email, password);
                //if (newUser!=null)
                //{
                //    Account account = AccountBLL.Account_Login(email, password);
                //    FormsAuthentication.SetAuthCookie(account.AccountID.ToString(), false);
                //    HttpCookie userInfo = new HttpCookie("userInfo");
                //    userInfo["AccountID"] = account.AccountID.ToString();
                //    userInfo["FullName"] = Server.UrlEncode(account.LastName +" "+ account.FirstName);
                //    userInfo["Email"] = account.Email;
                //    userInfo["PhotoPath"] = account.PhotoPath;
                //    userInfo.Expires = DateTime.Now.AddDays(1);
                //    Response.Cookies.Add(userInfo);
                //    return RedirectToAction("Index", "Dashboard");
                //}
                //else
                //{
                //    ModelState.AddModelError("error", "Đăng nhập thất bại");
                //    ViewBag.email = email;
                //    return View();
                //}
                var userAccount = UserAccountBLL.Authorize(email, password, UserAccountTypes.Employee);
                if(userAccount != null)
                {
                    WebUserData cookieData = new WebUserData()
                    {
                        UserID = userAccount.UserID,
                        FullName = userAccount.FullName,
                        GroupName = WebUserRoles.STAFF,
                        SessionID = Session.SessionID,
                        ClientIP = Request.UserHostAddress,
                        Photo = userAccount.Photo
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
        /// Quên mật khẩu
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return null;
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
        public ActionResult Edit(Account model, HttpPostedFileBase uploadPhoto)
        {
            //if (string.IsNullOrEmpty(model.Email))
            //{
            //    ModelState.AddModelError("Email", "Email is required");
            //}
            //if (string.IsNullOrEmpty(model.HomePhone))
            //{
            //    ModelState.AddModelError("HomePhone", "Phone is required");
            //}
            //if (string.IsNullOrEmpty(model.Address))
            //{
            //    ModelState.AddModelError("Address", "Address is required");
            //}
            //if (string.IsNullOrEmpty(model.Country))
            //{
            //    ModelState.AddModelError("Country", "Country is required");
            //}
            //if (string.IsNullOrEmpty(model.City))
            //{
            //    ModelState.AddModelError("City", "City is required");
            //}
            //if (string.IsNullOrEmpty(model.Notes))
            //{
            //    model.Notes = "";
            //}
            //HttpCookie requestCookies = Request.Cookies["userInfo"];
            //model.AccountID = Convert.ToInt32(requestCookies["AccountID"]);
            //string emailCookie = Convert.ToString(requestCookies["Email"]);
            //if (!HumanResourceBLL.Employee_CheckEmail(model.AccountID, model.Email, "update") && (model.Email != emailCookie))
            //{
            //    ModelState.AddModelError("Email", "Email ready exist");
            //}
            ////Upload ảnh
            //if (uploadPhoto != null && uploadPhoto.ContentLength > 0)
            //{
            //    string filePath = Path.Combine(Server.MapPath("~/Images"), uploadPhoto.FileName);
            //    uploadPhoto.SaveAs(filePath);
            //    model.PhotoPath = "/Images/" + uploadPhoto.FileName;
            //    requestCookies.Values["PhotoPath"] = Convert.ToString(model.PhotoPath);
            //    Response.SetCookie(requestCookies);
            //}
            //else if (model.PhotoPath == null)
            //{
            //    model.PhotoPath = Convert.ToString(requestCookies["PhotoPath"]);
            //}
            //DateTime hireDate = DateTime.Today;
            //if ((hireDate.Year - (model.BirthDate).Year) < 18)
            //{
            //    ModelState.AddModelError("BirthDate", "You must be over 18 years old");
            //}
            ////Kiểm tra có tồn tại bất kỳ lỗi nào hay không
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            //try
            //{
            //    bool rs = AccountBLL.Account_Update(model);
            //    requestCookies.Values["Email"] = model.Email;
            //    Response.SetCookie(requestCookies);
            //    return RedirectToAction("Index");                               
            //}
            //catch (Exception e)
            //{
            //    ModelState.AddModelError("", e.Message + ":" + e.StackTrace);
            //    return View(model);
            //}
            return RedirectToAction("Index");  
        }
    }
}