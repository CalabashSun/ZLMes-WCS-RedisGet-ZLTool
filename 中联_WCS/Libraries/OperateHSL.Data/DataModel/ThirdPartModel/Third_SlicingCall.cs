using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.DataModel.ThirdPartModel
{
    [Table("third_slicingcall")]
    public class Third_SlicingCall
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TicketNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProductType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MaterialQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TikcetRandom { get; set; }

    }
}
