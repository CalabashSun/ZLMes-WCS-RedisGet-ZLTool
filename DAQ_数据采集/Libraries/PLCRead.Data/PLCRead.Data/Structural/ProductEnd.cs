using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Data.Structural
{
    public class ProductNorthEnd
    {
        public int NorthFirstState { get; set; }

        public string NorthFirstRfid { get; set; }

        public int NorthSecondState { get; set; }

        public string NorthSecondRfid { get; set; }
    }

    public class ProductSouthEnd
    {
        public int SouthFirstState { get; set; }

        public string SouthFirstRfid { get; set; }

        public int SouthSecondState { get; set; }

        public string SouthSecondRfid { get; set; }
    }
}
