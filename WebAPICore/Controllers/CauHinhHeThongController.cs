﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebAPICore.Repository;
using Microsoft.AspNetCore.Authorization;
using WebAPICore.Model.CauHinh;

namespace WebAPICore.Controllers
{
    [Authorize]
    [ApiController]
    public class CauHinhHeThongController : ControllerBase
    {

        [HttpGet("api/cau-hinh/get-menu-by-user")]
        public JToken getMenu(int user_id, int role_id)
        {
            return CauHinhHeThongRepository.getMenuByUser(user_id,role_id);
        }
        [HttpGet("api/cau-hinh/get-menu")]
        public JToken getMenuAll(int user_id, int role_id)
        {
            return CauHinhHeThongRepository.getMenuAdmin(user_id, role_id);
        }



        [HttpGet("api/cau-hinh/get-loai-tai-khoan")]
        public JToken GetLoaiTaiKhoan()
        {
            return CauHinhHeThongRepository.getLoaiTaiKhoan();
        }
        [HttpPost("api/cau-hinh/them-them-nhom-quyen")]
        public JToken ThemNhomQuyen(CauHinhModal obj)
        {
            return CauHinhHeThongRepository.modifyNhomQuyen(obj);
        }

    }
}
