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
                        //thongSoAqi.TimeLoad = ConvertTimeToFormat(repo.data.max_time);
                    }
                }
                //read file local and insert in database from time max
                Create_Chi_So_Quan_Trac(ma_loai_quan_trac, ma_tram_quan_trac, time_max);

                //DateTime time_start = DateTime.ParseExact(time_max, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                //DateTime time_end = DateTime.Now;
                //ResponseList resChiSo = ChiSoKhongKhiRepository.GetChiSoTramQuanTracByTime(ma_tram_quan_trac,
                //                            String.Concat(time_start.ToString("yyyyMMddHH"), "0000"), 
                //                            String.Concat(time_end.ToString("yyyyMMddHH"), "5959"));
                //if(resChiSo.success)
                //{
                //    List <ChiSoTramQuanTracModel> lstChiSoTramQT = JsonHelper.ToClass<List<ChiSoTramQuanTracModel>>(JsonHelper.ToJson(resChiSo.data).ToString());
                //    var b = 1;
                //}

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
            List<ChiSoTramQuanTracModel> lstChiSoTramQT = ReloadData(ma_quan_trac, ma_tram_quan_trac, time_max);
            Dictionary<string, List<NongDoQuanTracKhongKhiModel>> dicNDQTTheoGio = new Dictionary<string, List<NongDoQuanTracKhongKhiModel>>();
            foreach (var iChiSoTramQT in lstChiSoTramQT)
            {
                string time_hour = iChiSoTramQT.thoi_gian.Substring(0, 10);
                NongDoQuanTracKhongKhiModel item = new NongDoQuanTracKhongKhiModel();

                string[] lineChiSoTramQT = iChiSoTramQT.chi_so_do_dac.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (var iLineChiSoTramQT in lineChiSoTramQT)
                {
                    string[] lineData = iLineChiSoTramQT.Split('	');
                    
                    FileChiSoQuanTracModel model = new FileChiSoQuanTracModel();
                    model.TenChiSo = lineData[0];
                    model.ChiSo = float.Parse(lineData[1], CultureInfo.InvariantCulture.NumberFormat);
                    model.DonVi = lineData[2];
                    model.ThoiGian = lineData[3];
                    model.NoName = lineData[4];
                }
                
                if (dicNDQTTheoGio.ContainsKey(time_hour)){
                    dicNDQTTheoGio[time_hour].Add(item);
                }
                else
                {
                    List<NongDoQuanTracKhongKhiModel> lst = new List<NongDoQuanTracKhongKhiModel>();
                    lst.Add(item);
                    dicNDQTTheoGio.Add(time_hour, lst);
                }
                //ChiSoKhongKhiRepository.Create_ChiSoTramQuanTrac(chi_so_tramQT_in);
            }
            return false;
        }

        private List<ChiSoTramQuanTracModel> ReloadData(string ma_quan_trac, string ma_tram_quan_trac, string time_max)
        {
            List<ChiSoTramQuanTracModel> lstChiSoIn = new List<ChiSoTramQuanTracModel>();
              

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
                ChiSoTramQuanTracModel cs = new ChiSoTramQuanTracModel();
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
    

        private float CalculateAQI(string ma_tram_quan_trac)
        {
            float AQI = 70;
            //DateTime curDateTime = DateTime.Now.AddHours(-1);
           


            return AQI;
        }

        private float MathNowCast()
        {
            float nowCast = 0;

            return nowCast;
        }
    }
}
