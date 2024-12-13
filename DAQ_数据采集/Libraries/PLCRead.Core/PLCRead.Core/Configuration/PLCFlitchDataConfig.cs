using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Core.Configuration
{
    public class PLCFlitchSeatDataConfig
    {
        public  string PlcIp { get; set; }

        public  string plcDesc { get; set; }

        public  string PlcAddress { get; set; } = "DB120";

        public  short PlcLength { get; set; } = 1719;
    }

    public class PLCFlitchSeat1DataConfig: PLCFlitchSeatDataConfig
    {
        public PLCFlitchSeat1DataConfig()
        {
            PlcIp = "10.99.108.1";
            plcDesc = "贴板北侧左1";
        }
    }

    public class PLCFlitchSeat2DataConfig : PLCFlitchSeatDataConfig
    {
        public PLCFlitchSeat2DataConfig()
        {
            PlcIp = "10.99.108.12";
            plcDesc = "贴板北侧左2";
        }
    }

    public class PLCFlitchSeat3DataConfig: PLCFlitchSeatDataConfig
    {
        public PLCFlitchSeat3DataConfig()
        {
            PlcIp = "10.99.108.23";
            plcDesc = "贴板北侧左3";
        }
    }
}
