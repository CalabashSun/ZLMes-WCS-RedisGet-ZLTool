using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.PLCModel
{
    [Table("PLC_FidSendRecord")]
    public class PLC_FidSendRecord
    {
        [Key]
        public int Id { get; set; }

        public string IpAddress { get; set; }

        public string SendResult { get; set; }

        public DateTime LastSendTime { get; set; }
    }
}
