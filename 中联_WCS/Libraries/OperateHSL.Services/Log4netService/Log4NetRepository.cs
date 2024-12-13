using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Services.Log4netService
{
    public class Log4NetRepository
    {
        public static ILoggerRepository loggerRepository { get; set; }
    }
}
