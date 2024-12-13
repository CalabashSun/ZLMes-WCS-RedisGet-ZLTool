using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.ThirdPartModel
{
    [Table("third_agvtask")]
    public class Third_AgvTask
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
        /// 
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
        /// 数据来源 1:下料线空满转换 2：拣货线空满 3：贴板焊接空满 4：机器组队空满 5.打磨缓存区送到打磨房 6.打磨房送到上挂  7.上挂单送  8：下件补有用框 9：下件补备用框 10：下件区送到打磨缓存区 11：成品缓存到下件区  20:继续执行任务
        /// </summary>
        public int DataSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TrayType { get; set; }

        public string ReqCode { get; set; }

        public int TaskStep { get; set; }

        public int PlaceHex { get; set; }
    }
}
