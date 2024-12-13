using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.ThirdPartModel
{
    [Table("third_structuralagvplcposition")]
    public  class Third_StructuralAgvPlcPosition
    {
        /// <summary>
        /// 
        /// </summary>
        public string PlcAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AgvAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PlcIpInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}
