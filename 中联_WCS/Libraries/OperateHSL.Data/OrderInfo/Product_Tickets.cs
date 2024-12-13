using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.OrderInfo
{
    [Table("Product_Tickets")]
    public class Product_Tickets
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MesJobNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TicketCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime TicketActualStartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TicketName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TicketType { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        public DateTime TicketActualEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime TicketPlanStartTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public int TicketState { get; set; } = 4;
        /// <summary>
        /// 
        /// </summary>
        public decimal TicketCompletedCount { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public DateTime TicketPlanEndTime { get; set; } = DateTime.Now.AddDays(1);
        
        /// <summary>
        /// 
        /// </summary>
        public decimal TicketPlanCount { get; set; }
        
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
        public string ProjectCode { get; set; } = "ZL001";
        /// <summary>
        /// 
        /// </summary>
        public int UrgencyLevel { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; } = "ZL001";
        /// <summary>
        /// 
        /// </summary>
        public int IsTicketStandard { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        public string TicketProductType { get; set; } = "group";
        /// <summary>
        /// 
        /// </summary>
        public string TicketCate { get; set; } = "制造工单";
        /// <summary>
        /// 
        /// </summary>
        public string Prcode { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public int PickingCount { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int PickingState { get; set; } = 0;


        public string uloc { get; set; } = "";

        public string MesProductCode { get; set; } = "";

        public string MesProductDesc { get; set; } = "";

    }
}
