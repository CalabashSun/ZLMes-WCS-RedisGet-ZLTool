using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLOperateTool.Models
{
    [Table("third_agvtask")]
    public class third_agvtask
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StartStation { get; set; }
        /// <summary>
        /// 0:新建 1：已下发 2：运行中 3：输送完成
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TaskStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TaskType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TaskNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EndStation { get; set; }

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
        /// 
        /// </summary>
        public string ReportTicketCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DataSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TrayType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AgvCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReqCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PlaceHex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TaskStep { get; set; }
    }
}
