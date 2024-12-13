using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.OrderInfo
{
    [Table("BomInfoReplace")]
    public class BomInfoReplace
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Base_UOM_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CombineName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NameCombineName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string parentCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string project_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string project_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal UsageQty { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string procedureName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FinishProductId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string procedureNo { get; set; }
    }
}
