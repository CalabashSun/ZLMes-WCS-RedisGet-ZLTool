using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.PLCModel
{
    [Table("PLC_FIDReadRecord")]
    public class PLC_FIDReadRecord
    {
        [Key]
        public int Id { get; set; }

        public string IpAddress { get; set; }

        public string ReadResult { get; set; }

        public int State { get; set; }

        public DateTime LastReadTime { get; set; }
    }
}
