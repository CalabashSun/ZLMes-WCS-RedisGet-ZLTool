using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLOperateTool.Models
{
    [Table("third_traytype")]
    public class third_traytype
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TypeUse { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TypeArea { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TrayEndPosition { get; set; }
    }
}
