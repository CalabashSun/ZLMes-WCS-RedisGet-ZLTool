using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.Tickets
{
    [Table("TicketTaskRfid")]
    public class TicketTaskRfid
    {
        /// <summary>
        /// 
        /// </summary>
        public int Ass_create_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ass_TicketTaskReport { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ass_update_user { get; set; }
        /// <summary>
        /// 0:未开工 1：已开工 2：回告失败 后续重试使用
        /// </summary>
        public int autoReportState { get; set; }
        /// <summary>
        /// 回告描述
        /// </summary>
        public string autoReportStateDesc { get; set; }
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
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 工控机id
        /// </summary>
        public string ipccode { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        public string uloc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsDeleted { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string maktx { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string matnr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mesorder { get; set; }
        /// <summary>
        /// 0:未开工 1：已开工 2：回告失败 后续重试使用
        /// </summary>
        public int reportstate { get; set; }
        /// <summary>
        /// 回告描述
        /// </summary>
        public string reportStateDesc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rfidcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateDt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UpdaterID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Updatername { get; set; }
    }
}
