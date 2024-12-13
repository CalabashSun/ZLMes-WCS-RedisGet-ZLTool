using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.PLC
{
    public class PLCCallProductData
    {
        /// <summary>
        /// 1#立库叫料位是否叫料
        /// </summary>
        public bool BankOnePlcSend { get; set; }
        /// <summary>
        /// 1#立库叫料位产品型号
        /// </summary>
        public int BankOnePlcProduct { get; set; }

        public bool BankTwoPlcSend { get; set; }

        public int BankTwoPlcProduct { get; set; }

        public bool BankThreePlcSend { get; set; }

        public int BankThreePlcProduct { get; set; }

        /// <summary>
        /// 信息化上位机已经向立库叫料PLC确认1
        /// </summary>
        public bool BankPlcConfirmOne { get; set; }

        public bool BankPlcConfirmTwo { get; set; }

        public bool BankPlcConfirmThree { get; set; }

        /// <summary>
        /// 1#立库叫料位执行进度 1未执行  2正在执行   3执行完成
        /// </summary>
        public int BankExcuteStateOne { get; set; }
        /// <summary>
        /// 1#立库叫料位订单号
        /// </summary>
        public int BankOrderOne { get; set; }
        /// <summary>
        /// 1#立库叫料位来料数量
        /// </summary>
        public int BankOrderNumberOne { get; set; }

        #region 2，3
        /// <summary>
        /// 2#立库叫料位执行进度 1未执行  2正在执行   3执行完成
        /// </summary>
        public int BankExcuteStateTwo { get; set; }
        /// <summary>
        /// 2#立库叫料位订单号
        /// </summary>
        public int BankOrderTwo { get; set; }
        /// <summary>
        /// 2#立库叫料位来料数量
        /// </summary>
        public int BankOrderNumberTwo { get; set; }
        /// <summary>
        /// 3#立库叫料位执行进度 1未执行  2正在执行   3执行完成
        /// </summary>
        public int BankExcuteStateThree { get; set; }
        /// <summary>
        /// 3#立库叫料位订单号
        /// </summary>
        public int BankOrderThree { get; set; }
        /// <summary>
        /// 3#立库叫料位来料数量
        /// </summary>
        public int BankOrderNumberThree { get; set; }
        #endregion

        /// <summary>
        /// 1号叫料位信息化已经向立库叫料
        /// </summary>
        public bool BankSendOne { get; set; }

        public bool BankSendTwo { get; set; }

        public bool BankSendThree { get; set; }
    }
}
