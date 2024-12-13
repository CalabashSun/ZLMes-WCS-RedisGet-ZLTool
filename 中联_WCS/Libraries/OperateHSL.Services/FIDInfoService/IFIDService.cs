using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OperateHSL.Services.FIDInfoService
{
    public interface  IFIDService
    {
        string ReadIFidInfo(string ip);

        string ReadIFidInfoByDb(string ip);
    }
}
