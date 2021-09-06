using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Model
{
    public class NongDoQuanTracKhongKhiModel
    {
        public float SO2 { get; set; }
        public float CO { get; set; }
        public float NO2 { get; set; }
        public float O3 { get; set; }
        public float PM10 { get; set; }
        public float PM2_5 { get; set; }
    }

    public class ChiSoTramQuanTracModel
    {
        public string ma_tram_quan_trac { get; set; }
        public string thoi_gian { get; set; }
        public string chi_so_do_dac { get; set; }
        public string file_name { get; set; }
    }

}
