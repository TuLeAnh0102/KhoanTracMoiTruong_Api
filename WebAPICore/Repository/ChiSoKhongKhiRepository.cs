using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPICore.Model;
using WebAPICore.Model.ChiSoKhongKhi;
using WebAPICore.Model.Response;

namespace WebAPICore.Repository
{
    public class ChiSoKhongKhiRepository
    {
        public static JToken Create_ChiSoTramQuanTrac(Chi_So_TramQT_Create_IN obj)
        {
            ResponseExecute response = new ResponseExecute();
            //insert user
            using (var baseSQL = new BaseSQL())
            {
                var param = new SQLDynamicParameters();
                param.Add("@p_ma_tram_quan_trac", obj.ma_tram_quan_trac);
                param.Add("@p_file_name", obj.file_name);
                param.Add("@p_thoi_gian", obj.thoi_gian);
                param.Add("@p_chi_so_do_dac", obj.chi_so_do_dac);

                response = baseSQL.Execute("Create_ChiSoTramQuanTrac", param);
                return JsonHelper.ToJson(response);
            }
        }

        public static JToken GetKhuVucByLoaiQuanTrac(string loaiQuanTrac)
        {
            //insert user
            using (var baseSQL = new BaseSQL())
            {
                var param = new SQLDynamicParameters();
                param.Add("@P_MAQUANTRAC", loaiQuanTrac);

                var response = baseSQL.GetList("GetKhuVucByLoaiQuanTrac", param);
                return JsonHelper.ToJson(response);
            }
        }

        public static ResponseSingleClass<ThongBaoDanAqiModel> GetThongSoAqiKhongKhi(string ma_loai_quan_trac, float aqi)
        {
            //insert user
            using (var baseSQL = new BaseSQL())
            {
                var param = new SQLDynamicParameters();
                param.Add("@P_aqi", aqi);
                param.Add("@P_ma_loai_quan_trac", ma_loai_quan_trac);

                return baseSQL.GetSingleClass<ThongBaoDanAqiModel>("GetThongSoAqiKhongKhi", param); ;
            }
        }

        public static ResponseSingle GetMaxTimeByTramQuanTrac(string ma_tram_quan_trac)
        {
            //insert user
            using (var baseSQL = new BaseSQL())
            {
                var param = new SQLDynamicParameters();
                param.Add("@P_ma_tram_quan_trac", ma_tram_quan_trac);

                var response = baseSQL.GetSingle("GetMaxTimeByTramQuanTrac", param);
                return response;
            }
        }
    }
}
