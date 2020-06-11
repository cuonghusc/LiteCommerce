using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin
{
    public class WebUserData
    {
        /// <summary>
        /// ID/tên đăng nhập của tài khoản
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// Tên gọi/tên hiển thị của tài khoản
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Tên nhóm
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// Thời điểm đăng nhập
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// Session ID
        /// </summary>
        public string SessionID { get; set; }
        /// <summary>
        /// Địa chỉ IP của user khi đăng nhập
        /// </summary>
        public string ClientIP { get; set; }
        /// <summary>
        /// Dữ liệu bổ sung dùng cho mục đích khác nếu có
        /// (lưu ý: dữ liệu là chuỗi và không được chứa dấu |)
        /// </summary>
        public string AditionalData { get; set; }
        /// <summary>
        /// Chuyển thông tin tài khoản đăng nhập thành chuỗi để ghi Cookie
        /// </summary>
        /// <returns></returns>
        public string ToCookieString()
        {
            return string.Format($"{UserID}|{FullName}|{GroupName}|{LoginTime}|{SessionID}|{ClientIP}|{AditionalData}");
        }

        /// <summary>
        /// Lấy thông tin tài khoản đăng nhập từ Cookie
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static WebUserData FromCookieString(string cookie)
        {
            try
            {
                string[] infos = cookie.Split('|');
                if (infos.Length == 7)
                {
                    return new WebUserData()
                    {
                        UserID = infos[0],
                        FullName = infos[1],
                        GroupName = infos[2],
                        LoginTime = Convert.ToDateTime(infos[3]),
                        SessionID = infos[4],
                        ClientIP = infos[5],
                        AditionalData = infos[6]
                    };
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}