using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.PLC
{
    public class PLCTrussSingelCompleted
    {
        /// <summary>
        /// 1号桁架开始工作，写入1，信息化收到后写2
        /// </summary>
        public int LIsCompleted { get; set; }

        public string LFid { get; set; }

        public int RIsCompleted { get; set; }

        public string RFid { get; set; }
    }
}
