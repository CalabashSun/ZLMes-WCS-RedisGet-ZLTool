using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.PLCModel
{
    [Table("work_device")]
    public class Work_device
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeviceCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Addr2State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Addr1State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProductionState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DeviceState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ToAddr1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ToAddr2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ass_Work_seat { get; set; }
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
