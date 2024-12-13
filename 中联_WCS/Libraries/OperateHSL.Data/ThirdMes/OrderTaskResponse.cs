using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.ThirdMes
{
    public class OrderTaskResponse:MesOrderResponse
    {
        public MesOrderRequest data { get; set; } = new MesOrderRequest();
    }

    
}
