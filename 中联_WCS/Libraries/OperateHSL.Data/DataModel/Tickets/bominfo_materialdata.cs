using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.Tickets
{
    [Table("bominfo_materialdata")]
    public class bominfo_materialdata
    {
        /// <summary>
        /// 
        /// </summary>
        public string DataCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataDesc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FidCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MaterialName { get; set; }
    }
}
