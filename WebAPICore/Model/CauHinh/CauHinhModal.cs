using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Model.CauHinh
{
    public class CauHinhModal
    {
        public int id_loai_tai_khoan { get; set; }
        public int id_nhom_quyen { get; set; }
        public string ten_nhom_quyen { get; set; }

        public int id_menu { get; set; }
        public string ten_menu { get; set; }
        public string tag{ get; set; }
        public string icon { get; set; }
        public string duong_dan { get; set; }
        public string id_cha { get; set; }
        public int stt { get; set; }
        public int is_delete { get; set; }
    }
}
