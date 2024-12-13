using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.Bank
{
     public  class StockInfo
    {
        public int OID { get; set; }

        public string StockNum { get; set; }

        public int Quantity { get; set; }

        public int StockType { get; set; }

        public DateTime ReadTime { get; set; }

        public string Remark { get; set; }
    }
}
