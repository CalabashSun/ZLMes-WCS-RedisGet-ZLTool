using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.IMes
{
    public class PLCOrder
    {
        public string OrderNo { get; set; }

        public short ONumber { get; set; }

        public short OProduct { get; set; }

        public short FProduct { get; set; }

        public short FNumber { get; set; }

        public int OrderPlace { get; set; }
    }

    public class OrderCompleted
    {
        public int places { get; set; }

        public string OrderNo { get; set; }
    }
}
