using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.PLC
{
    public class PLCTrussData
    {
        /// <summary>
        /// 0:左侧 1：右侧
        /// </summary>
        public int place { get; set; }

        /// <summary>
        /// 数据详情
        /// </summary>
        public List<PLCTrussDataDetail> details { get; set; } = new List<PLCTrussDataDetail>();

    }



    public class PLCTrussDataDetail
    {
        /// <summary>
        /// 第一位 偏移量是0 2 4 4.1 6.0 28  后续偏移量 dataposition*50+数据偏移量
        /// </summary>
        public int dataPosition { get; set; }
        /// <summary>
        /// 料箱最大量
        /// </summary>
        public short stacksMax { get; set; }
        /// <summary>
        /// 料箱当前量
        /// </summary>
        public short stacksNow { get; set; }
        /// <summary>
        /// 料框满箱标识
        /// </summary>
        public bool maxFlag{ get; set; }

        /// <summary>
        /// 是否允许下料
        /// </summary>
        public bool isAllow { get; set; }

        /// <summary>
        /// 当前物料
        /// </summary>
        public string currentMaterial { get; set; }

        /// <summary>
        /// 当前料框编号
        /// </summary>
        public string currentNumber { get; set; }
    }
}
