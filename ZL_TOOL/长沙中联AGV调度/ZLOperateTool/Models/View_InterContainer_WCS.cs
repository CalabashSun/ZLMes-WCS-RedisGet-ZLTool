using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLOperateTool.Models
{
    public class View_InterContainer_WCS
    {
        public string StockNum { get; set; }

        public string StockType { get; set; }

        public string DataCode { get; set; }

        public string ProductName { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }

        public DateTime LastActionTime { get; set; }

        public string OutStatus { get; set; }
    }
}
