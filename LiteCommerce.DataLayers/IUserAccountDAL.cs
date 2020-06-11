﻿using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IUserAccountDAL
    {
        /// <summary>
        /// Kiểm tra thông tin đăng nhập của user xem có hợp lệ hay không ?
        /// - Nếu hợp lệ hà trả về thông tin user
        /// - Ngược lại hàm trả về null
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount Authorize(string userName, string password);
    }
}
