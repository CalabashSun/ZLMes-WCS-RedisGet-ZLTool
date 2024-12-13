using PLCRead.Data.Flitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Data.TeamUp
{
    public class TeamUpData
    {
        /// <summary>
        /// 组队工位 1,2,3
        /// </summary>
        public int WorkSeat { get; set; }
        /// <summary>
        /// 组队工位状态 1:运行中 2：停止 3：故障
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
        public List<TeamUpRobot> robotInfos { get; set; } = new List<TeamUpRobot>();
        /// <summary>
        /// 水汽检测
        /// </summary>
        public List<WaterFlowCheck> flowCheck { get; set; } = new List<WaterFlowCheck>();

        /// <summary>
        /// 当前错误码
        /// </summary>
        public List<ErrorCoding> errorCoding { get; set; } = new List<ErrorCoding>();
    }

    public class TeamUpRobot
    {
        /// <summary>
        /// 机器人状态
        /// </summary>
        public int RobotState { get; set; }

        public bool RobotIsAuto { get; set; }
        /// <summary>
        /// 工控机id
        /// </summary>
        public int ipcId { get; set; }
        /// <summary>
        /// 是否报警
        /// </summary>
        public bool RobotWarningState { get; set; }
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
        /// 零件号
        /// </summary>
        public string WorkPiece { get; set; } = "";

        public float WireFeed { get; set;}
        /// <summary>
        /// 是否正在生产
        /// </summary>
        public bool IsProducting { get; set; }
    }
}
