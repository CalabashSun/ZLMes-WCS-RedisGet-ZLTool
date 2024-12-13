using PLCRead.Data.Flitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Data.AutoWelding
{
    public class AutoData
    {
        /// <summary>
        /// 绗架工位 1,2,3
        /// </summary>
        public int WorkSeat { get; set; }

        public string WorkDesc { get; set; }

        /// <summary>
        /// 自动焊工位状态 1:运行中 2：停止 3：故障
        /// </summary>
        public int WorkState { get; set; }

        /// <summary>
        /// 是否自动
        /// </summary>
        public bool isAuto { get; set; }

        public string WarningCode { get; set; } = "";
        /// <summary>
        /// 焊接电流
        /// </summary>
        public float CurrentIntensity { get; set; }

        /// <summary>
        /// 焊接电压
        /// </summary>
        public float VoltageIntensity { get; set; }
        /// <summary>
        /// 焊接速度
        /// </summary>
        public float SpeedIntensity { get; set; }
        /// <summary>
        /// 送丝速度
        /// </summary>
        public float WireFeed { get; set; }
        /// <summary>
        /// 零件号
        /// </summary>
        public string WorkPiece { get; set; } = "";

        /// <summary>
        /// 焊接程序号
        /// </summary>
        public int WorkProgram { get; set; }

        public bool RobotIsAuto { get; set; }

        /// <summary>
        /// 冷却水流量
        /// </summary>
        public float CoolingWaterFlow { get; set; }

        /// <summary>
        /// 保护气流量
        /// </summary>
        public float ShieldingGasFlow { get; set; }
        /// <summary>
        /// 是否正在生产
        /// </summary>
        public bool IsProducting { get; set; }

        /// <summary>
        /// 当前错误码
        /// </summary>
        public List<ErrorCoding> errorCoding { get; set; } = new List<ErrorCoding>();
    }
}
