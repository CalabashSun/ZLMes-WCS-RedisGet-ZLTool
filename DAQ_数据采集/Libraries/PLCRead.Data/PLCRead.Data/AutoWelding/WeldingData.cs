using PLCRead.Data.TeamUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Data.AutoWelding
{
    public class WeldingData
    {
        /// <summary>
        /// 自动焊工位 1,2,3
        /// </summary>
        public int WorkSeat { get; set; }
        /// <summary>
        /// 自动焊工位状态 1:运行中 2：停止 3：手动 4:自动
        /// </summary>
        public int WorkState { get; set; }
        /// <summary>
        /// 自动焊机器人
        /// </summary>
        public List<WeldingRobot> robotInfos { get; set; } = new List<WeldingRobot>();
    }

    public class WeldingRobot
    {
        /// <summary>
        /// 机器人状态
        /// </summary>
        public int RobotState { get; set; }
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
    }
}
