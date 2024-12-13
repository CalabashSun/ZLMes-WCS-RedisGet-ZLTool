using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.ThirdPartModel
{
    [Table("Third_AgvPosition")]
    public class Third_AgvPosition
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PositionCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PositionInBind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PositionName { get; set; }
        /// <summary>
        /// 1:空闲 2：占用 3：有任务
        /// </summary>
        public int PositionState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TrayCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TrayOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Ass_Third_AgvArea { get; set; }

        /// <summary>
        /// 第二区域
        /// </summary>
        public int AreaPartTwo { get; set; }
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
        /// 0：空置点位 1:矩管托盘 2：拣选托盘 3:贴板托盘  暂时有18种拣选托盘暂定为20-38为拣选托盘 50下件正常托盘 60下件单叉托盘
        /// </summary>
        public int TrayType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PositionTicketId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PositionSort { get; set; }
        /// <summary>
        /// 1:空托盘 2：实物托盘 0：空置位置 3：拣选位占用 4拣选已满 5:自动组队下料中 6:待打磨
        /// </summary>
        public int TrayData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TrayDataMF { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TrayDataMO { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TrayDataCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReqCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TrayOrderType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PositionHex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PositionRemark { get; set; }
        /// <summary>
        /// 托盘物料
        /// </summary>
        public string TrayMaterial { get; set; }
    }
}
