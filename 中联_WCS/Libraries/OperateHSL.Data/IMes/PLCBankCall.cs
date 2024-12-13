using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.IMes
{
    public class PLCBankCall
    {
        /// <summary>
        /// 0-32000
        /// </summary>
        public short orderNo { get; set; }

        public short pipCount { get; set; }

        public int orderPlace { get; set; }
    }

    public class BankOrderState
    {
        public short orderNo { get; set; }

        public short orderState { get; set; }

        public int orderPlace { get; set; }
    }
}
