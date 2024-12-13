using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.ThirdMes
{
    public class DeliveryInfoRequest
    {
        public string delTaskNo { get; set; }

        public string pointName { get; set; }

        public string pointCode { get; set; }

        public string useTime { get; set; }

        public string sortSeqNo { get; set; }

        public string orderNo { get; set; }

        public string deliveryType { get; set; }

        public List<DeliveryInfoDetail> OrderDetail { get; set; } = new List<DeliveryInfoDetail>();
    }

    public class DeliveryInfoDetail
    {
        public string delTaskRowNo { get; set; }

        public string matnr { get; set; }

        public string maktx { get; set; }

        public int qty { get; set; }
    }
}
