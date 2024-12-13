using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Core.Configuration
{
    public static partial class collection
    {
        public static string orderPlc { get; set; } = "10.99.110.171";

        public static string orderPlcAddress { get; set; } = "DB2";

        public static short orderPlcLength { get; set; } = 1119;
    }


    public static partial class structuralLeft
    {
        public static string structuralPlc { get; set; } = "10.99.109.240";

        public static string structuralPlcAddress { get; set; } = "DB111";

        public static short structuralPlcLength { get; set; } = 1150;
    }

    public static partial class structuralRight
    {
        public static string structuralPlc { get; set; } = "10.99.109.247";

        public static string structuralPlcAddress { get; set; } = "DB111";

        public static short structuralPlcLength { get; set; } = 1150;
    }

    public static partial class structuralLeftCompleted
    {
        public static string completedLetfPlc { get; set; } = "10.99.109.240";

        public static string completedLetfPlcAddress { get; set; } = "DB112";

        public static short completedLetfPlcLength { get; set; } = 72;
    }
    public static partial class structuralRightCompleted
    {
        public static string completedRightPlc { get; set; } = "10.99.109.247";

        public static string completedRightPlcAddress { get; set; } = "DB112";

        public static short completedRightPlcLength { get; set; } = 72;
    }

    public static partial class TeamUpNorthLeft1
    {
        public static string PlcIp { get; set; } = "10.99.108.34";

        public static string PlcAddress { get; set; } = "DB50";

        public static short PlcLength { get; set; } = 68;
    }
    public static partial class TeamUpNorthLeft2
    {
        public static string PlcIp { get; set; } = "10.99.108.46";

        public static string PlcAddress { get; set; } = "DB50";

        public static short PlcLength { get; set; } = 68;
    }
    public static partial class TeamUpNorthLeft3
    {
        public static string PlcIp { get; set; } = "10.99.108.58";

        public static string PlcAddress { get; set; } = "DB50";

        public static short PlcLength { get; set; } = 68;
    }
    public static partial class TeamUpSouthLeft1
    {
        public static string PlcIp { get; set; } = "10.99.108.70";

        public static string PlcAddress { get; set; } = "DB50";

        public static short PlcLength { get; set; } = 68;
    }
    public static partial class TeamUpSouthLeft2
    {
        public static string PlcIp { get; set; } = "10.99.108.82";

        public static string PlcAddress { get; set; } = "DB50";

        public static short PlcLength { get; set; } = 68;
    }
    public static partial class TeamUpSouthLeft3
    {
        public static string PlcIp { get; set; } = "10.99.108.94";

        public static string PlcAddress { get; set; } = "DB50";

        public static short PlcLength { get; set; } = 68;
    }
}
