using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.ThirdMes
{
    public class EmptyTrayRequest
    {
        /// <summary>
        /// mes单号
        /// </summary>
        public string mesTaskId { get; set; }

        /// <summary>
        /// 起点位置
        /// </summary>
        public string StartLoc { get; set; }

        /// <summary>
        /// 托盘号
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string LotAttr01 { get; set; }

        /// <summary>
        /// 库存地
        /// </summary>
        public string LotAttr02 { get; set; }

        /// <summary>
        /// 载具类型/工装类型
        /// </summary>
        public string Vehicletype { get; set; }
    }

    public class EmptyRickingTrayRequest
    {
        /// <summary>
        /// reqNo
        /// </summary>
        public string reqNo { get; set; }

        /// <summary>
        /// 置空点位
        /// </summary>
        public string positionCode { get; set; }

        /// <summary>
        /// 托盘类型 50 正常托盘 60 单叉托盘
        /// </summary>
        public string trayDataType { get; set; }
    }


    public class CloseWelding
    {

        /// <summary>
        /// -1 全部关闭 1-12
        /// </summary>
        public int position { get; set; }

        /// <summary>
        /// 1 叫料 2 送料
        /// </summary>
        public int type { get; set; }
    }

    public class StructuralSignalHandle
    {

        /// <summary>
        /// agv地址
        /// </summary>
        public string position { get; set; }

        /// <summary>
        /// 1 禁止放料 2 允许放料
        /// </summary>
        public int type { get; set; }
    }
}
