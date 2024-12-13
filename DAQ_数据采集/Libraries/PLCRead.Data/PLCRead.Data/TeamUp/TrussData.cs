using PLCRead.Data.Flitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Data.TeamUp
{
    public class TrussData
    {
        /// <summary>
        /// 绗架工位 1,2,3
        /// </summary>
        public int WorkSeat { get; set; }

        /// <summary>
        /// 实时产能
        /// </summary>
        public int CurrentCapacity { get; set; }
        /// <summary>
        /// 工位机器人
        /// </summary>
        public List<TrussRobot> robotInfos { get; set; } = new List<TrussRobot>();

        /// <summary>
        /// 当前错误码
        /// </summary>
        public List<ErrorCoding> errorCoding { get; set; } = new List<ErrorCoding>();
    }

    public class TrussRobot
    {
        /// <summary>
        /// 1.手动/2.自动/3.自动运行/4.自动任务中/8.错误
        /// </summary>
        public int WorkState { get; set; }
        /// <summary>
        /// 是否自动
        /// </summary>
        public bool isAuto { get; set; }
        /// <summary>
        /// 绗架 id
        /// </summary>
        public int ipcId { get; set; }
        /// <summary>
        /// x轴速度
        /// </summary>
        public float XSpeed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float YSpeed { get; set; }

        public float ZSpeed { get; set; }

        public float XPosition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float YPosition { get; set; }

        public float ZPosition { get; set; }
        /// <summary>
        /// 路径号
        /// </summary>
        public int PathCode { get; set; }
        /// <summary>
        /// 总行程
        /// </summary>
        public float XTotalPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float YTotalPath { get; set; }

        public float ZTotalPath { get; set; }

    }
}
