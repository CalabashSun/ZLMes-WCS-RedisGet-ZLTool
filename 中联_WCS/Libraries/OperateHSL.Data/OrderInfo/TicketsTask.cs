using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.OrderInfo
{
    [Table("TicketsTask")]
    public class TicketsTask
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime TaskCompletedTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TaskState { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int TaskSortId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal TaskCompletedCount { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public decimal TaskCount { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int Ass_Product_Tickets { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Updatername { get; set; } = "Api";
        /// <summary>
        /// 
        /// </summary>
        public string UpdaterID { get; set; } = "-1";
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDt { get; set; } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public string Creatername { get; set; } = "Api";
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateDt { get; set; } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public int IsDeleted { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Ass_update_user { get; set; } = -1;
        /// <summary>
        /// 
        /// </summary>
        public int Ass_create_user { get; set; } = -1;
        /// <summary>
        /// 
        /// </summary>
        public string TaskCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TaskLevel { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        public int TaskIsAuto { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public string ParentCode { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public decimal TaskBaseCount { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        public decimal PublishCount { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        public int TaskType { get; set; } = 2;

        /// <summary>
        /// 工序
        /// </summary>
        public string uloc { get; set; }

        public string MesProductCode { get; set; } = "";

        public string MesProductDesc { get; set; } = "";

        public string mesorder { get; set; } = "";

        public string ipccode { get; set; } = "";

        public string FidCode { get; set; } = "";

        public string TaskSortCode { get; set; } = "";
    }
}
