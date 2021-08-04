using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Model.NhanVien
{
    public class NV_CUD_NHAN_VIEN_IN
    {
        public NV_CUD_NHAN_VIEN_IN()
        {
            this.username = string.Empty;
            this.ho_va_ten_nhan_vien = string.Empty;
            this.password = string.Empty;
            this.don_vi = string.Empty;
            this.is_kcn = false;
            this.is_cong_ty = false;
            this.is_lanh_dao = false;
            this.is_chot = false;
            this.ma_cong_ty = ma_cong_ty;
            this.ma_chot = string.Empty;
        }

        public int ma_nhan_vien_kc { get; set; }
        public string username { get; set; }
        public string ho_va_ten_nhan_vien { get; set; }
        public string password { get; set; }
        public string don_vi { get; set; }
        public bool is_kcn { get; set; }
        public bool is_cong_ty { get; set; }
        public bool is_lanh_dao { get; set; }
        public bool is_chot { get; set; }
        public int ma_cong_ty { get; set; }
        public int ma_kcn { get; set; }
        public string ma_chot { get; set; }
        public int ma_huyen { get; set; }
    }
}
