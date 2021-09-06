using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Data;
using WebAPICore.Model;
using WebAPICore.Model.CauHinh;

namespace WebAPICore.Repository
{
    public class CauHinhHeThongRepository
    {
        public static JToken getMenuByUser(int user_id, int role_id)
        {
            using (var baseSQL = new BaseSQL())
            {
                var param = new SQLDynamicParameters();
                param.Add("p_user_id", user_id);
                param.Add("p_role_id", role_id);
                var response = baseSQL.GetList("CAUHINH_Get_Menu_By_User", param);
                return JsonHelper.ToJson(response);
            }
        }
        public static JToken getMenuAdmin(int user_id, int role_id)
        {
            using (var baseSQL = new BaseSQL())
            {
                var param = new SQLDynamicParameters();
                param.Add("p_user_id", user_id);
                param.Add("p_role_id", role_id);
                var response = baseSQL.GetList("CAUHINH_Get_Danh_Sach_Menu", param);
                return JsonHelper.ToJson(response);
            }
        }
        public static JToken updateDanhSachMenuHeThong(CauHinhModal obj)
        {
            using (var baseSQL = new BaseSQL())
            {
                var param = new SQLDynamicParameters();
                param.Add("p_id_menu", obj.id_menu);
                param.Add("p_ten_menu", obj.ten_menu);
                param.Add("p_tag", obj.tag);
                param.Add("p_icon", obj.icon);
                param.Add("p_duong_dan", obj.duong_dan);
                param.Add("p_stt", obj.stt);
                param.Add("p_id_cha", obj.id_cha);
                param.Add("p_is_delete", obj.is_delete);
                var response = baseSQL.GetList("CAUHINH_Update_Danh_Sach_Menu", param);
                return JsonHelper.ToJson(response);
            }
        }


        public static JToken getLoaiTaiKhoan()
        {
            using (var baseSQL = new BaseSQL())
            {
                var param = new SQLDynamicParameters();
                var response = baseSQL.GetList("CAUHINH_GET_LOAI_TAI_KHOAN", param);
                return JsonHelper.ToJson(response);
            }
        }
        public static JToken modifyNhomQuyen(CauHinhModal obj)
        {
            using (var baseSQL = new BaseSQL())
            {
                var param = new SQLDynamicParameters();
                param.Add("p_id_nhom_quyen", obj.id_nhom_quyen);
                param.Add("p_ten_nhom_quyen", obj.ten_nhom_quyen);
                param.Add("p_id_loai_tai_khoan", obj.id_loai_tai_khoan);
                var response = baseSQL.Execute("CAUHINH_THEM_NHOM_QUYEN", param);
                return JsonHelper.ToJson(response);
            }
        }


    }
}
