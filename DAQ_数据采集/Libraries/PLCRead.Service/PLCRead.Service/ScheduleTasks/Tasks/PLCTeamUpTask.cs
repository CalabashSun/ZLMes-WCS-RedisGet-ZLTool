using HslCommunication;
using HslCommunication.Profinet.Panasonic.Helper;
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
    public class PLCTeamUpTask : IScheduledTask
    {
        private IStaticCacheManager _redisManager;

        public PLCTeamUpTask(IStaticCacheManager redisManager)
        {
            _redisManager = redisManager;
        }

        public int Schedule => 27000;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            #region 只采集工件型号的  ----------废弃-------------------
            //var checkData = new List<string>();
            //checkData.Add("获取人工组队当前生产信息采集任务开始" + DateTime.Now);
            //var redisSaveDatas = new List<MacheProduct>();
            //var task1 = Task.Factory.StartNew(async () =>
            //{
            //    checkData.Add("北组队左1采集任务开始" + DateTime.Now);
            //    var s7Net = PlcBufferRack.Instance(TeamUpNorthLeft1.PlcIp);
            //    if (s7Net != null)
            //    {
            //        var dataLength = Convert.ToUInt16(TeamUpNorthLeft1.PlcLength);
            //        OperateResult<byte[]> result = s7Net.Read(TeamUpNorthLeft1.PlcAddress + ".0", dataLength);
            //        if (result.IsSuccess)
            //        {
            //            var recvs = result.Content;
            //            var redisSaveData6 = new MacheProduct();
            //            redisSaveData6.ipcId = 6;
            //            var redisSaveData7 = new MacheProduct();
            //            redisSaveData7.ipcId = 7;
            //            //提取plc数据
            //            redisSaveData6.productCode = AnalysisPLCData.GetData(0, "String[32]", recvs);
            //            redisSaveData7.productCode = AnalysisPLCData.GetData(34, "String[32]", recvs);
            //            redisSaveDatas.Add(redisSaveData6);
            //            redisSaveDatas.Add(redisSaveData7);
            //            checkData.Add("北组队左1采集数据成功" + DateTime.Now);
            //        }
            //        else
            //        {
            //            //日志记录一下错误数据
            //            checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //            PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //        }
            //    }
            //    else
            //    {
            //        checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //        PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //    }
            //    await Task.Delay(10);
            //    checkData.Add("北组队左1采集任务结束" + DateTime.Now);
            //});

            //var task2 = Task.Factory.StartNew(async () =>
            //{
            //    checkData.Add("北组队左2采集任务开始" + DateTime.Now);
            //    var s7Net = PlcBufferRack.Instance(TeamUpNorthLeft2.PlcIp);
            //    if (s7Net != null)
            //    {
            //        var dataLength = Convert.ToUInt16(TeamUpNorthLeft2.PlcLength);
            //        OperateResult<byte[]> result = s7Net.Read(TeamUpNorthLeft2.PlcAddress + ".0", dataLength);
            //        if (result.IsSuccess)
            //        {
            //            var recvs = result.Content;
            //            var redisSaveData8 = new MacheProduct();
            //            redisSaveData8.ipcId = 8;
            //            var redisSaveData9 = new MacheProduct();
            //            redisSaveData9.ipcId = 9;
            //            //提取plc数据
            //            redisSaveData8.productCode = AnalysisPLCData.GetData(0, "String[32]", recvs);
            //            redisSaveData9.productCode = AnalysisPLCData.GetData(34, "String[32]", recvs);
            //            redisSaveDatas.Add(redisSaveData8);
            //            redisSaveDatas.Add(redisSaveData9);
            //            checkData.Add("北组队左2采集数据成功" + DateTime.Now);
            //        }
            //        else
            //        {
            //            //日志记录一下错误数据
            //            checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //            PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //        }
            //    }
            //    else
            //    {
            //        checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //        PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //    }
            //    await Task.Delay(10);
            //    checkData.Add("北组队左2采集任务结束" + DateTime.Now);
            //});

            //var task3 = Task.Factory.StartNew(async () =>
            //{
            //    checkData.Add("北组队左3采集任务开始" + DateTime.Now);
            //    var s7Net = PlcBufferRack.Instance(TeamUpNorthLeft3.PlcIp);
            //    if (s7Net != null)
            //    {
            //        var dataLength = Convert.ToUInt16(TeamUpNorthLeft3.PlcLength);
            //        OperateResult<byte[]> result = s7Net.Read(TeamUpNorthLeft3.PlcAddress + ".0", dataLength);
            //        if (result.IsSuccess)
            //        {
            //            var recvs = result.Content;
            //            var redisSaveData10 = new MacheProduct();
            //            redisSaveData10.ipcId = 10;
            //            var redisSaveData11 = new MacheProduct();
            //            redisSaveData11.ipcId = 11;
            //            //提取plc数据
            //            redisSaveData10.productCode = AnalysisPLCData.GetData(0, "String[32]", recvs);
            //            redisSaveData11.productCode = AnalysisPLCData.GetData(34, "String[32]", recvs);
            //            redisSaveDatas.Add(redisSaveData10);
            //            redisSaveDatas.Add(redisSaveData11);
            //            checkData.Add("北组队左3采集数据成功" + DateTime.Now);
            //        }
            //        else
            //        {
            //            //日志记录一下错误数据
            //            checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //            PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //        }
            //    }
            //    else
            //    {
            //        checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //        PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //    }
            //    checkData.Add("北组队左3采集任务结束" + DateTime.Now);
            //    await Task.Delay(10);
            //});

            //var task4 = Task.Factory.StartNew(async () =>
            //{
            //    checkData.Add("南组队左1采集任务开始" + DateTime.Now);
            //    var s7Net = PlcBufferRack.Instance(TeamUpSouthLeft1.PlcIp);
            //    if (s7Net != null)
            //    {
            //        var dataLength = Convert.ToUInt16(TeamUpSouthLeft1.PlcLength);
            //        OperateResult<byte[]> result = s7Net.Read(TeamUpSouthLeft1.PlcAddress + ".0", dataLength);
            //        if (result.IsSuccess)
            //        {
            //            var recvs = result.Content;
            //            var redisSaveData12 = new MacheProduct();
            //            redisSaveData12.ipcId = 12;
            //            var redisSaveData13 = new MacheProduct();
            //            redisSaveData13.ipcId = 13;
            //            //提取plc数据
            //            redisSaveData12.productCode = AnalysisPLCData.GetData(0, "String[32]", recvs);
            //            redisSaveData13.productCode = AnalysisPLCData.GetData(34, "String[32]", recvs);
            //            redisSaveDatas.Add(redisSaveData12);
            //            redisSaveDatas.Add(redisSaveData13);
            //            checkData.Add("南组队左1采集数据成功" + DateTime.Now);
            //        }
            //        else
            //        {
            //            //日志记录一下错误数据
            //            checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //            PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //        }
            //    }
            //    else
            //    {
            //        checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //        PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //    }
            //    checkData.Add("南组队左1采集任务结束" + DateTime.Now);
            //    await Task.Delay(10);
            //});

            //var task5 = Task.Factory.StartNew(async () =>
            //{
            //    checkData.Add("南组队左2采集任务开始" + DateTime.Now);
            //    var s7Net = PlcBufferRack.Instance(TeamUpSouthLeft2.PlcIp);
            //    if (s7Net != null)
            //    {
            //        var dataLength = Convert.ToUInt16(TeamUpSouthLeft2.PlcLength);
            //        OperateResult<byte[]> result = s7Net.Read(TeamUpSouthLeft2.PlcAddress + ".0", dataLength);
            //        if (result.IsSuccess)
            //        {
            //            var recvs = result.Content;
            //            var redisSaveData14 = new MacheProduct();
            //            redisSaveData14.ipcId = 14;
            //            var redisSaveData15 = new MacheProduct();
            //            redisSaveData15.ipcId = 15;
            //            //提取plc数据
            //            redisSaveData14.productCode = AnalysisPLCData.GetData(0, "String[32]", recvs);
            //            redisSaveData15.productCode = AnalysisPLCData.GetData(34, "String[32]", recvs);
            //            redisSaveDatas.Add(redisSaveData14);
            //            redisSaveDatas.Add(redisSaveData15);
            //            checkData.Add("南组队左2采集数据成功" + DateTime.Now);
            //        }
            //        else
            //        {
            //            //日志记录一下错误数据
            //            checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //            PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //        }
            //    }
            //    else
            //    {
            //        checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //        PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //    }
            //    checkData.Add("南组队左2采集任务结束" + DateTime.Now);
            //    await Task.Delay(10);
            //});

            //var task6 = Task.Factory.StartNew(async () =>
            //{
            //    checkData.Add("南组队左3采集任务开始" + DateTime.Now);
            //    var s7Net = PlcBufferRack.Instance(TeamUpSouthLeft3.PlcIp);
            //    if (s7Net != null)
            //    {
            //        var dataLength = Convert.ToUInt16(TeamUpSouthLeft3.PlcLength);
            //        OperateResult<byte[]> result = s7Net.Read(TeamUpSouthLeft3.PlcAddress + ".0", dataLength);
            //        if (result.IsSuccess)
            //        {
            //            var recvs = result.Content;
            //            var redisSaveData16 = new MacheProduct();
            //            redisSaveData16.ipcId = 16;
            //            var redisSaveData17 = new MacheProduct();
            //            redisSaveData17.ipcId = 17;
            //            //提取plc数据
            //            redisSaveData16.productCode = AnalysisPLCData.GetData(0, "String[32]", recvs);
            //            redisSaveData17.productCode = AnalysisPLCData.GetData(34, "String[32]", recvs);
            //            redisSaveDatas.Add(redisSaveData16);
            //            redisSaveDatas.Add(redisSaveData17);
            //            checkData.Add("南组队左3采集数据成功" + DateTime.Now);
            //        }
            //        else
            //        {
            //            //日志记录一下错误数据
            //            checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //            PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //        }
            //    }
            //    else
            //    {
            //        checkData.Add(structuralRight.structuralPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //        PlcBufferRack.RemovePlcIp(structuralRight.structuralPlc);
            //    }
            //    checkData.Add("南组队左3采集任务结束" + DateTime.Now);
            //    await Task.Delay(10);
            //});

            //await Task.WhenAll(task1, task2, task3, task4, task5, task6);
            //var prefex = new string[] { "teamupproduct" };
            //var cacheKey = new CacheKey("teamupproduct_001", prefex);
            //await _redisManager.SetAsync<List<MacheProduct>>(cacheKey, redisSaveDatas);
            //checkData.Add("获取人工组队当前生产信息采集任务结束" + DateTime.Now);
            //var prefexCheckData = new string[] { "teamupcheckdata" };
            //var cacheKeyCheckData = new CacheKey("teamupcheckdata_001", prefex);
            //await _redisManager.SetAsync<List<string>>(cacheKeyCheckData, checkData);
            //await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
            #endregion

            var checkData = new List<string>();
            checkData.Add("获取人工组队当前生产信息采集任务开始" + DateTime.Now);
            //获取当前plc
            var plcDatas = new List<PLCTeamUpDataConfig>
            {
                new PLCTeamUp1DataConfig(),
                new PLCTeamUp2DataConfig(),
                new PLCTeamUp3DataConfig(),
                new PLCTeamUp4DataConfig(),
                new PLCTeamUp5DataConfig(),
                new PLCTeamUp6DataConfig()
            };
            var allTeamUpData = new List<TeamUpData>();
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
                            allTeamUpData.Add(GetTeamUpData(recvs, plcdata.PlcIp));
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
            checkData.Add("获取人工组队当前生产信息采集任务结束" + DateTime.Now);
            //封装当前夹具生产信息
            var redisSaveDatas = new List<MacheProduct>();
            foreach (var data in allTeamUpData)
            {
                var saveData1 = new MacheProduct();
                if (data != null)
                {
                    saveData1.ipcId = data.robotInfos[0].ipcId;
                    saveData1.productCode = data.robotInfos[0].WorkPiece;
                    redisSaveDatas.Add(saveData1);
                    var saveData2 = new MacheProduct();
                    saveData2.ipcId = data.robotInfos[1].ipcId;
                    saveData2.productCode = data.robotInfos[1].WorkPiece;
                    redisSaveDatas.Add(saveData2);
                }
            }


            var prefex = new string[] { "teamupproduct" };
            var cacheKey = new CacheKey("teamupproduct_001", prefex);
            await _redisManager.SetAsync<List<MacheProduct>>(cacheKey, redisSaveDatas);

            var prefexdata = new string[] { "teamupdata" };
            var cacheKeydata = new CacheKey("teamupdata_001", prefexdata);
            await _redisManager.SetAsync<List<TeamUpData>>(cacheKeydata, allTeamUpData);


            var prefexCheckData = new string[] { "teamupcheckdata" };
            var cacheKeyCheckData = new CacheKey("teamupcheckdata_001", prefexCheckData);
            await _redisManager.SetAsync<List<string>>(cacheKeyCheckData, checkData);

            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }

        public TeamUpData GetTeamUpData(byte[] recvs,string ipaddress)
        {
            var result = new TeamUpData();
            result.WorkState = Convert.ToInt32(AnalysisPLCData.GetData(0, "Byte", recvs));
            result.ErrorType = Convert.ToInt32(AnalysisPLCData.GetData(1, "Byte", recvs));
            var isAuto = AnalysisPLCData.GetData((decimal)2.1, "Bool", recvs);
            result.isAuto = isAuto == "True" ? true : false;
            var robotsInfo = new List<TeamUpRobot>();
            for (int robotCount = 0; robotCount < 2; robotCount++)
            {
                var robotInfo = new TeamUpRobot();
                //电流
                var curPosition = 4 + (robotCount * 4);
                robotInfo.CurrentIntensity = Convert.ToSingle(AnalysisPLCData.GetData(curPosition, "Real", recvs));
                //电压
                var volPosition = 12 + (robotCount * 4);
                robotInfo.VoltageIntensity = Convert.ToSingle(AnalysisPLCData.GetData(volPosition, "Real", recvs));
                //焊接速度
                var speedPosition = 20 + (robotCount * 4);
                robotInfo.SpeedIntensity = Convert.ToSingle(AnalysisPLCData.GetData(speedPosition, "Real", recvs));
                //送丝速度
                var wirePosition = 28 + (robotCount * 4);
                robotInfo.WireFeed = Convert.ToSingle(AnalysisPLCData.GetData(wirePosition, "Real", recvs));
                //当前生产的工件
                var piecePosition = 36 + (robotCount * 256);
                robotInfo.WorkPiece = AnalysisPLCData.GetData(piecePosition, "String[256]", recvs);
                var autoPosition = (decimal)548.1 + (robotCount * 2);
                robotInfo.RobotIsAuto = Convert.ToBoolean(AnalysisPLCData.GetData(autoPosition, "Bool", recvs));

                var productingPosition = 606 + (robotCount * 1);
                robotInfo.IsProducting = AnalysisPLCData.GetData(productingPosition, "Byte", recvs) == "1" ? true : false;

                //处理ipc数据
                if (ipaddress == "10.99.108.34")
                {
                    result.WorkSeat = 1;
                    robotInfo.ipcId = robotCount == 1 ? 6 : 7;
                }
                if (ipaddress == "10.99.108.46")
                {
                    result.WorkSeat = 2;
                    robotInfo.ipcId = robotCount == 1 ? 8 : 9;
                }
                if (ipaddress == "10.99.108.58")
                {
                    result.WorkSeat = 3;
                    robotInfo.ipcId = robotCount == 1 ? 10: 11;
                }
                if (ipaddress == "10.99.108.70")
                {
                    result.WorkSeat = 4;
                    robotInfo.ipcId = robotCount == 1 ? 12 : 13;
                }
                if (ipaddress == "10.99.108.82")
                {
                    result.WorkSeat =5;
                    robotInfo.ipcId = robotCount == 1 ? 14 : 15;
                }
                if (ipaddress == "10.99.108.94")
                {
                    result.WorkSeat = 6;
                    robotInfo.ipcId = robotCount == 1 ? 16 : 17;
                }
                robotsInfo.Add(robotInfo);

            }
            ///机器人数据
            result.robotInfos = robotsInfo;

            var flowsInfo = new List<WaterFlowCheck>();
            for (int flowCount = 0; flowCount <2; flowCount++)
            {
                var flowInfo = new WaterFlowCheck();
                var onePosition = 590 + (flowCount * 4);
                flowInfo.CoolingWaterFlow = Convert.ToSingle(AnalysisPLCData.GetData(onePosition, "Real", recvs));
                var twoPosition = 598 + (flowCount * 4);
                flowInfo.ShieldingGasFlow = Convert.ToSingle(AnalysisPLCData.GetData(twoPosition, "Real", recvs));

                flowsInfo.Add(flowInfo);
            }

            ///水流数据
            result.flowCheck = flowsInfo;


            var errorCodes = AnalysisPLCData.GetData(556, "String[32]", recvs);
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
