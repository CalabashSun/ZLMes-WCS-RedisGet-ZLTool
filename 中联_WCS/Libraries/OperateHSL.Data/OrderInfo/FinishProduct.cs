using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.OrderInfo
{
    [Table("finishproduct")]
    public class FinishProduct
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProductState { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public decimal DeliverQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CoId { get; set; } = "ZL";
        /// <summary>
        /// 
        /// </summary>
        public string ProductMaterial { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; } = "ZL001";
        /// <summary>
        /// 
        /// </summary>
        public string ProductDesc { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CoName { get; set; } = "ZL";
        /// <summary>
        /// 
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal RequiredQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProjectCode { get; set; } = "ZL001";

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
        public int Ass_Work_Route { get; set; } = 99;
        /// <summary>
        /// 
        /// </summary>
        public string MaterialCode { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string ProductType { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FinishProductCode { get; set; } = "";

    }
}
