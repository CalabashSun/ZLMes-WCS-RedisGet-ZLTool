using HslCommunication;
using PLCRead.Core.Caching;
using PLCRead.Core.Configuration;
using PLCRead.Core.Tool;
using PLCRead.Data.Structural;
using PLCRead.Data.TeamUp;
using PLCRead.Service.PlcConnectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Service.ScheduleTasks.Tasks
{
    public class PLCStructuralEndTask : IScheduledTask
    {
        private IStaticCacheManager _redisManager;

        public PLCStructuralEndTask(IStaticCacheManager redisManager)
        {
            _redisManager = redisManager;
        }

        public int Schedule => 11000;
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();
            checkData.Add("结构线下料plc采集任务开始" + DateTime.Now);
            var task1 = Task.Factory.StartNew(async () =>
            {
                checkData.Add("结构线下料北侧采集任务开始" + DateTime.Now);
                var s7Net = PlcBufferRack.Instance(structuralLeftCompleted.completedLetfPlc);
                if (s7Net != null)
                {
                    var dataLength = Convert.ToUInt16(structuralLeftCompleted.completedLetfPlcLength);
                    OperateResult<byte[]> result = s7Net.Read(structuralLeftCompleted.completedLetfPlcAddress + ".0", dataLength);
                    if (result.IsSuccess)
                    {
                        var recvs = result.Content;
                        //处理plc数据
                        var northEnd = new ProductNorthEnd();
                        northEnd.NorthFirstState = Convert.ToInt16(AnalysisPLCData.GetData(0, "Int", recvs));
                        northEnd.NorthFirstRfid = AnalysisPLCData.GetData(2, "String[32]", recvs);
                        northEnd.NorthSecondState = Convert.ToInt16(AnalysisPLCData.GetData(36, "Int", recvs));
                        northEnd.NorthSecondRfid = AnalysisPLCData.GetData(38, "String[32]", recvs);
                        var prefex = new string[] { "xialiaonorthresult" };
                        var cacheKey = new CacheKey("xialiaonorthresult_001", prefex);
                        await _redisManager.SetAsync<ProductNorthEnd>(cacheKey, northEnd);
                        checkData.Add("结构线下料北侧任务成功" + DateTime.Now);
                    }
                    else
                    {
                        //日志记录一下错误数据
                        checkData.Add(structuralLeft.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                        PlcBufferRack.RemovePlcIp(structuralLeft.structuralPlc);
                    }
                }
                else
                {
                    checkData.Add(structuralLeft.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                    PlcBufferRack.RemovePlcIp(structuralLeft.structuralPlc);
                }
                await Task.Delay(10);
                checkData.Add("结构线下料北侧任务结束" + DateTime.Now);
            });

            var task2 = Task.Factory.StartNew(async () =>
            {
                checkData.Add("结构线下料南侧任务开始" + DateTime.Now);
                var s7Net = PlcBufferRack.Instance(structuralRightCompleted.completedRightPlc);
                if (s7Net != null)
                {
                    var dataLength = Convert.ToUInt16(structuralRightCompleted.completedRightPlcLength);
                    OperateResult<byte[]> result = s7Net.Read(structuralRightCompleted.completedRightPlcAddress + ".0", dataLength);
                    if (result.IsSuccess)
                    {
                        var recvs = result.Content;
                        //处理plc数据
                        var southEnd = new ProductSouthEnd();
                        southEnd.SouthFirstState = Convert.ToInt16(AnalysisPLCData.GetData(0, "Int", recvs));
                        southEnd.SouthFirstRfid = AnalysisPLCData.GetData(2, "String[32]", recvs);
                        southEnd.SouthSecondState = Convert.ToInt16(AnalysisPLCData.GetData(36, "Int", recvs));
                        southEnd.SouthSecondRfid = AnalysisPLCData.GetData(38, "String[32]", recvs);
                        var prefex = new string[] { "xialiaosouthresult" };
                        var cacheKey = new CacheKey("xialiaosouthresult_001", prefex);
                        await _redisManager.SetAsync<ProductSouthEnd>(cacheKey, southEnd);
                        checkData.Add("结构线下料南侧采集任务成功" + DateTime.Now);
                    }
                    else
                    {
                        //日志记录一下错误数据
                        checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                        PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
                    }
                }
                else
                {
                    checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                    PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
                }
                checkData.Add("结构线下料南侧任务结束" + DateTime.Now);
                await Task.Delay(10);
            });

            await Task.WhenAll(task1, task2);
            checkData.Add("结构线下料plc采集任务结束" + DateTime.Now);
            var prefexCheckData = new string[] { "productEndcheckdata" };
            var cacheKeyCheckData = new CacheKey("productEndcheckdata_001", prefexCheckData);
            await _redisManager.SetAsync<List<string>>(cacheKeyCheckData, checkData);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
