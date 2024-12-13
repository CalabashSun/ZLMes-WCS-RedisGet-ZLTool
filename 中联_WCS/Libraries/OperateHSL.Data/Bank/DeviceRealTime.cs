using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.Bank
{
    public class DeviceRealTime
    {
        public int OID { get; set; }

        public string DeviceNo { get; set; }

        public int  Quantity { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public int StockType { get; set; }

        /// <summary>
        /// 料框到位，允许卸载料框货物
        /// </summary>
        public int StockReady_Unload { get; set; }

        /// <summary>
        /// 料框回流棍子机允许放置空料框
        /// </summary>
        public int RollerConveyorReady_Stock { get; set; }

        /// <summary>
        /// 下料点空料框取走完成，料框已经离开取料位置
        /// </summary>
        public int Stock_UnloadFinished { get; set; }

        /// <summary>
        /// 料框回流线空料框放置完成
        /// </summary>
        public int StockPlaceFinished { get; set; }

        /// <summary>
        /// 料过程禁止卸载升降链条机运行
        /// </summary>
        public int ForbidRun_UnloadChian { get; set; }

        /// <summary>
        /// 放置空料框过程，禁止回流棍子机运行
        /// </summary>
        public int ForbidRun_loadRoller { get; set; }

        public string FaultCode { get; set; }

        public DateTime ReadTime { get; set; }

        public string Remark { get; set; }
    }
}
