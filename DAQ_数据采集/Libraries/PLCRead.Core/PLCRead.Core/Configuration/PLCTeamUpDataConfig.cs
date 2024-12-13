using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Core.Configuration
{
    public class PLCTeamUpDataConfig
    {
        public  string PlcIp { get; set; }

        public  string plcDesc { get; set; }

        public string PlcAddress { get; set; } = "DB120";

        public short PlcLength { get; set; } = 608;
    }
    public class PLCTeamUp1DataConfig: PLCTeamUpDataConfig
    {
        public PLCTeamUp1DataConfig() {

            PlcIp = "10.99.108.34";
            plcDesc = "组队北侧左1";
        }   
    }
    public class PLCTeamUp2DataConfig : PLCTeamUpDataConfig
    {
        public PLCTeamUp2DataConfig()
        {
            PlcIp = "10.99.108.46";
            plcDesc = "组队北侧左2";
        }
    }
    public class PLCTeamUp3DataConfig : PLCTeamUpDataConfig
    {
        public PLCTeamUp3DataConfig()
        {
            PlcIp = "10.99.108.58";
            plcDesc = "组队北侧左3";
        }
    }
    public class PLCTeamUp4DataConfig : PLCTeamUpDataConfig
    {
        public PLCTeamUp4DataConfig()
        {
            PlcIp = "10.99.108.70";
            plcDesc = "组队南侧左1";
        }
    }
    public class PLCTeamUp5DataConfig : PLCTeamUpDataConfig
    {
        public PLCTeamUp5DataConfig()
        {
            PlcIp = "10.99.108.82";
            plcDesc = "组队南侧左2";
        }
    }
    public class PLCTeamUp6DataConfig : PLCTeamUpDataConfig
    {
        public PLCTeamUp6DataConfig()
        {
            PlcIp = "10.99.108.94";
            plcDesc = "组队南侧左3";
        }
    }
}
