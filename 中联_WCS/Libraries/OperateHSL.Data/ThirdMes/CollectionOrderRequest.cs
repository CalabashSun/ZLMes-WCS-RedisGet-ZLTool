using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.ThirdMes
{
    /// <summary>
    /// 下料线报工
    /// </summary>
    public class CollectionOrderRequest
    {
        /// <summary>
        /// 订单
        /// </summary>
        public string AUFNR { get; set; }
        /// <summary>
        /// 报工日期 yyyyMMdd
        /// </summary>
        public string BGDAT { get; set; }

        public string BWART { get; set; } = "131";
        /// <summary>
        /// 报工时分秒
        /// </summary>
        public string IEDZ { get; set; } = "HHmmss";

        public string LGORT { get; set; }= "9511";
        /// <summary>
        /// 物料编号
        /// </summary>
        public string MATNR { get; set; } = "";
        /// <summary>
        /// 数量
        /// </summary>
        public string MENGE { get; set; } = "";

        /// <summary>
        /// 工厂
        /// </summary>
        public string PLANT { get; set; } = "9301";
        /// <summary>
        /// 报废数量
        /// </summary>
        public string SCRAP { get; set; } = "0";
        /// <summary>
        /// 报工工序
        /// </summary>
        public string VORNR { get; set; } = "S20101";

    }
}
