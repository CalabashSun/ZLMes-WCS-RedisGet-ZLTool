using HslCommunication;
using PLCRead.Core.Caching;
using PLCRead.Core.Configuration;
using PLCRead.Core.Tool;
using PLCRead.Data.AutoWelding;
using PLCRead.Data.Flitch;
using PLCRead.Data.TeamUp;
using PLCRead.Service.PlcConnectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Service.ScheduleTasks.Tasks
{
    public class AutomaticTask : IScheduledTask
    {
        private IStaticCacheManager _redisManager;

        public AutomaticTask(IStaticCacheManager redisManager)
        {
            _redisManager = redisManager;
        }

        public int Schedule => 38000;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            var checkData = new List<string>();
            var errorData=new List<string>();  
            checkData.Add("获取自动焊当前生产信息采集任务开始" + DateTime.Now);
            //获取当前plc
            var plcDatas = new List<PLCAutomaticConfig>
            {
                new PLCAutomatic1Config(),
                new PLCAutomatic2Config(),
                new PLCAutomatic3Config(),
                new PLCAutomatic4Config(),
                new PLCAutomatic5Config(),
                new PLCAutomatic6Config(),
                new PLCAutomatic7Config(),
                new PLCAutomatic8Config(),
                new PLCAutomatic9Config(),
                new PLCAutomatic10Config(),
                new PLCAutomatic11Config(),
                new PLCAutomatic12Config(),
                new PLCAutomatic13Config(),
                new PLCAutomatic14Config(),
                new PLCAutomatic15Config(),
                new PLCAutomatic16Config(),
                new PLCAutomatic17Config(),
                new PLCAutomatic18Config(),
                new PLCAutomatic19Config(),
                new PLCAutomatic20Config(),
                new PLCAutomatic21Config(),
                new PLCAutomatic22Config(),
                new PLCAutomatic23Config(),
                new PLCAutomatic24Config(),
                new PLCAutomatic25Config(),
                new PLCAutomatic26Config(),
                new PLCAutomatic27Config(),
                new PLCAutomatic28Config(),
            };

            var allAutoData = new List<AutoData>();
            Task taskLoop = Task.Run(() =>
            {
                ParallelLoopResult resultRead = Parallel.ForEach(plcDatas, plcdata =>
                {
                    try
                    {
                        checkData.Add("开始采集" + plcdata.plcDesc + "的信息化集采数据" + DateTime.Now);
                        var s7Net = PlcBufferRack.Instance(plcdata.PlcIp);
                        if (s7Net != null)
                        {
                            var dataLength = Convert.ToUInt16(plcdata.PlcLength);
                            OperateResult<byte[]> result = s7Net.Read(plcdata.PlcAddress + ".0", dataLength);
                            if (result.IsSuccess)
                            {
                                var recvs = result.Content;
                                allAutoData.Add(GetAutomaticData(recvs, plcdata.ipcId, plcdata.plcDesc));
                                checkData.Add("成功采集" + plcdata.plcDesc + "的信息化集采数据" + DateTime.Now);
                            }
                            else
                            {
                                errorData.Add(plcdata.plcDesc + ":" + plcdata.PlcIp + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                                //日志记录一下错误数据

                                PlcBufferRack.RemovePlcIp(plcdata.PlcIp);
                            }
                        }
                        else
                        {

                            errorData.Add(plcdata.plcDesc + ":" + plcdata.PlcIp + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                            PlcBufferRack.RemovePlcIp(plcdata.PlcIp);
                        }
                    }
                    catch
                    {
                        errorData.Add(plcdata.plcDesc + ":" + plcdata.PlcIp + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                    }
                });
            });

            taskLoop.Wait();
            checkData.Add("获取人工组队当前生产信息采集任务结束" + DateTime.Now);

            var prefexdata = new string[] { "autodata" };
            var cacheKeydata = new CacheKey("autodata_001", prefexdata);
            await _redisManager.SetAsync<List<AutoData>>(cacheKeydata, allAutoData);


            var prefexCheckData = new string[] { "autocheckdata" };
            var cacheKeyCheckData = new CacheKey("autocheckdata_001", prefexCheckData);
            await _redisManager.SetAsync<List<string>>(cacheKeyCheckData, checkData);

            var prefexErrorData = new string[] { "autoerrordata" };
            var cacheKeyErrorData = new CacheKey("autoerrordata_001", prefexErrorData);
            await _redisManager.SetAsync<List<string>>(cacheKeyErrorData, errorData);

            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }

        public AutoData GetAutomaticData(byte[] recvs, int ipcId,string ipdesc)
        {
            var result = new AutoData();
            result.WorkSeat = ipcId;
            result.WorkDesc = ipdesc;
            result.WorkState = Convert.ToInt32(AnalysisPLCData.GetData(0, "Byte", recvs));
            result.isAuto = Convert.ToBoolean(AnalysisPLCData.GetData((decimal)2.1, "Bool", recvs));
            result.CurrentIntensity= Convert.ToSingle(AnalysisPLCData.GetData(4, "Real", recvs));
            result.VoltageIntensity = Convert.ToSingle(AnalysisPLCData.GetData(8, "Real", recvs));
            result.SpeedIntensity = Convert.ToSingle(AnalysisPLCData.GetData(12, "Real", recvs));
            result.WireFeed = Convert.ToSingle(AnalysisPLCData.GetData(16, "Real", recvs));
            result.WorkPiece = AnalysisPLCData.GetData(20, "String[256]", recvs);
            result.WorkProgram = Convert.ToInt32(AnalysisPLCData.GetData(276, "Byte", recvs));
            result.RobotIsAuto = Convert.ToBoolean(AnalysisPLCData.GetData((decimal)278.1, "Bool", recvs));
            result.IsProducting = AnalysisPLCData.GetData((decimal)326, "Byte", recvs) == "1" ? true : false;

            result.CoolingWaterFlow = Convert.ToSingle(AnalysisPLCData.GetData(318, "Real", recvs));
            result.ShieldingGasFlow = Convert.ToSingle(AnalysisPLCData.GetData(322, "Real", recvs));


            var errorCodes = AnalysisPLCData.GetData(284, "String[32]", recvs);
            if (!string.IsNullOrEmpty(errorCodes) && errorCodes.Contains("N"))
            {
                var erorsTruss = new List<ErrorCoding>();
                int[] numbers = errorCodes.Split('N', StringSplitOptions.RemoveEmptyEntries)
                        .Select(str => str.Trim())
                        .Where(str => !string.IsNullOrEmpty(str))
                        .Select(str => int.Parse(str))
                        .ToArray();
                foreach (var errorItem in numbers)
                {
                    var errorTruss = new ErrorCoding();
                    errorTruss.ErrorCode = errorItem;
                    erorsTruss.Add(errorTruss);
                }
                result.errorCoding = erorsTruss;
            }

            return result;
        }
    }
}
