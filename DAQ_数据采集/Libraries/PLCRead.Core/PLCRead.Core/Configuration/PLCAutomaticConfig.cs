using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Core.Configuration
{
    public class PLCAutomaticConfig
    {
        public string PlcIp { get; set; }

        public int ipcId { get; set; }

        public string plcDesc { get; set; }

        public string PlcAddress { get; set; } = "DB120";

        public short PlcLength { get; set; } = 327;
    }


    public class PLCAutomatic1Config : PLCAutomaticConfig
    {
        public PLCAutomatic1Config()
        {

            PlcIp = "10.99.109.79";
            ipcId = 1;
            plcDesc = "自动焊(北1)B5";
        }
    }

    public class PLCAutomatic2Config : PLCAutomaticConfig
    {
        public PLCAutomatic2Config()
        {

            PlcIp = "10.99.109.73";
            ipcId = 2;
            plcDesc = "自动焊(北2)A9";
        }
    }
    public class PLCAutomatic3Config : PLCAutomaticConfig
    {
        public PLCAutomatic3Config()
        {

            PlcIp = "10.99.109.67";
            ipcId = 3;
            plcDesc = "自动焊(北3)A8";
        }
    }
    public class PLCAutomatic4Config : PLCAutomaticConfig
    {
        public PLCAutomatic4Config()
        {

            PlcIp = "10.99.109.61";
            ipcId = 4;
            plcDesc = "自动焊(北4)B4";
        }
    }
    public class PLCAutomatic5Config : PLCAutomaticConfig
    {
        public PLCAutomatic5Config()
        {

            PlcIp = "10.99.109.55";
            ipcId = 5;
            plcDesc = "自动焊(北5)A7";
        }
    }
    public class PLCAutomatic6Config : PLCAutomaticConfig
    {
        public PLCAutomatic6Config()
        {

            PlcIp = "10.99.109.49";
            ipcId = 6;
            plcDesc = "自动焊(北6)A6";
        }
    }
    public class PLCAutomatic7Config : PLCAutomaticConfig
    {
        public PLCAutomatic7Config()
        {

            PlcIp = "10.99.109.43";
            ipcId = 7;
            plcDesc = "自动焊(北7)B3";
        }
    }
    public class PLCAutomatic8Config : PLCAutomaticConfig
    {
        public PLCAutomatic8Config()
        {

            PlcIp = "10.99.109.37";
            ipcId = 8;
            plcDesc = "自动焊(北8)A5";
        }
    }
    public class PLCAutomatic9Config : PLCAutomaticConfig
    {
        public PLCAutomatic9Config()
        {

            PlcIp = "10.99.109.31";
            ipcId = 9;
            plcDesc = "自动焊(北9)A4";
        }
    }
    public class PLCAutomatic10Config : PLCAutomaticConfig
    {
        public PLCAutomatic10Config()
        {

            PlcIp = "10.99.109.25";
            ipcId = 10;
            plcDesc = "自动焊(北10)B2";
        }
    }
    public class PLCAutomatic11Config : PLCAutomaticConfig
    {
        public PLCAutomatic11Config()
        {

            PlcIp = "10.99.109.19";
            ipcId = 11;
            plcDesc = "自动焊(北11)A3";
        }
    }
    public class PLCAutomatic12Config : PLCAutomaticConfig
    {
        public PLCAutomatic12Config()
        {

            PlcIp = "10.99.109.13";
            ipcId = 12;
            plcDesc = "自动焊(北12)A2";
        }
    }
    public class PLCAutomatic13Config : PLCAutomaticConfig
    {
        public PLCAutomatic13Config()
        {

            PlcIp = "10.99.109.7";
            ipcId = 13;
            plcDesc = "自动焊(北13)B";
        }
    }
    public class PLCAutomatic14Config : PLCAutomaticConfig
    {
        public PLCAutomatic14Config()
        {

            PlcIp = "10.99.109.1";
            ipcId = 14;
            plcDesc = "自动焊(北14)A1";
        }
    }



    public class PLCAutomatic15Config : PLCAutomaticConfig
    {
        public PLCAutomatic15Config()
        {

            PlcIp = "10.99.109.163";
            ipcId = 15;
            plcDesc = "自动焊(南1)B10";
        }
    }
    public class PLCAutomatic16Config : PLCAutomaticConfig
    {
        public PLCAutomatic16Config()
        {

            PlcIp = "10.99.109.157";
            ipcId = 16;
            plcDesc = "自动焊(南2)A18";
        }
    }
    public class PLCAutomatic17Config : PLCAutomaticConfig
    {
        public PLCAutomatic17Config()
        {

            PlcIp = "10.99.109.151";
            ipcId = 17;
            plcDesc = "自动焊(南3)A17";
        }
    }
    public class PLCAutomatic18Config : PLCAutomaticConfig
    {
        public PLCAutomatic18Config()
        {

            PlcIp = "10.99.109.145";
            ipcId = 18;
            plcDesc = "自动焊(南4)B9";
        }
    }
    public class PLCAutomatic19Config : PLCAutomaticConfig
    {
        public PLCAutomatic19Config()
        {

            PlcIp = "10.99.109.139";
            ipcId = 19;
            plcDesc = "自动焊(南5)A16";
        }
    }
    public class PLCAutomatic20Config : PLCAutomaticConfig
    {
        public PLCAutomatic20Config()
        {

            PlcIp = "10.99.109.133";
            ipcId = 20;
            plcDesc = "自动焊(南6)A15";
        }
    }
    public class PLCAutomatic21Config : PLCAutomaticConfig
    {
        public PLCAutomatic21Config()
        {

            PlcIp = "10.99.109.127";
            ipcId = 21;
            plcDesc = "自动焊(南7)B8";
        }
    }
    public class PLCAutomatic22Config : PLCAutomaticConfig
    {
        public PLCAutomatic22Config()
        {

            PlcIp = "10.99.109.121";
            ipcId = 22;
            plcDesc = "自动焊(南8)A14";
        }
    }
    public class PLCAutomatic23Config : PLCAutomaticConfig
    {
        public PLCAutomatic23Config()
        {

            PlcIp = "10.99.109.115";
            ipcId = 23;
            plcDesc = "自动焊(南9)A13";
        }
    }
    public class PLCAutomatic24Config : PLCAutomaticConfig
    {
        public PLCAutomatic24Config()
        {

            PlcIp = "10.99.109.109";
            ipcId = 24;
            plcDesc = "自动焊(南10)B7";
        }
    }
    public class PLCAutomatic25Config : PLCAutomaticConfig
    {
        public PLCAutomatic25Config()
        {

            PlcIp = "10.99.109.107";
            ipcId = 25;
            plcDesc = "自动焊(南11)A12";
        }
    }
    public class PLCAutomatic26Config : PLCAutomaticConfig
    {
        public PLCAutomatic26Config()
        {

            PlcIp = "10.99.109.97";
            ipcId = 26;
            plcDesc = "自动焊(南12)A11";
        }
    }
    public class PLCAutomatic27Config : PLCAutomaticConfig
    {
        public PLCAutomatic27Config()
        {

            PlcIp = "10.99.109.91";
            ipcId = 27;
            plcDesc = "自动焊(南13)B6";
        }
    }
    public class PLCAutomatic28Config : PLCAutomaticConfig
    {
        public PLCAutomatic28Config()
        {

            PlcIp = "10.99.109.85";
            ipcId = 28;
            plcDesc = "自动焊(南14)A10";
        }
    }
}
