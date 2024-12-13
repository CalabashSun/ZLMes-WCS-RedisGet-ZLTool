using HslCommunication;
using PLCRead.Core.Caching;
using PLCRead.Core.Configuration;
using PLCRead.Core.Tool;
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
    public class PLCTrussTask : IScheduledTask
    {
        private IStaticCacheManager _redisManager;

        public PLCTrussTask(IStaticCacheManager redisManager)
        {
            _redisManager = redisManager;
        }

        public int Schedule => 9000;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            var checkData = new List<string>();
            checkData.Add("获取绗架当前生产信息采集任务开始" + DateTime.Now);
            //获取当前plc
            var plcDatas = new List<PLCTrussDataConfig>
            {
                new PLCTruss1DataConfig(),
                new PLCTruss2DataConfig()
            };
            var allTrussData = new List<TrussData>();
            Task taskLoop = Task.Run(() =>
            {
                ParallelLoopResult resultRead = Parallel.ForEach(plcDatas, plcdata =>
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
                            allTrussData.Add(GetTrussData(recvs, plcdata.PlcIp));
                            checkData.Add("成功采集" + plcdata.plcDesc + "的信息化集采数据" + DateTime.Now);
                        }
                        else
                        {
                            //日志记录一下错误数据
                            checkData.Add(plcdata.plcDesc + ":" + plcdata.PlcIp + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                            PlcBufferRack.RemovePlcIp(plcdata.PlcIp);
                        }
                    }
                    else
                    {
                        checkData.Add(plcdata.plcDesc + ":" + plcdata.PlcIp + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                        PlcBufferRack.RemovePlcIp(plcdata.PlcIp);
                    }
                });
            });

            taskLoop.Wait();
            checkData.Add("获取绗架当前生产信息采集任务结束" + DateTime.Now);

            var prefexdata = new string[] { "trussdata" };
            var cacheKeydata = new CacheKey("trussdata_001", prefexdata);
            await _redisManager.SetAsync<List<TrussData>>(cacheKeydata, allTrussData);


            var prefexCheckData = new string[] { "trusscheckdata" };
            var cacheKeyCheckData = new CacheKey("trusscheckdata_001", prefexCheckData);
            await _redisManager.SetAsync<List<string>>(cacheKeyCheckData, checkData);

            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }

        public TrussData GetTrussData(byte[] recvs, string ipaddress)
        {
            var result = new TrussData();
            result.WorkSeat = ipaddress== "10.99.109.240"?1:2;
            result.CurrentCapacity= Convert.ToInt32(AnalysisPLCData.GetData(84, "Int", recvs));

            var robotsInfo = new List<TrussRobot>();
            for (int robotCount = 0; robotCount < 2; robotCount++)
            {
                var robotInfo = new TrussRobot();
                var curXSpeedPosition = 8 + robotCount * 12;
                robotInfo.XSpeed = Convert.ToSingle(AnalysisPLCData.GetData(curXSpeedPosition, "Real", recvs));
                var curYSpeedPosition =12+ robotCount * 12;
                robotInfo.YSpeed = Convert.ToSingle(AnalysisPLCData.GetData(curYSpeedPosition, "Real", recvs));
                var curZSpeedPosition = 12 + robotCount * 12;
                robotInfo.ZSpeed = Convert.ToSingle(AnalysisPLCData.GetData(curZSpeedPosition, "Real", recvs));

                var curXPosition = 32 + robotCount * 12;
                robotInfo.XPosition = Convert.ToSingle(AnalysisPLCData.GetData(curXPosition, "Real", recvs));
                var curYPosition = 36 + robotCount * 12;
                robotInfo.YPosition = Convert.ToSingle(AnalysisPLCData.GetData(curYPosition, "Real", recvs));
                var curZPosition = 40 + robotCount * 12;
                robotInfo.ZPosition = Convert.ToSingle(AnalysisPLCData.GetData(curZPosition, "Real", recvs));

                var curPathPosition = 56 + robotCount * 2;
                robotInfo.PathCode = Convert.ToInt32(AnalysisPLCData.GetData(curPathPosition, "Int", recvs));

                var curXTotalPosition = 60 + robotCount * 12;
                robotInfo.XTotalPath = Convert.ToSingle(AnalysisPLCData.GetData(curXTotalPosition, "Real", recvs));
                var curYTotalPosition = 64 + robotCount * 12;
                robotInfo.YTotalPath = Convert.ToSingle(AnalysisPLCData.GetData(curYTotalPosition, "Real", recvs));
                var curZTotalPosition = 68 + robotCount * 12;
                robotInfo.ZTotalPath = Convert.ToSingle(AnalysisPLCData.GetData(curZTotalPosition, "Real", recvs));
                robotsInfo.Add(robotInfo);

            }
            ///机器人数据
            result.robotInfos = robotsInfo;


            var errorCodes = AnalysisPLCData.GetData(86, "String[32]", recvs);
            if (!string.IsNullOrEmpty(errorCodes)&&errorCodes.Contains("N"))
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
