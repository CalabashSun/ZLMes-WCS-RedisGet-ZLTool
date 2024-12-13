using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.IMes
{
    public class AgvOrder
    {
        /// <summary>
        /// 1-12 1,2 1号切割机的两个缓存位 3,4 2号切割机的两个缓存位...
        /// </summary>
        public int places { get; set; }
        /// <summary>
        /// 当前托盘订单号
        /// </summary>
        public string orderNo { get; set; } 
        /// <summary>
        /// 当前托盘里的物料数量
        /// </summary>
        public short dataCount { get; set; }

        /// <summary>
        /// 当前托盘里的物料类型
        /// </summary>
        public short mType { get; set; } = -1;
    }

    public class AgvOrderCallBack
    {
        /// <summary>
        /// 1-12 1,2 1号切割机的两个缓存位 3,4 2号切割机的两个缓存位...
        /// </summary>
        public int places { get; set; }

        public int state { get; set; } = 2;
    }
}
