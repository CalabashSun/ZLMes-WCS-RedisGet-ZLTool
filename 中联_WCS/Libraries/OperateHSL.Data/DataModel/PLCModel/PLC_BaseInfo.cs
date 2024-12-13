using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.PLCModel
{
    [Table("plc_baseinfo")]
    public class PLC_BaseInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PlcState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PlcIp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PlcArea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PlcName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PlcCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PlcPort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PlcType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Updatername { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UpdaterID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Creatername { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateDt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsDeleted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ass_update_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ass_create_user { get; set; }
    }
}
