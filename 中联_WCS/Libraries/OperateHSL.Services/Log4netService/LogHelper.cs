using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Services.Log4netService
{
    public class LogHelper
    {
        private static readonly ILog logerror = LogManager.GetLogger(Log4NetRepository.loggerRepository.Name, "errLog");
        private static readonly ILog loginfo = LogManager.GetLogger(Log4NetRepository.loggerRepository.Name, "infoLog");

        public static void Error(string throwMsg, Exception ex)
        {
            string errorMsg = string.Format("【异常描述】：{0} <br>【异常类型】：{1} <br>【异常信息】：{2} <br>【堆栈调用】：{3}",
                new object[] {
                    throwMsg,
                    ex.GetType().Name,
                    ex.Message,
                    ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            logerror.Error(errorMsg);
        }

        public static void Info(string message)
        {
            loginfo.Info(string.Format("【日志信息】：{0}", message));
        }
    }
}
