using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Model.ChiSoKhongKhi
{
    public class ThongSoAqiKhongKhiModel
    {
        private float _VN_AQI_H = 0;
        private string _timeLoad = "0";
        private ThongBaoDanAqiModel _thongBaoDan = new ThongBaoDanAqiModel();
        private AqiModel _KK_SO2 = new AqiModel();
        private AqiModel _KK_CO = new AqiModel();
        private AqiModel _KK_NO2 = new AqiModel();
        private AqiModel _KK_O3 = new AqiModel();
        private AqiModel _KK_TSP = new AqiModel();
        private AqiModel _KK_PM1_0 = new AqiModel();
        private AqiModel _KK_PM2_5 = new AqiModel();

        public float VN_AQI_H { get => _VN_AQI_H; set => _VN_AQI_H = value; }
        public AqiModel KK_SO2 { get => _KK_SO2; set => _KK_SO2 = value; }
        public AqiModel KK_CO { get => _KK_CO; set => _KK_CO = value; }
        public AqiModel KK_NO2 { get => _KK_NO2; set => _KK_NO2 = value; }
        public AqiModel KK_O3 { get => _KK_O3; set => _KK_O3 = value; }
        public AqiModel KK_TSP { get => _KK_TSP; set => _KK_TSP = value; }
        public AqiModel KK_PM1_0 { get => _KK_PM1_0; set => _KK_PM1_0 = value; }
        public AqiModel KK_PM2_5 { get => _KK_PM2_5; set => _KK_PM2_5 = value; }
        public string TimeLoad { get => _timeLoad; set => _timeLoad = value; }
        public ThongBaoDanAqiModel ThongBaoDan { get => _thongBaoDan; set => _thongBaoDan = value; }
    }
    public class AqiModel
    {
        private float _AQI_H = 0;
        private float _AQI_8H = 0;
        private float _AQI_24H = 0;

        public float AQI_H { get => _AQI_H; set => _AQI_H = value; }
        public float AQI_8H { get => _AQI_8H; set => _AQI_8H = value; }
        public float AQI_24H { get => _AQI_24H; set => _AQI_24H = value; }
    }

    public class ThongBaoDanAqiModel
    {
        public string chat_luong_khong_khi { get; set; }
        public string ma_mau_rbg  { get; set; }
        public string mau_sac { get; set; }
        public string anh_huong_suc_khoe { get; set; }
        public string kn_nguoi_binh_thuong { get; set; }
        public string kn_nguoi_nhay_cam { get; set; }
    }
}
