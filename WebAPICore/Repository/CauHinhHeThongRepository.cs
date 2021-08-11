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
