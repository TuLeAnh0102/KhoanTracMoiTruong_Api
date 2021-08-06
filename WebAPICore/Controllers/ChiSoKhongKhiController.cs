using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using WebAPICore.Model.File;
using WebAPICore.Model;
using System.Text.Json;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using WebAPICore.Repository;
using WebAPICore.Model.Response;
using WebAPICore.Model.ChiSoKhongKhi;
using System.Globalization;

namespace WebAPICore.Controllers
{
    [Authorize]
    [ApiController]
    public class ChiSoKhongKhiController : ControllerBase
    {
        private HttpClient client = new HttpClient();

       
        [AllowAnonymous]
        [HttpGet("api/chi-so-khong-khi/thong-so-aqi-khong-khi")]
        public JToken GetThongSoAqiKhongKhi(string ma_loai_quan_trac, string ma_tram_quan_trac)
        {
            ResponseSingle result = new ResponseSingle();
            List<TimeTramQuanTracModel> lstModel = new List<TimeTramQuanTracModel>();
            ThongSoAqiKhongKhiModel thongSoAqi = new ThongSoAqiKhongKhiModel();
            string time_max = string.Empty;
            try
            {
                //Get time max in data base
                ResponseSingle repo = ChiSoKhongKhiRepository.GetMaxTimeByTramQuanTrac(ma_tram_quan_trac);
                if (repo.success)
                {
                    if (!string.IsNullOrWhiteSpace(repo.data.max_time))
                    {
                        time_max = repo.data.max_time;
                        thongSoAqi.TimeLoad = ConvertTimeToFormat(repo.data.max_time);
                    }
                }
                //read file local and insert in database from time max
                Create_Chi_So_Quan_Trac(ma_loai_quan_trac, ma_tram_quan_trac, time_max);


                thongSoAqi.VN_AQI_H = 70;
                thongSoAqi.ThongBaoDan = ThongBaoNguoiDanByAQI(ma_loai_quan_trac, thongSoAqi.VN_AQI_H);

                result.success = true;
                result.data = thongSoAqi;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return JsonHelper.ToJson(result);
        }

        [AllowAnonymous]
        [HttpGet("api/chi-so-khong-khi/dm-khu-vuc")]    
        public JToken get_dm_khu_vuc(string ma_loai_quan_trac)
        {
            return ChiSoKhongKhiRepository.GetKhuVucByLoaiQuanTrac(ma_loai_quan_trac);
        }

        private ThongBaoDanAqiModel ThongBaoNguoiDanByAQI(string ma_loai_quan_trac, float aqi)
        {
            var res=  ChiSoKhongKhiRepository.GetThongSoAqiKhongKhi(ma_loai_quan_trac, aqi);
            if(res.success)
            {
                return res.data;
            }
            return new ThongBaoDanAqiModel();
        }

        private bool Create_Chi_So_Quan_Trac(string ma_quan_trac, string ma_tram_quan_trac, string time_max)
        {
            List<Chi_So_TramQT_Create_IN> lstChiSoIn = ReloadData(ma_quan_trac, ma_tram_quan_trac, time_max);
            foreach (var chi_so_tramQT_in in lstChiSoIn)
            {
                ChiSoKhongKhiRepository.Create_ChiSoTramQuanTrac(chi_so_tramQT_in);
            }
            return false;
        }

        private List<Chi_So_TramQT_Create_IN> ReloadData(string ma_quan_trac, string ma_tram_quan_trac, string time_max)
        {
            List<Chi_So_TramQT_Create_IN> lstChiSoIn = new List<Chi_So_TramQT_Create_IN>();
              

            String strAPI = Startup.ConnectApiLoadData + "api/file/read-tram-quan-trac?ma_loai_quan_trac=KHONG_KHI&ma_tram_quan_trac=DXI_SoTNMT&max_time=" + time_max;
            string strResponse = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strAPI);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                strResponse = readStream.ReadToEnd();
            }

            List<TimeTramQuanTracModel> lstTramQT = new List<TimeTramQuanTracModel>();
            lstTramQT = JsonHelper.ToClass<List<TimeTramQuanTracModel>>(strResponse);
            foreach (TimeTramQuanTracModel timeTramQT in lstTramQT)
            {
                Chi_So_TramQT_Create_IN cs = new Chi_So_TramQT_Create_IN();
                cs.file_name = timeTramQT.TenFile;
                cs.chi_so_do_dac = timeTramQT.ChiSoDoDac;
                cs.thoi_gian = timeTramQT.ThoiGian;
                cs.ma_tram_quan_trac = ma_tram_quan_trac;
                lstChiSoIn.Add(cs);                
            }

            return lstChiSoIn;
        }
    
        private string ConvertTimeToFormat(string time)
        {
            if (string.IsNullOrWhiteSpace(time))
                return string.Empty;
            DateTime datetime = DateTime.ParseExact(time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            return datetime.ToString("HH:ss dd/MM/yyyy");
        }
    }
}
