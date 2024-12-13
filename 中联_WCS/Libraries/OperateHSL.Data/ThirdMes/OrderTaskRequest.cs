using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.ThirdMes
{
    /// <summary>
    /// 5.2.生产任务查询
    /// </summary>
    public class OrderTaskRequest
    {
        /// <summary>
        /// 序列号（钢印号）
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 工序/工位
        /// </summary>
        public string uloc { get; set; }
    }
}
