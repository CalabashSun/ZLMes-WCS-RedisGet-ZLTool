using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Core.Configuration
{
    public class PLCElecConfig
    {
        public string PlcIp { get; set; }

        public int ipcId { get; set; }

        public string plcDesc { get; set; }
    }

    public class PLCEFlitch1Config : PLCElecConfig
    {
        public PLCEFlitch1Config()
        {

            PlcIp = "10.99.110.234";
            ipcId = 1;
            plcDesc = "贴板1";
        }
    }

    public class PLCEFlitch2Config : PLCElecConfig
    {
        public PLCEFlitch2Config()
        {

            PlcIp = "10.99.110.235";
            ipcId = 2;
            plcDesc = "贴板2";
        }
    }

    public class PLCEFlitch3Config : PLCElecConfig
    {
        public PLCEFlitch3Config()
        {

            PlcIp = "10.99.110.236";
            ipcId = 3;
            plcDesc = "贴板3";
        }
    }



    public class PLCETeamUp1Config : PLCElecConfig
    {
        public PLCETeamUp1Config()
        {

            PlcIp = "10.99.110.228";
            ipcId = 10;
            plcDesc = "组队北1";
        }
    }

    public class PLCETeamUp2Config : PLCElecConfig
    {
        public PLCETeamUp2Config()
        {

            PlcIp = "10.99.110.229";
            ipcId = 11;
            plcDesc = "组队北2";
        }
    }

    public class PLCETeamUp3Config : PLCElecConfig
    {
        public PLCETeamUp3Config()
        {

            PlcIp = "10.99.110.230";
            ipcId = 12;
            plcDesc = "组队北3";
        }
    }

    public class PLCETeamUp4Config : PLCElecConfig
    {
        public PLCETeamUp4Config()
        {

            PlcIp = "10.99.110.231";
            ipcId = 13;
            plcDesc = "组队南1";
        }
    }

    public class PLCETeamUp5Config : PLCElecConfig
    {
        public PLCETeamUp5Config()
        {

            PlcIp = "10.99.110.232";
            ipcId = 14;
            plcDesc = "组队南2";
        }
    }

    public class PLCETeamUp6Config : PLCElecConfig
    {
        public PLCETeamUp6Config()
        {

            PlcIp = "10.99.110.233";
            ipcId = 15;
            plcDesc = "组队南3";
        }
    }




    public class PLCEAuto1Config : PLCElecConfig
    {
        public PLCEAuto1Config()
        {

            PlcIp = "10.99.110.214";
            ipcId = 50;
            plcDesc = "自动焊北1";
        }
    }

    public class PLCEAuto2Config : PLCElecConfig
    {
        public PLCEAuto2Config()
        {

            PlcIp = "10.99.110.215";
            ipcId = 51;
            plcDesc = "自动焊北2";
        }
    }

    public class PLCEAuto3Config : PLCElecConfig
    {
        public PLCEAuto3Config()
        {

            PlcIp = "10.99.110.216";
            ipcId = 52;
            plcDesc = "自动焊北3";
        }
    }

    public class PLCEAuto4Config : PLCElecConfig
    {
        public PLCEAuto4Config()
        {

            PlcIp = "10.99.110.217";
            ipcId = 53;
            plcDesc = "自动焊北4";
        }
    }

    public class PLCEAuto5Config : PLCElecConfig
    {
        public PLCEAuto5Config()
        {

            PlcIp = "10.99.110.218";
            ipcId = 54;
            plcDesc = "自动焊北5";
        }
    }

    public class PLCEAuto6Config : PLCElecConfig
    {
        public PLCEAuto6Config()
        {

            PlcIp = "10.99.110.219";
            ipcId = 55;
            plcDesc = "自动焊北6";
        }
    }

    public class PLCEAuto7Config : PLCElecConfig
    {
        public PLCEAuto7Config()
        {

            PlcIp = "10.99.110.220";
            ipcId = 56;
            plcDesc = "自动焊北7";
        }
    }

    public class PLCEAuto8Config : PLCElecConfig
    {
        public PLCEAuto8Config()
        {

            PlcIp = "10.99.110.221";
            ipcId = 57;
            plcDesc = "自动焊北8";
        }
    }

    public class PLCEAuto9Config : PLCElecConfig
    {
        public PLCEAuto9Config()
        {

            PlcIp = "10.99.110.222";
            ipcId = 58;
            plcDesc = "自动焊北9";
        }
    }

    public class PLCEAuto10Config : PLCElecConfig
    {
        public PLCEAuto10Config()
        {

            PlcIp = "10.99.110.223";
            ipcId = 59;
            plcDesc = "自动焊北10";
        }
    }

    public class PLCEAuto11Config : PLCElecConfig
    {
        public PLCEAuto11Config()
        {

            PlcIp = "10.99.110.224";
            ipcId = 60;
            plcDesc = "自动焊北11";
        }
    }

    public class PLCEAuto12Config : PLCElecConfig
    {
        public PLCEAuto12Config()
        {

            PlcIp = "10.99.110.225";
            ipcId = 61;
            plcDesc = "自动焊北12";
        }
    }

    public class PLCEAuto13Config : PLCElecConfig
    {
        public PLCEAuto13Config()
        {

            PlcIp = "10.99.110.226";
            ipcId = 62;
            plcDesc = "自动焊北13";
        }
    }

    public class PLCEAuto14Config : PLCElecConfig
    {
        public PLCEAuto14Config()
        {

            PlcIp = "10.99.110.227";
            ipcId = 63;
            plcDesc = "自动焊北14";
        }
    }



    public class PLCEAuto15Config : PLCElecConfig
    {
        public PLCEAuto15Config()
        {

            PlcIp = "10.99.110.200";
            ipcId = 80;
            plcDesc = "自动焊南1";
        }
    }

    public class PLCEAuto16Config : PLCElecConfig
    {
        public PLCEAuto16Config()
        {

            PlcIp = "10.99.110.201";
            ipcId = 81;
            plcDesc = "自动焊南2";
        }
    }

    public class PLCEAuto17Config : PLCElecConfig
    {
        public PLCEAuto17Config()
        {

            PlcIp = "10.99.110.202";
            ipcId = 82;
            plcDesc = "自动焊南3";
        }
    }

    public class PLCEAuto18Config : PLCElecConfig
    {
        public PLCEAuto18Config()
        {

            PlcIp = "10.99.110.203";
            ipcId = 83;
            plcDesc = "自动焊南4";
        }
    }

    

    public class PLCEAuto20Config : PLCElecConfig
    {
        public PLCEAuto20Config()
        {

            PlcIp = "10.99.110.204";
            ipcId = 84;
            plcDesc = "自动焊南5";
        }
    }

    public class PLCEAuto21Config : PLCElecConfig
    {
        public PLCEAuto21Config()
        {

            PlcIp = "10.99.110.205";
            ipcId = 85;
            plcDesc = "自动焊南6";
        }
    }

    public class PLCEAuto22Config : PLCElecConfig
    {
        public PLCEAuto22Config()
        {

            PlcIp = "10.99.110.206";
            ipcId = 86;
            plcDesc = "自动焊南7";
        }
    }

    public class PLCEAuto23Config : PLCElecConfig
    {
        public PLCEAuto23Config()
        {

            PlcIp = "10.99.110.207";
            ipcId = 87;
            plcDesc = "自动焊南8";
        }
    }

    public class PLCEAuto24Config : PLCElecConfig
    {
        public PLCEAuto24Config()
        {

            PlcIp = "10.99.110.208";
            ipcId = 88;
            plcDesc = "自动焊南9";
        }
    }

    public class PLCEAuto25Config : PLCElecConfig
    {
        public PLCEAuto25Config()
        {

            PlcIp = "10.99.110.209";
            ipcId = 89;
            plcDesc = "自动焊南10";
        }
    }

    public class PLCEAuto26Config : PLCElecConfig
    {
        public PLCEAuto26Config()
        {

            PlcIp = "10.99.110.210";
            ipcId = 90;
            plcDesc = "自动焊南11";
        }
    }

    public class PLCEAuto27Config : PLCElecConfig
    {
        public PLCEAuto27Config()
        {

            PlcIp = "10.99.110.211";
            ipcId = 91;
            plcDesc = "自动焊南12";
        }
    }

    public class PLCEAuto28Config : PLCElecConfig
    {
        public PLCEAuto28Config()
        {

            PlcIp = "10.99.110.212";
            ipcId = 92;
            plcDesc = "自动焊南13";
        }
    }

    public class PLCEAuto29Config : PLCElecConfig
    {
        public PLCEAuto29Config()
        {

            PlcIp = "10.99.110.213";
            ipcId = 93;
            plcDesc = "自动焊南14";
        }
    }
}
