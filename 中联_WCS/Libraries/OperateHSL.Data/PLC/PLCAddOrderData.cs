using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.PLC
{
    public class PLCAddOrderData
    {
        /// <summary>
        /// 1#预下发订单号
        /// </summary>
        public string OrderNoOne { get; set; }
        /// <summary>
        /// 1#预下发订单矩形管数量
        /// </summary>
        public short ONumberOne { get; set; }
        /// <summary>
        /// 1#预下发订单原材料型号
        /// </summary>
        public short OProductOne { get; set; }
        /// <summary>
        /// 1#预下发订单产成品型号
        /// </summary>
        public short FProductOne { get; set; }
        /// <summary>
        /// 1#预下发订单产成品数量
        /// </summary>
        public short FNumberOne { get; set; }

        #region 2,3,4,5,6
        public string OrderNoTwo { get; set; }
        public short ONumberTwo { get; set; }
        public short OProductTwo { get; set; }
        public short FProductTwo { get; set; }
        public short FNumberTwo { get; set; }
        public string OrderNoThree { get; set; }
        public short ONumberThree { get; set; }
        public short OProductThree { get; set; }
        public short FProductThree { get; set; }
        public short FNumberThree { get; set; }
        public string OrderNoFour { get; set; }
        public short ONumberFour { get; set; }
        public short OProductFour { get; set; }
        public short FProductFour { get; set; }
        public short FNumberFour { get; set; }
        public string OrderNoFive { get; set; }
        public short ONumberFive { get; set; }
        public short OProductFive { get; set; }
        public short FProductFive { get; set; }
        public short FNumberFive { get; set; }
        public string OrderNoSix { get; set; }
        public short ONumberSix { get; set; }
        public short OProductSix { get; set; }
        public short FProductSix { get; set; }
        public short FNumberSix { get; set; }
        #endregion

        /// <summary>
        /// 1#预下发订单号已经完成下发
        /// </summary>
        public bool OrderSendOne { get; set; }
        #region 2,3,4,5,6
        public bool OrderSendTwo { get; set; }
        public bool OrderSendThree { get; set; }
        public bool OrderSendFour { get; set; }
        public bool OrderSendFive { get; set; }
        public bool OrderSendSix { get; set; }
        #endregion
        /// <summary>
        /// PLC已经收到最新1#预下发订单号
        /// </summary>
        public bool OrderOnePLCAdd { get; set; }

        #region 2,3,4,5,6
        public bool OrderTwoPLCAdd { get; set; }
        public bool OrderThreePLCAdd { get; set; }
        public bool OrderFourPLCAdd { get; set; }
        public bool OrderFivePLCAdd { get; set; }
        public bool OrderSixPLCAdd { get; set; }
        #endregion

        /// <summary>
        /// 1#下发订单号生产情况
        /// </summary>
        public int OrderOnePLCState { get; set; }

        #region 2，3，4，5，6
        public int OrderTwoPLCState { get; set; }
        public int OrderThreePLCState { get; set; }
        public int OrderFourPLCState { get; set; }
        public int OrderFivePLCState { get; set; }
        public int OrderSixPLCState { get; set; }
        #endregion
    }
}
