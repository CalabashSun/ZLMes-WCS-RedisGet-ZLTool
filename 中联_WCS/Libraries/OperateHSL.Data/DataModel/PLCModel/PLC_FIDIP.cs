using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.PLCModel
{
    [Table("PLC_FIDIP")]
    public class PLC_FIDIP
    {
        public int IsUse { get; set; }

        public string FidIpAddress { get; set; }

        public string FidAddressDesc { get; set; }

        public string PLCIpAddress { get; set; }

        public int SortId { get; set; }
        [Key]
        public int Id { get; set; }
    }
}
