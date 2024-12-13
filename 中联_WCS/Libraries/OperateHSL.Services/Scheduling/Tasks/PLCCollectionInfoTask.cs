using HslCommunication;
using Operate.Services.PlcConnectService;
using OperateHSL.Core.Caching;
using OperateHSL.Core.Tool;
using OperateHSL.Data.PLC;
using OperateHSL.Services.PlcInfoService;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    /// <summary>
    /// 获取交互类plc数据
    /// </summary>
    public class PLCCollectionInfoTask : IScheduledTask
    {
        private readonly IPlcDataService _plcDataSevice;
        private readonly ICacheManager _caheManager;
        public PLCCollectionInfoTask(IPlcDataService plcDataService, ICacheManager cacheManager)
        {
            _plcDataSevice = plcDataService;
            _caheManager = cacheManager; 

        }

        public int Schedule => 5000;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();
            Console.WriteLine("下料线plc交互类数据采集任务开始" + DateTime.Now);
            checkData.Add("下料线plc交互类数据采集任务开始" + DateTime.Now);
            //清空下料线交互类数据采集缓存
            var dataInfos = _plcDataSevice.GetCollectionPlcDb("001");
            var s7Net = PlcBufferRack.Instance(dataInfos.plc_ip);
            if (s7Net != null)
            {
                var dataLength = Convert.ToUInt16(dataInfos.db_length);
                OperateResult<byte[]> result = s7Net.Read(dataInfos.plc_dbadr + ".0", dataLength);
                var dataInfo = dataInfos.plc_dbInfos;
                if (result.IsSuccess)
                {
                    var recvs = result.Content;
                    _caheManager.Remove("slingcingresult_001");
                    _caheManager.Set("slingcingresult_001", recvs, 1);
                }
                else
                {
                    //日志记录一下错误数据
                    Console.WriteLine(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败");
                    checkData.Add(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                    PlcBufferRack.RemovePlcIp(dataInfos.plc_ip);
                }
            }
            else
            {
                Console.WriteLine(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败");
                checkData.Add(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                PlcBufferRack.RemovePlcIp(dataInfos.plc_ip);
            }




            Console.WriteLine("下料线plc交互类数据采集任务结束" + DateTime.Now);
            checkData.Add("下料线plc交互类数据采集任务结束" + DateTime.Now);
            _caheManager.Remove("slingcingresult_001_check");
            _caheManager.Set("slingcingresult_001_check", checkData, 1);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
