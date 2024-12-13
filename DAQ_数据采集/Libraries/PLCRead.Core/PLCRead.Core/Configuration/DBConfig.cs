using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Core.Configuration
{
    public partial class mysql:IConfig
    {
        public string connection { get; set; }
    }
}
