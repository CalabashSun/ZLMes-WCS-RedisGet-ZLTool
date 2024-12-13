using OperateHSL.Data.DataModel.ThirdPartModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OperateHSL.Services.LogService
{
    public interface IApiLogService
    {
        void AddApiLogRecord(Third_ApiRecord model);

        void UpdateApiLogRecord(Third_ApiRecord model);

        /// <summary>
        /// 添加切割线叫货记录
        /// </summary>
        /// <param name="model"></param>
        void AddSlicingCallRecord(Third_SlicingCall model);

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        Third_ApiRecord GetLogInfo(int logId);

    }
}
