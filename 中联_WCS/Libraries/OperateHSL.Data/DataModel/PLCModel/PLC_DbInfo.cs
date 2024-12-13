using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.PLCModel
{
    [Table("plc_dbinfo")]
    public class PLC_DbInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ReadInterval { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DbAdr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DbState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DbDesc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DbLen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Ass_PLC_BaseInfo { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string DbCode { get; set; }
    }
}
