using HslCommunication;
using Operate.Services.PlcConnectService;
using OperateHSL.Core.Caching;
using OperateHSL.Core.Configuration;
using OperateHSL.Core.Infrastructure;
using OperateHSL.Core.Tool;
using OperateHSL.Data.DataModel.ThirdPartModel;
using OperateHSL.Data.PLC;
using OperateHSL.Services.AgvPosition;
using OperateHSL.Services.AgvService;
using OperateHSL.Services.PlcInfoService;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class WeldingRepairLinePerformTask : IScheduledTask
    {
        private readonly ICacheManager _caheManager;
        private readonly IAgvControlService _agvControlService;
        public static string hecinMesUrl = EngineContext.Current.Resolve<mysql>().hecinMesUrl;
        public static RestClient client = new RestClient(hecinMesUrl);
        private readonly IPlcDataService _plcDataSevice;
        private readonly IAgvPositionService _agvPositionService;

        public WeldingRepairLinePerformTask(ICacheManager cacheManager,
            IAgvControlService agvControlService,
            IPlcDataService plcDataService,
            IAgvPositionService agvPositionService
            )
        {
            _caheManager = cacheManager;
            _agvControlService = agvControlService;
            _plcDataSevice = plcDataService;
            _agvPositionService = agvPositionService;
        }

        public int Schedule => 10000;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();
            Console.WriteLine("任务开始处理补焊房的PLC数据" + DateTime.Now);
            checkData.Add("任务开始处理补焊房的PLC数据" + DateTime.Now);
            var listData = new List<PLCTrussData>();
            var resultPlcData = new PLCTrussSingelCompleted();

            var dataInfos = _plcDataSevice.GetCollectionPlcData("011", "1");
            var s7Net = PlcBufferRack.Instance(dataInfos.plc_ip);
            if (s7Net != null)
            {
                var dataLength = Convert.ToUInt16(dataInfos.db_length);
                OperateResult<byte[]> result = s7Net.Read(dataInfos.plc_dbadr + ".0", dataLength);
                var dataInfo = dataInfos.plc_dbInfos;
                if (result.IsSuccess)
                {
                    var recvs = result.Content;
                    //叫料
                    for (int i = 0; i < 12; i++)
                    {
                        var plcPostion = Convert.ToDecimal(dataInfos.plc_dbInfos[i].data_offset.ToString("0.0"));
                        var result1 = Convert.ToBoolean(AnalysisPLCData.GetData(plcPostion, "Bool", recvs));
                        if (result1 == true)
                        {
                            //寻找终点是否闲置
                            var endPositionName = "人工打磨区" + (i + 1);
                            checkData.Add(endPositionName + "申请叫料" + DateTime.Now);
                            var currentTaskCount = await _agvPositionService.GetRepairePositionTaskCount(endPositionName);

                            if (currentTaskCount <= 0)
                            {
                                //寻找空闲点位
                                var endPositionSend = await _agvPositionService.GetRepairePositionData(endPositionName);
                                if (endPositionSend == null)
                                {
                                    checkData.Add(endPositionName + "没有可用叫料点位,或者任务正在处理中" + DateTime.Now);
                                    continue;
                                }
                                else
                                {
                                    //寻找下件区是否有可用物料    
                                    var trayType = 50;
                                    //原来 i=1，3 后来改成 2，3
                                    if (i == 3||i==2)
                                    {
                                        trayType = 60;
                                    }
                                    var workSeat = i + 1;
                                    var structuralEndSend = await _agvPositionService.GetRepaireEndPositionData(trayType, workSeat);
                                    //判断该点位是否有正在执行的任务
                                    if (structuralEndSend == null)
                                    {
                                        Console.WriteLine("缓存区没有放满的物料托盘，或者该工位有任务执行" + DateTime.Now);
                                        checkData.Add("缓存区没有放满的物料托盘，或者该工位有任务执行" + DateTime.Now);

                                        continue;
                                    }
                                    else
                                    {
                                        var isExistAgvTask = _plcDataSevice.PositionIsExistAgvTask(structuralEndSend.PositionCode);
                                        if (isExistAgvTask)
                                        {
                                            Console.WriteLine(structuralEndSend.PositionCode + "该点位有正在执行的AGV任务" + DateTime.Now);
                                            checkData.Add(structuralEndSend.PositionCode + "该点位有正在执行的AGV任务" + DateTime.Now);
                                            continue;
                                        }
                                        else
                                        {
                                            var isExistAgvTask1 = _plcDataSevice.PositionIsExistAgvTask(structuralEndSend.PositionCode);
                                            if (isExistAgvTask1)
                                            {
                                                Console.WriteLine(structuralEndSend.PositionCode + "该点位有正在执行的AGV任务" + DateTime.Now);
                                                checkData.Add(structuralEndSend.PositionCode + "该点位有正在执行的AGV任务" + DateTime.Now);
                                                continue;
                                            }
                                            Random random = new Random();
                                            //生成AGV任务
                                            Third_AgvTask third_AgvTask = new Third_AgvTask()
                                            {
                                                StartStation = structuralEndSend.PositionCode,
                                                Status = 0,
                                                TaskStatus = 0,
                                                TaskType = "补焊房呼叫托盘",
                                                //加1的目的是 在自动化跑的过程中发现生成过相同的TaskNo
                                                TaskNo = $"Trus" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + random.Next(1, 100).ToString(),
                                                EndStation = endPositionSend.PositionCode,
                                                CreateDt = DateTime.Now,
                                                TaskStep = 1,
                                                DataSource = 5,
                                                PlaceHex = i + 1,
                                                ReqCode = DateTime.Now.ToString("yyyyMMddHHmmssfff") + random.Next(1, 100).ToString(),
                                                ReportTicketCode = ""
                                            };
                                            await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);
                                            //修改起始点和终点状态
                                            endPositionSend.PositionState = 3;
                                            endPositionSend.PositionRemark = structuralEndSend.PositionRemark;
                                            endPositionSend.TrayType = structuralEndSend.TrayType;
                                            endPositionSend.UpdateDt = DateTime.Now;
                                            await _agvPositionService.UpdatePositionInfo(endPositionSend);
                                            structuralEndSend.PositionState = 3;
                                            structuralEndSend.UpdateDt = DateTime.Now;
                                            structuralEndSend.TrayData = 0;

                                            await _agvPositionService.UpdatePositionInfo(structuralEndSend);
                                            checkData.Add(endPositionName + " 发送AGV任务叫料成功" + DateTime.Now);
                                        }

                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("该工位有正在执行的AGV任务，请等待" + DateTime.Now);
                                checkData.Add("该工位已经有正在执行的AGV任务，请等待" + DateTime.Now);
                            }
                        }
                        
                    }
                    var dataInfoEnd = _plcDataSevice.GetCollectionPlcData("011", "2");
                    //送料
                    for (int i = 0; i < 12; i++)
                    {
                        var plcPostion = Convert.ToDecimal(dataInfoEnd.plc_dbInfos[i].data_offset.ToString("0.0"));
                        var result1 = Convert.ToBoolean(AnalysisPLCData.GetData(plcPostion, "Bool", recvs));
                        if (result1 == true)
                        {

                            //寻找起点是否满框
                            var startPositionName = "人工打磨区" + (i + 1);
                            checkData.Add(startPositionName + "申请送走料框" + DateTime.Now);
                            var currentTaskCount = await _agvPositionService.GetRepairePositionTaskCount(startPositionName);
                            if (currentTaskCount <= 0)
                            {
                                var startPositionSend = await _agvPositionService.GetRepairePositionEndData(startPositionName);

                                if (startPositionSend == null)
                                {
                                    checkData.Add(startPositionSend + "没有可用送料点位" + DateTime.Now);

                                    continue;
                                }
                                else
                                {
                                    //寻找打磨完成缓存区是否有可用空置点位
                                    var rickingEndSend = await _agvPositionService.GetRickingEndEmtyPositionData();
                                    if (rickingEndSend == null)
                                    {
                                        checkData.Add("上挂区域没有可用空置点位" + DateTime.Now);
                                        continue;
                                    }
                                    else
                                    {
                                        Random random = new Random();
                                        //生成AGV任务
                                        Third_AgvTask third_AgvTask = new Third_AgvTask()
                                        {
                                            StartStation = startPositionSend.PositionCode,
                                            Status = 0,
                                            TaskStatus = 0,
                                            TaskType = "补焊房送走成品",
                                            TaskNo = $"Riking" + DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1,100).ToString(),
                                            EndStation = rickingEndSend.PositionCode,
                                            CreateDt = DateTime.Now,
                                            TaskStep = 1,
                                            DataSource = 6,
                                            PlaceHex = i + 1,
                                            ReqCode = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1, 100).ToString(),
                                            ReportTicketCode = ""
                                        };
                                        await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);

                                        //获取另一侧AGV数据
                                        var otherPositionCode = 0;
                                        var intPositionInfo = Convert.ToInt32(startPositionSend.PositionCode);

                                        var postionName = startPositionSend.PositionName.Split('_')[0];
                                        //if (intPositionInfo % 2 == 0)
                                        //{
                                        //    otherPositionCode = intPositionInfo - 1;
                                        //}
                                        //else
                                        //{
                                        //    otherPositionCode = intPositionInfo + 1;
                                        //}
                                        // var otherDataPosition = await _agvPositionService.GetPositionInfo(otherPositionCode.ToString());
                                        var otherDataPosition = await _agvPositionService.GetDMFAnotherPostion(postionName, startPositionSend.PositionCode);
                                        //置空另一侧数据
                                        otherDataPosition.TrayData = 1;
                                        otherDataPosition.PositionState = 2;
                                        otherDataPosition.UpdateDt = DateTime.Now;
                                        await _agvPositionService.UpdatePositionInfo(otherDataPosition);
                                        //修改起始点和终点状态
                                        startPositionSend.PositionState = 3;
                                        startPositionSend.PositionRemark = otherDataPosition.PositionRemark;
                                        startPositionSend.TrayData = 0;
                                        // startPositionSend.TrayType = 0;
                                        startPositionSend.UpdateDt = DateTime.Now;
                                        await _agvPositionService.UpdatePositionInfo(startPositionSend);
                                        rickingEndSend.PositionState = 3;
                                        rickingEndSend.TrayType = startPositionSend.TrayType;
                                        rickingEndSend.TrayData = 2;
                                        rickingEndSend.PositionRemark = "打磨完工" + otherDataPosition.PositionRemark;
                                        rickingEndSend.UpdateDt = DateTime.Now;
                                        await _agvPositionService.UpdatePositionInfo(rickingEndSend);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("该工位有正在执行的AGV任务，请等待" + DateTime.Now);
                                checkData.Add("该工位有正在执行的AGV任务，请等待" + DateTime.Now);
                            }
                        }
                       
                    }
                }
            }
            Console.WriteLine("任务结束处理补焊房的PLC数据" + DateTime.Now);
            checkData.Add("任务结束处理补焊房的PLC数据" + DateTime.Now);
            _caheManager.Remove("weldingRepairPerform_001_check");
            _caheManager.Set("weldingRepairPerform_001_check", checkData, 1);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);

            //var task = Task.Factory.StartNew(async () =>
            //{
            //    Console.WriteLine("开始处理补焊房叫料" + DateTime.Now);
            //    checkData.Add("开始处理补焊房叫料" + DateTime.Now);
            //    //叫料
            //    var dataInfos = _plcDataSevice.GetCollectionPlcData("011", "1");
            //    var s7Net = PlcBufferRack.Instance(dataInfos.plc_ip);
            //    if (s7Net != null)
            //    {
            //        var dataLength = Convert.ToUInt16(dataInfos.db_length);
            //        OperateResult<byte[]> result = s7Net.Read(dataInfos.plc_dbadr + ".0", dataLength);
            //        var dataInfo = dataInfos.plc_dbInfos;
            //        if (result.IsSuccess)
            //        {
            //            var recvs = result.Content;
            //            for (int i = 0; i < 12; i++)
            //            {
            //                var plcPostion = Convert.ToDecimal(dataInfos.plc_dbInfos[i].data_offset.ToString("0.0"));

            //                var result1 = Convert.ToBoolean(AnalysisPLCData.GetData(plcPostion, "Bool", recvs));
            //                //叫料                                                  
            //                if (result1 == true)
            //                {
            //                    //获取是哪个位置叫的料
            //                    var getPlcoInfo = _plcDataSevice.GetPlcDBInfo(plcPostion);
            //                    if (plcPostion == Convert.ToDecimal(0.0) || plcPostion == Convert.ToDecimal(0.1) || plcPostion == Convert.ToDecimal(0.2)
            //                    || plcPostion == Convert.ToDecimal(0.3))
            //                    {
            //                        //找内拆1、3、5、1212外叉1的料
            //                        List<string> list = new List<string>() { "NC1", "NC3", "NC5", "1212WC1" };
            //                        var agvPostion = _plcDataSevice.GetAgvPositionInfo(list);
            //                        if (agvPostion == null)
            //                        {
            //                            Console.WriteLine($"补焊房:{getPlcoInfo.DataName},下料线没有料托运");
            //                            checkData.Add($"补焊房:{getPlcoInfo.DataName},下料线没有料托运" + DateTime.Now);
            //                        }
            //                        else
            //                        {
            //                            //获取目标点位
            //                            int index = getPlcoInfo.DataName.IndexOf("号");
            //                            string postionName = getPlcoInfo.DataName.Substring(0, index);
            //                            var endstation = _plcDataSevice.GetEndAgvPostion(postionName);
            //                            if (endstation != null)
            //                            {
            //                                //根据终点查询是否有未完成的任务存在 有就不新增
            //                                var booresult = _plcDataSevice.ISExesitAgvTask(endstation);

            //                                if (booresult == true)
            //                                {
            //                                    //新增任务
            //                                    Third_AgvTask third_AgvTask = new Third_AgvTask()
            //                                    {
            //                                        StartStation = agvPostion.PositionCode,
            //                                        Status = 0,
            //                                        TaskStatus = 0,
            //                                        TaskType = "补焊房呼叫托盘",
            //                                        TaskNo = $"TrussesTrayOrder" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //                                        EndStation = endstation,
            //                                        CreateDt = DateTime.Now,
            //                                        TaskStep = 1,
            //                                        DataSource = 5
            //                                    };
            //                                    await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);
            //                                    //修改起始点和目标点位状态
            //                                    // string[] data = new[] { agvPostion.PositionCode, endstation };
            //                                    Dictionary<Third_AgvPosition, string> dic = new Dictionary<Third_AgvPosition, string>();
            //                                    dic.Add(agvPostion, endstation);
            //                                    await _plcDataSevice.modifyAgvPostion(dic);
            //                                    //叫料反馈
            //                                    var callDataInfs = _plcDataSevice.GetCollectionPlcData("011", "3");
            //                                    var address2 = callDataInfs.plc_dbadr + "." + "DBX" + callDataInfs.plc_dbInfos[i].data_offset.ToString("0.0");
            //                                    s7Net.Write(address2, true);

            //                                }
            //                                else
            //                                {
            //                                    Console.WriteLine($"补焊房:{getPlcoInfo.DataName}有未完成的任务");
            //                                    checkData.Add($"补焊房:{getPlcoInfo.DataName}有未完成的任务" + DateTime.Now);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                Console.WriteLine($"补焊房:{getPlcoInfo.DataName}没有空闲的点位可用");
            //                                checkData.Add($"补焊房:{getPlcoInfo.DataName}没有空闲的点位可用" + DateTime.Now);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {

            //                        //获取起始点位
            //                        //排除内拆1、3、5、1212外叉1的料 
            //                        List<string> list = new List<string>() { "NC1", "NC3", "NC5", "1212WC1" };
            //                        var agvPostion = _plcDataSevice.GetRemoveAgvPositionInfo(false);
            //                        if (plcPostion == Convert.ToDecimal(1.2) || plcPostion == Convert.ToDecimal(1.3))
            //                        {
            //                            //找单叉的物料
            //                            agvPostion = _plcDataSevice.GetRemoveAgvPositionInfo(true);
            //                        }

            //                        if (agvPostion != null)
            //                        {
            //                            //获取目标点位
            //                            int index = getPlcoInfo.DataName.IndexOf("号");
            //                            string postionName = getPlcoInfo.DataName.Substring(0, index);
            //                            var endstation = _plcDataSevice.GetEndAgvPostion(postionName);
            //                            if (endstation != null)
            //                            {
            //                                var booresult = _plcDataSevice.ISExesitAgvTask(endstation);
            //                                if (booresult == true)
            //                                {
            //                                    //新增任务
            //                                    Third_AgvTask third_AgvTask = new Third_AgvTask()
            //                                    {
            //                                        StartStation = agvPostion.PositionCode,
            //                                        Status = 0,
            //                                        TaskStatus = 0,
            //                                        TaskType = "补焊房呼叫托盘",
            //                                        TaskNo = $"TrussesTrayOrder" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            //                                        EndStation = endstation,
            //                                        CreateDt = DateTime.Now,
            //                                        TaskStep = 1,
            //                                        DataSource = 5

            //                                    };
            //                                    await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);
            //                                    //修改起始点和目标点位状态
            //                                    //string[] data = new[] { agvPostion.PositionCode, endstation };
            //                                    Dictionary<Third_AgvPosition, string> dic = new Dictionary<Third_AgvPosition, string>();
            //                                    dic.Add(agvPostion, endstation);
            //                                    await _plcDataSevice.modifyAgvPostion(dic);
            //                                    //叫料反馈
            //                                    var callDataInfs = _plcDataSevice.GetCollectionPlcData("011", "3");
            //                                    var address2 = callDataInfs.plc_dbadr + "." + "DBX" + callDataInfs.plc_dbInfos[i].data_offset.ToString("0.0");
            //                                    s7Net.Write(address2, true);
            //                                }
            //                                else
            //                                {
            //                                    Console.WriteLine($"补焊房:{getPlcoInfo.DataName}有未完成的任务");
            //                                    checkData.Add($"补焊房:{getPlcoInfo.DataName}有未完成的任务" + DateTime.Now);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                Console.WriteLine($"补焊房:{getPlcoInfo.DataName}没有空闲的点位可用");
            //                                checkData.Add($"补焊房:{getPlcoInfo.DataName}没有空闲的点位可用" + DateTime.Now);
            //                            }

            //                        }
            //                        else
            //                        {
            //                            Console.WriteLine($"补焊房:{getPlcoInfo.DataName}叫料,下料线没有料托运");
            //                            checkData.Add($"补焊房:{getPlcoInfo.DataName}叫料,下料线没有料托运" + DateTime.Now);

            //                        }
            //                    }

            //                }
            //            }
            //        }
            //        else
            //        {
            //            //日志记录一下错误数据
            //            Console.WriteLine(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败");
            //            checkData.Add(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //            PlcBufferRack.RemovePlcIp(dataInfos.plc_ip);
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败");
            //        checkData.Add(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //        PlcBufferRack.RemovePlcIp(dataInfos.plc_ip);
            //    }
            //    await Task.Delay(10);
            //});

            //await task;


            //var task1 = Task.Factory.StartNew(async () =>
            //{
            //    Console.WriteLine("开始处理补焊房送料" + DateTime.Now);
            //    checkData.Add("开始处理补焊房送料" + DateTime.Now);
            //    //送料
            //    var dataInfos = _plcDataSevice.GetCollectionPlcData("011", "2");
            //    var s7Net = PlcBufferRack.Instance(dataInfos.plc_ip);
            //    if (s7Net != null)
            //    {
            //        var dataLength = Convert.ToUInt16(dataInfos.db_length);
            //        OperateResult<byte[]> result = s7Net.Read(dataInfos.plc_dbadr + ".0", dataLength);
            //        var dataInfo = dataInfos.plc_dbInfos;
            //        if (result.IsSuccess)
            //        {
            //            var recvs = result.Content;
            //            for (int i = 0; i < 12; i++)
            //            {
            //                var plcPostion = Convert.ToDecimal(dataInfos.plc_dbInfos[i].data_offset.ToString("0.0"));

            //                var result1 = Convert.ToBoolean(AnalysisPLCData.GetData(plcPostion, "Bool", recvs));
            //                //送料

            //                if (result1 == true)
            //                {
            //                    //获取是哪个位置要送料
            //                    var getPlcoInfo = _plcDataSevice.GetPlcDBInfo(plcPostion);

            //                    //查询
            //                    _plcDataSevice.GetFinishArea();
            //                }
            //            }
            //        }
            //        else
            //        {
            //            //日志记录一下错误数据
            //            Console.WriteLine(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败");
            //            checkData.Add(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //            PlcBufferRack.RemovePlcIp(dataInfos.plc_ip);
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败");
            //        checkData.Add(dataInfos.plc_ip + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
            //        PlcBufferRack.RemovePlcIp(dataInfos.plc_ip);
            //    }

            //    var handleData = _caheManager.Get<byte[]>("buhanfangcaiji_001");

            //    if (handleData != null)
            //    {
            //        //叫料
            //        var dataInfs = _plcDataSevice.GetCollectionPlcData("011", "1");

            //        for (int i = 0; i < 12; i++)
            //        {
            //            var ss = Convert.ToDecimal(dataInfs.plc_dbInfos[i].data_offset.ToString("0.0"));
            //            var result1 = Convert.ToBoolean(AnalysisPLCData.GetData(ss, "Bool", handleData));
            //            if (result1 == true)
            //            {
            //                var callDataInfs = _plcDataSevice.GetCollectionPlcData("011", "3");
            //                var address2 = dataInfs.plc_dbadr + ".";
            //                s7Net.Write(address2, true);




            //            }


            //        }


            //    }
            //    else
            //    {
            //        Console.WriteLine("结束处理补焊房数据：plc数据为空" + DateTime.Now);
            //        checkData.Add("结束处理补焊房数据：plc数据为空" + DateTime.Now);
            //    }

            //    await Task.Delay(10);


            //    await Task.Delay(10);
            //});
            //await Task.WhenAll(task1, task2,task3);


        }
    }
}
