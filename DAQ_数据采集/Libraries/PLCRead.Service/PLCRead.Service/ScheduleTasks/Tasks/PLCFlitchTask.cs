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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Service.ScheduleTasks.Tasks
{
    public class PLCFlitchTask : IScheduledTask
    {
        private IStaticCacheManager _redisManager;

        public PLCFlitchTask(IStaticCacheManager redisManager)
        {
            _redisManager = redisManager;
        }

        public int Schedule => 19000;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();
            checkData.Add("获取贴板工位信息化采集任务开始" + DateTime.Now);
            //获取当前plc
            var plcDatas = new List<PLCFlitchSeatDataConfig>
            {
                new PLCFlitchSeat1DataConfig(),
                new PLCFlitchSeat2DataConfig(),
                new PLCFlitchSeat3DataConfig()
            };

            var allFlitchData = new List<FlitchData>();
            Task taskLoop = Task.Run(() =>
            {
                ParallelLoopResult resultRead = Parallel.ForEach(plcDatas, plcdata =>
                {
                    checkData.Add("开始贴板工位采集" + plcdata.plcDesc + "的信息化集采数据" + DateTime.Now);
                    var s7Net = PlcBufferRack.Instance(plcdata.PlcIp);
                    if (s7Net != null)
                    {
                        var dataLength = Convert.ToUInt16(plcdata.PlcLength);
                        OperateResult<byte[]> result = s7Net.Read(plcdata.PlcAddress + ".0", dataLength);
                        if (result.IsSuccess)
                        {
                            var recvs = result.Content;
                            allFlitchData.Add(GetFlitchData(recvs,plcdata.PlcIp));
                            checkData.Add("成功采集" + plcdata.plcDesc + "的信息化集采数据" + DateTime.Now);
                        } 
                        else
                        {
                            //日志记录一下错误数据
                            checkData.Add(plcdata.PlcIp + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                            PlcBufferRack.RemovePlcIp(plcdata.PlcIp);
                        }
                    }
                    else
                    {
                        checkData.Add(plcdata.PlcIp + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                        PlcBufferRack.RemovePlcIp(plcdata.PlcIp);
                    }

                });
            });

            taskLoop.Wait();
            checkData.Add("获取贴板工位数据信息成功" + DateTime.Now);
            var prefex = new string[] { "flitchdata" };
            var cacheKey = new CacheKey("flitchdata_001", prefex);
            await _redisManager.SetAsync<List<FlitchData>>(cacheKey, allFlitchData);
            
            var prefexCheckData = new string[] { "flitchcheckdata" };
            var cacheKeyCheckData = new CacheKey("flitchcheckdata_001", prefex);
            await _redisManager.SetAsync<List<string>>(cacheKeyCheckData, checkData);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }

        public FlitchData GetFlitchData(byte[] recvs,string ipaddress)
        {
            var result=new FlitchData();
            result.WorkState=Convert.ToInt32(AnalysisPLCData.GetData(0, "Byte", recvs));
            result.ErrorType = Convert.ToInt32(AnalysisPLCData.GetData(1, "Byte", recvs));
            var isAuto=AnalysisPLCData.GetData((decimal)2.1, "Bool", recvs);
            result.isAuto = isAuto == "True" ? true : false;
            var robotsInfo = new List<FlitchRobot>();
            for (int robotCount = 0; robotCount < 6; robotCount++)
            {
                var robotInfo = new FlitchRobot();
                var curPosition = 4 + (robotCount*4);
                robotInfo.CurrentIntensity = Convert.ToSingle(AnalysisPLCData.GetData(curPosition, "Real", recvs));
                var volPosition = 28 + (robotCount * 4);
                robotInfo.VoltageIntensity= Convert.ToSingle(AnalysisPLCData.GetData(volPosition, "Real", recvs));
                var speedPosition= 52 + (robotCount * 4);
                robotInfo.SpeedIntensity= Convert.ToSingle(AnalysisPLCData.GetData(speedPosition, "Real", recvs));
                var wirePosition = 76 + (robotCount * 4);
                robotInfo.WireFeed= Convert.ToSingle(AnalysisPLCData.GetData(wirePosition, "Real", recvs));
                var piecePosition=100+(robotCount * 256);
                robotInfo.WorkPiece = AnalysisPLCData.GetData(piecePosition, "String[256]", recvs);

                var progPosition = 1636 + robotCount;
                robotInfo.WorkProgram=Convert.ToInt32(AnalysisPLCData.GetData(progPosition, "Byte", recvs));
                var autoPosition =(decimal)1642.1 + (robotCount * 2);
                robotInfo.RobotIsAuto = Convert.ToBoolean(AnalysisPLCData.GetData(autoPosition, "Bool", recvs));

                if (robotCount == 0 || robotCount == 1)
                {
                    robotInfo.IsProducting = AnalysisPLCData.GetData(1716, "Byte", recvs)=="1"?true:false;
                }
                else if (robotCount == 2 || robotCount == 3)
                {
                    robotInfo.IsProducting = AnalysisPLCData.GetData(1717, "Byte", recvs) == "1" ? true : false;
                }
                else if (robotCount == 4 || robotCount == 5)
                {
                    robotInfo.IsProducting = AnalysisPLCData.GetData(1718, "Byte", recvs) == "1" ? true : false;
                }
                //处理ipc数据
                if (ipaddress == "10.99.108.1")
                {
                    result.WorkSeat = 1;
                }
                if (ipaddress == "10.99.108.12")
                {
                    result.WorkSeat = 2;
                }
                if (ipaddress == "10.99.108.23")
                {
                    result.WorkSeat = 3;
                }
                robotsInfo.Add(robotInfo); 

            }
            ///机器人数据
            result.robotInfos = robotsInfo;

            var flowsInfo = new List<WaterFlowCheck>();
            for (int flowCount = 0; flowCount < 3; flowCount++)
            {
                var flowInfo = new WaterFlowCheck();
                var onePosition = 1692 + (flowCount * 4);
                flowInfo.CoolingWaterFlow = Convert.ToSingle(AnalysisPLCData.GetData(onePosition, "Real", recvs));
                var twoPosition= 1704+ (flowCount * 4);
                flowInfo.ShieldingGasFlow= Convert.ToSingle(AnalysisPLCData.GetData(twoPosition, "Real", recvs));
                flowsInfo.Add(flowInfo);
            }

            ///水流数据
            result.flowCheck= flowsInfo;

            var errorCodes = AnalysisPLCData.GetData(1658, "String[32]", recvs);
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
