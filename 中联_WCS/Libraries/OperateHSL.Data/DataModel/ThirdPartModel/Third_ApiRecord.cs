using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.ThirdPartModel
{
    [Table("third_apirecord")]
    public class Third_ApiRecord
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataResult { get; set; }
        /// <summary>
        ///1 请求 2 接收
        /// </summary>
        public string RequestType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TaskNo { get; set; }
        /// <summary>
        ///  1:AGV接口 2：立库接口 3：mes接口 4：光栅接口 5：合信置空上挂托盘 6:自动焊报工MES 7：sn报工MES 8：组队报工MES 
        /// </summary>
        public string DataType { get; set; }

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
