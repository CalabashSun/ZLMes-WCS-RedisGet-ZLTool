using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.PLC
{
    public class CompletedMaterial
    {
        public int ContainerId { get; set; }

        public int StacksMax { get; set; }

        public int StacksNow { get; set; }

        public bool MaxFlag { get; set; }

        /// <summary>
        /// 为true为单叉 false正常
        /// </summary>
        public bool Alone { get; set; }

        public bool Allow { get; set; }

        public string DataModel { get; set; } = "";
        /// <summary>
        /// AGV点位
        /// </summary>
        public string StacksNumber { get; set; } = "";


    }
}
