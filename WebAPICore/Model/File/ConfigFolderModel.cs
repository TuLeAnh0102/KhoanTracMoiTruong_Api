using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Model.File
{
    public class ConfigQuanTracModel
    {
        public string LoaiQuanTrac { get; set; }

        private List<ConfigTramQuanTracModel> _configtramQuanTrac = new List<ConfigTramQuanTracModel>();
        public List<ConfigTramQuanTracModel> ConfigTramQuanTrac { get => _configtramQuanTrac; set => _configtramQuanTrac = value; }

    }

    public class ConfigTramQuanTracModel
    {
        public string IdTram { get; set; }
        public string PathTram { get; set; }
    }
}
