using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.ThirdMes
{
    public class DeliverySyncRequest
    {
        public string factoryId { get; set; }

        public string wmsOrderCode { get; set; }

        public string delTaskNo { get; set; }

        public List<DeliverySyncDetail> detailList { get; set; } = new List<DeliverySyncDetail>();
    }

    public class DeliverySyncDetail
    {
        public string wmsOrderLine { get; set; }

        public string delTaskNo { get; set; }

        public string billNo { get; set; }

        public string mrlCode { get; set; }

        public string qty { get; set; }
    }
}
