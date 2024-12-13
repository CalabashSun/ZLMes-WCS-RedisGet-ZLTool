using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.ThirdMes
{
    /// <summary>
    /// 生产开工/报工
    /// </summary>
    public class TaskReportRequest
    {
        /// <summary>
        /// 任务id
        /// </summary>
        public string taskId { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public string jobNo { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string machineNo { get; set; } = "";
        /// <summary>
        /// 数量
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// 报工类型 0：开工,1：完工
        /// </summary>
        public string reportType { get; set; }
        /// <summary>
        /// 来源 ZJ:中集HYWD:焊研威达SET:思而特WN:威诺WZ:武重DJ:东杰
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string plantNo { get; set; }
        /// <summary>
        /// 工位/工序
        /// </summary>
        public string uloc { get; set; }
    }
}
