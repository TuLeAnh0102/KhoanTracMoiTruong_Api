using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPICore.Dtos;
using WebAPICore.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using WebAPICore.Model;
using WebAPICore.Model.Response;
using WebAPICore.Repository;
using System.Data;
using WebAPICore.Helpers;
using Microsoft.Extensions.Options;
using WebAPICore.Model.NhanVien;

namespace WebAPICore.Controllers
{
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [HttpGet("api/users/getall")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("api/nhan-vien/login")]
        public JToken Login(string username, string password)
        {
            var response = new ResponseSingle();
            var userResponse = _userService.Login(username, password);
            if (!userResponse.success)
                response = userResponse;
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, userResponse.data.MA_NHAN_VIEN_KC.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                var data = new
                {
                    token = tokenString,
                    ten_chot = userResponse.data.DON_VI,
                    ma_chot = userResponse.data.MA_CHOT,
                    ma_nhan_vien_kc = userResponse.data.MA_NHAN_VIEN_KC,
                    is_chot = userResponse.data.IS_CHOT,
                    is_lanh_dao = userResponse.data.IS_LANH_DAO,
                    is_kcn = userResponse.data.IS_KCN,
                    is_chot_kb_gtvt = userResponse.data.IS_CHOT_KB_GTVT,
                    is_admin_gtvt = userResponse.data.IS_ADMIN_GTVT,            
                    is_cong_ty = userResponse.data.IS_CONG_TY,
                    ma_cong_ty= userResponse.data.MA_CONG_TY,
                    ma_kcn = userResponse.data.MA_KCN,
					is_so_ldtbxh = userResponse.data.IS_SO_LDTBXH,
					is_admin =  userResponse.data.IS_ADMIN                
                };

                response.success = true;
                response.data = data;
            }
            return JsonHelper.ToJson(response);
        }

        [AllowAnonymous]
        [HttpPost("api/nhan-vien/cap-nhat-thong-tin")]
        public JToken CapNhatThongTinNhanVien(NV_CUD_NHAN_VIEN_IN obj)
        {
            return _userService.CapNhatThongTin(obj);
        }

        [AllowAnonymous]
        [HttpPost("api/nhan-vien/tao-nhan-vien")]
        public JToken ThemNhanVien(string username, string password, string hoten, string donvi)
        {
            NV_CUD_NHAN_VIEN_IN user = new NV_CUD_NHAN_VIEN_IN();
            user.ho_va_ten_nhan_vien = hoten;
            user.don_vi = donvi;
            user.username = username;
            user.password = password;
            return _userService.CapNhatThongTin(user);
        }

        [AllowAnonymous]
        [HttpPost("api/nhan-vien/reset-pass")]
        public JToken ResetPass(string username, string password, string key)
        {
            return _userService.ResetPass(username,password, key);
        }

        [HttpGet("api/nhan-vien/danh-sach-nhan-vien")]
        public JToken DanhSachNhanVien()
        {
            return _userService.DanhSachNhanVien();
        }

        [HttpGet("api/nhan-vien/thong-tin-nhan-vien")]
        public JToken ThongTinNhanVien(int ma_nhan_vien_kc)
        {
            return _userService.ThongTinNhanVien(ma_nhan_vien_kc);
        }
    }
}