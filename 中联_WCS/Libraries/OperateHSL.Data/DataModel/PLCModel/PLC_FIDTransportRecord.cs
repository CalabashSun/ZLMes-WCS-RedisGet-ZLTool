using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.PLCModel
{
    [Table("PLC_FIDTransportRecord")]
    public class PLC_FIDTransportRecord
    {
        [Key]
        public int Id { get; set; }

        public int SumCount { get; set; }

        public string SumModelData { get; set; }

        public string DbWriteAddress { get; set; }

        public string SumIpAddress { get; set; }

        public string OrginalIpAddress { get; set; }

        public DateTime CreateDt { get; set; } = DateTime.Now;

        public DateTime UpdateDt { get; set; } = DateTime.Now;
    }
}
