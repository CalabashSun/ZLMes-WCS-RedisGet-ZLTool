using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.OrderInfo
{
    [Table("Ticket_Product")]
    public class Ticket_Product
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal PlanCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ass_Product_Tickets { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ass_FinishProduct { get; set; }
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
    }
}
