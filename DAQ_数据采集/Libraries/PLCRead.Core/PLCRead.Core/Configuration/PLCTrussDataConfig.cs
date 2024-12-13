using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Core.Configuration
{
    /// <summary>
    /// 绗架
    /// </summary>
    public class PLCTrussDataConfig
    {
        public string PlcIp { get; set; }

        public string plcDesc { get; set; }

        public string PlcAddress { get; set; } = "DB80";

        public short PlcLength { get; set; } = 120;
    }

    public class PLCTruss1DataConfig:PLCTrussDataConfig
    {
        public PLCTruss1DataConfig()
        {
            PlcIp = "10.99.109.247";
            plcDesc = "南侧绗架";
        }    
    }

    public class PLCTruss2DataConfig : PLCTrussDataConfig
    {
        public PLCTruss2DataConfig()
        {
            PlcIp = "10.99.109.240";
            plcDesc = "北侧绗架";
        }
    }
}
