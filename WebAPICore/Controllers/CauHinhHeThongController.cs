using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebAPICore.Repository;
using Microsoft.AspNetCore.Authorization;
using WebAPICore.Model.CauHinh;
//using System.Dynamic;

namespace WebAPICore.Controllers
{
    [Authorize]
    [ApiController]
    public class CauHinhHeThongController : ControllerBase
    {
        //dynamic objtest = new ExpandoObject();
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
        [HttpPost("api/cau-hinh/update-danh-sach-menu")]
        public JToken updateDsMenu(CauHinhModal obj)
        {
            return CauHinhHeThongRepository.updateDanhSachMenuHeThong(obj);
        }
        [AllowAnonymous]
        [HttpPost("api/cau-hinh/test-bot")]
        public JToken updateDsMenu(object set_variables)
        {
            return CauHinhHeThongRepository.testbot(set_variables);
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
