using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.PLCModel
{
    [Table("plc_devicedb")]
    public class PLC_DeviceDb
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal DataOffset { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OperateType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DataLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsUse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Ass_Work_device { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ass_PLC_DbInfo { get; set; }
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
        /// <summary>
        /// 对应实体类名称
        /// </summary>
        public string DataModel { get; set; }
    }

}
