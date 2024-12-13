using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.ThirdMes
{
    /// <summary>
    /// 生产任务下发
    /// </summary>
    public class MesOrderRequest
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string jobNo { get; set; }
        /// <summary>
        /// 序列号（钢印号）
        /// </summary>
        public string sn { get; set; }
        /// <summary>
        /// 序列号（车号）
        /// </summary>
        public string vin { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string matnr { get; set; }
        /// <summary>
        /// 物料描述（物料名称）
        /// </summary>
        public string maktx { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal qty { get; set; }
        /// <summary>
        /// 工位/工序
        /// </summary>
        public string uloc { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string color { get; set; }
        /// <summary>
        /// 工艺参数
        /// </summary>
        public List<procedureParamsModel> procedureParams { get; set; } = new List<procedureParamsModel>();
        /// <summary>
        /// 物料清单
        /// </summary>
        public List<BomModel> boms { get; set; } = new List<BomModel>();

    }

    public class procedureParamsModel
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string paramName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string paramValue { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }
    }

    public class BomModel
    {
        public string matnr { get; set; }

        public string maktx { get; set; }

        public decimal qty { get; set; }

        public string baseUnit { get; set; }

        public string procedureNo { get; set; }

        public string procedureName { get; set; }
    }
}
