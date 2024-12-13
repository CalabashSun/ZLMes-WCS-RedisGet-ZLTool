using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Data.Flitch
{
    public class FlitchData
    {
        /// <summary>
        /// 贴板工位 1,2,3
        /// </summary>
        public int WorkSeat { get; set; }
        /// <summary>
        /// 贴板工位状态 1:运行中 2：停止 3：手动 4:自动
        /// </summary>
        public int WorkState { get; set; }
        /// <summary>
        /// 1为停机故障,2为一般故障
        /// </summary>
        public int ErrorType { get; set; }
        /// <summary>
        /// 是否自动
        /// </summary>
        public bool isAuto { get; set; }
        /// <summary>
        /// 工位机器人
        /// </summary>
        public List<FlitchRobot> robotInfos{ get; set; }=new List<FlitchRobot>();
        /// <summary>
        /// 水汽检测
        /// </summary>
        public List<WaterFlowCheck> flowCheck { get; set; }=new List<WaterFlowCheck>();
        
        /// <summary>
        /// 当前错误码
        /// </summary>
        public List<ErrorCoding> errorCoding { get; set; } = new List<ErrorCoding>();
    }

    public class FlitchRobot
    {
        /// <summary>
        /// 机器人状态
        /// </summary>
        public bool RobotIsAuto { get; set;}
        /// <summary>
        /// 报警描述
        /// </summary>
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

        public bool IsProducting { get; set; }

    }

    public class WaterFlowCheck
    {
        /// <summary>
        /// 冷却水流量
        /// </summary>
        public float CoolingWaterFlow { get; set; }

        /// <summary>
        /// 保护气流量
        /// </summary>
        public float ShieldingGasFlow { get; set; }
    }

    public class ErrorCoding
    {
        public int ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
    }
}
