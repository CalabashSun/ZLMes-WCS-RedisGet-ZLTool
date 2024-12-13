using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.PLC
{
    /// <summary>
    /// fid plc信息
    /// </summary>
    public class PLCFIDOne
    {
        public bool isReadFidOne { get; set; }

        public int isSuccessOne { get; set; }

        public string fidNumberOne { get; set; }
    }

    public class PLCFIDTwo:PLCFIDOne
    {
        public bool isReadFidTwo { get; set; }

        public int isSuccessOneTwo { get; set; }

        public string fidNumberOneTwo { get; set; }
    }
}
