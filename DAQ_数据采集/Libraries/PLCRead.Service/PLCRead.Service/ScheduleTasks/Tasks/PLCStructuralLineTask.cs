using HslCommunication;
using PLCRead.Core.Caching;
using PLCRead.Core.Configuration;
using PLCRead.Core.Tool;
using PLCRead.Data.TeamUp;
using PLCRead.Service.PlcConnectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PLCRead.Service.ScheduleTasks.Tasks
{
    public class PLCStructuralLineTask : IScheduledTask
    {
        private IStaticCacheManager _redisManager;

        public PLCStructuralLineTask(IStaticCacheManager redisManager)
        {
            _redisManager = redisManager;
        }

        public int Schedule => 8000;
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();
            checkData.Add("结构线行架plc采集任务开始" + DateTime.Now);
            var task1 = Task.Factory.StartNew(async () =>
            {
                checkData.Add("行架左侧采集任务开始" + DateTime.Now);
                var s7Net = PlcBufferRack.Instance(structuralLeft.structuralPlc);
                if (s7Net != null)
                {
                    var dataLength = Convert.ToUInt16(structuralLeft.structuralPlcLength);
                    OperateResult<byte[]> result = s7Net.Read(structuralLeft.structuralPlcAddress + ".0", dataLength);
                    if (result.IsSuccess)
                    {
                        var recvs = result.Content;
                        //处理plc数据
                        var completedDataLeft = new List<CompletedMaterial>();
                        for (int i = 0; i < 23; i++)
                        {
                            var dataDetailLeft = new CompletedMaterial();
                            dataDetailLeft.ContainerId = i + 1;
                            var offset1 = Convert.ToDecimal(i * 50 + 0);
                            dataDetailLeft.StacksMax = Convert.ToInt16(AnalysisPLCData.GetData(offset1, "Int", recvs));
                            var offset2 = Convert.ToDecimal(i * 50 + 2);
                            dataDetailLeft.StacksNow = Convert.ToInt16(AnalysisPLCData.GetData(offset2, "Int", recvs));
                            var offset3 = Convert.ToDecimal(i * 50 + 4);
                            dataDetailLeft.MaxFlag = Convert.ToBoolean(AnalysisPLCData.GetData(offset3, "Bool", recvs));
                            var offset4 = Convert.ToDecimal(i * 50 + 4.1);
                            dataDetailLeft.Alone = Convert.ToBoolean(AnalysisPLCData.GetData(offset4, "Bool", recvs));
                            var offset7 = Convert.ToDecimal(i * 50 + 4.2);
                            dataDetailLeft.Allow = Convert.ToBoolean(AnalysisPLCData.GetData(offset7, "Bool", recvs));
                            var offset5 = Convert.ToDecimal(i * 50 + 6);
                            dataDetailLeft.DataModel = AnalysisPLCData.GetData(offset5, "String[20]", recvs);
                            var offset6 = Convert.ToDecimal(i * 50 + 28);
                            var positionNumber = AnalysisPLCData.GetData(offset6, "String[20]", recvs);
                            if (positionNumber.Length >= 4)
                            {
                                positionNumber = positionNumber.Substring(0, 4);
                            }
                            dataDetailLeft.StacksNumber = positionNumber;
                            completedDataLeft.Add(dataDetailLeft);
                        }
                        var prefex = new string[] { "hangjialeftresult" };
                        var cacheKey = new CacheKey("hangjialeftresult_001", prefex);
                        await _redisManager.SetAsync<List<CompletedMaterial>>(cacheKey, completedDataLeft);
                        checkData.Add("行架左侧采集任务成功" + DateTime.Now);
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
                checkData.Add("行架左侧采集任务结束" + DateTime.Now);

                
            });

            var task2 = Task.Factory.StartNew(async () =>
            {
                checkData.Add("行架右侧采集任务开始" + DateTime.Now);
                var s7Net = PlcBufferRack.Instance(structuralRight.structuralPlc);
                if (s7Net != null)
                {
                    var dataLength = Convert.ToUInt16(structuralRight.structuralPlcLength);
                    OperateResult<byte[]> result = s7Net.Read(structuralRight.structuralPlcAddress + ".0", dataLength);
                    if (result.IsSuccess)
                    {
                        var recvs = result.Content;
                        //处理plc数据
                        var completedDataRight = new List<CompletedMaterial>();
                        for (int i = 0; i < 23; i++)
                        {
                            var dataDetailRight = new CompletedMaterial();
                            dataDetailRight.ContainerId = i + 1;
                            var offset1 = Convert.ToDecimal(i * 50 + 0);
                            dataDetailRight.StacksMax = Convert.ToInt16(AnalysisPLCData.GetData(offset1, "Int", recvs));
                            var offset2 = Convert.ToDecimal(i * 50 + 2);
                            dataDetailRight.StacksNow = Convert.ToInt16(AnalysisPLCData.GetData(offset2, "Int", recvs));
                            var offset3 = Convert.ToDecimal(i * 50 + 4);
                            dataDetailRight.MaxFlag = Convert.ToBoolean(AnalysisPLCData.GetData(offset3, "Bool", recvs));
                            var offset4 = Convert.ToDecimal(i * 50 + 4.1);
                            dataDetailRight.Alone = Convert.ToBoolean(AnalysisPLCData.GetData(offset4, "Bool", recvs));
                            var offset7 = Convert.ToDecimal(i * 50 + 4.2);
                            dataDetailRight.Allow = Convert.ToBoolean(AnalysisPLCData.GetData(offset7, "Bool", recvs));
                            var offset5 = Convert.ToDecimal(i * 50 + 6);
                            dataDetailRight.DataModel = AnalysisPLCData.GetData(offset5, "String[20]", recvs);
                            var offset6 = Convert.ToDecimal(i * 50 + 28);
                            var positionNumber = AnalysisPLCData.GetData(offset6, "String[20]", recvs);
                            if (positionNumber.Length >= 4)
                            {
                                positionNumber = positionNumber.Substring(0, 4);
                            }
                            dataDetailRight.StacksNumber = positionNumber;
                            completedDataRight.Add(dataDetailRight);
                        }
                        var prefex = new string[] { "hangjiarightresult" };
                        var cacheKey = new CacheKey("hangjiarightresult_002", prefex);
                        await _redisManager.SetAsync<List<CompletedMaterial>>(cacheKey, completedDataRight);
                        checkData.Add("行架右侧采集任务成功" + DateTime.Now);
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
                checkData.Add("行架右侧采集任务结束" + DateTime.Now);
                await Task.Delay(10);
            });

            await Task.WhenAll(task1, task2);
            checkData.Add("结构线行架plc采集任务结束" + DateTime.Now);
            var prefexCheckData = new string[] { "structuralcheckdata" };
            var cacheKeyCheckData = new CacheKey("structuralcheckdata_001", prefexCheckData);
            await _redisManager.SetAsync<List<string>>(cacheKeyCheckData, checkData);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
