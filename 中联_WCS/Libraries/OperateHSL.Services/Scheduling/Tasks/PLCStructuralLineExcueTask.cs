using Operate.Services.PlcConnectService;
using OperateHSL.Core.Caching;
using OperateHSL.Core.Configuration;
using OperateHSL.Core.Infrastructure;
using OperateHSL.Core.Tool;
using OperateHSL.Data.PLC;
using OperateHSL.Services.AgvService;
using OperateHSL.Services.PlcInfoService;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class PLCStructuralLineExcueTask : IScheduledTask
    {
        private readonly ICacheManager _caheManager;
        private readonly IAgvControlService _agvControlService;
        public static string hecinMesUrl = EngineContext.Current.Resolve<mysql>().hecinMesUrl;
        public static RestClient client = new RestClient(hecinMesUrl);
        private readonly IPlcDataService _plcDataSevice;
        public PLCStructuralLineExcueTask(ICacheManager cacheManager,
            IAgvControlService agvControlService,
            IPlcDataService plcDataService
            )
        {
            _caheManager = cacheManager;
            _agvControlService = agvControlService;
            _plcDataSevice = plcDataService;
        }



        public int Schedule => 7500;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();
            Console.WriteLine("任务开始处理行架的PLC数据" + DateTime.Now);
            checkData.Add("任务开始处理行架的PLC数据" + DateTime.Now);
            var listData = new List<PLCTrussData>();
            var resultPlcData = new PLCTrussSingelCompleted();
            var task3=Task.Factory.StartNew(async ()=>{
                Console.WriteLine("开始处理行架下料数据" + DateTime.Now);
                checkData.Add("开始处理行架下料数据" + DateTime.Now);
                var handleData = _caheManager.Get<byte[]>("hangjiaxialiao_001");
                if (handleData != null)
                {
                    resultPlcData.LIsCompleted = Convert.ToInt16(AnalysisPLCData.GetData(0, "Int", handleData));
                    resultPlcData.LFid = AnalysisPLCData.GetData(2, "String[32]", handleData);
                    resultPlcData.RIsCompleted = Convert.ToInt16(AnalysisPLCData.GetData(36, "Int", handleData));
                    resultPlcData.RFid =AnalysisPLCData.GetData(38, "String[32]", handleData);
                    Console.WriteLine("结束处理行架下料数据，处理成功" + DateTime.Now);
                    checkData.Add("结束处理行架下料数据，数据结果为：LIsCompleted："+ resultPlcData.LIsCompleted
                        + "LFid：" + resultPlcData.LFid
                        + "RIsCompleted：" + resultPlcData.RIsCompleted
                        + "RFid：" + resultPlcData.RFid
                        + DateTime.Now);
                }
                else
                {
                    Console.WriteLine("结束处理行架下料数据：plc数据为空" + DateTime.Now);
                    checkData.Add("结束处理行架下料数据：plc数据为空" + DateTime.Now);
                }

                await Task.Delay(10);


                await Task.Delay(10);
            });


            //await Task.WhenAll(task1, task2,task3);
            await task3;
            //处理list数据
            //foreach (var itemData in listData)
            //{
            //    //寻找需要下料的数据
            //    var needHandleData = itemData.details.Where(p => p.maxFlag == true);
            //    foreach (var itemHandle in needHandleData)
            //    {
            //        if (itemData.place == 0)
            //        {
            //            //左侧 placehex 是-1
            //            //寻找对应的储位
            //            await _agvControlService.CompleteTeamUpPosition(itemHandle.dataPosition * -1, itemHandle.currentMaterial, itemHandle.stacksNow);
            //        }
            //        else
            //        {
            //            //右侧 placehex 是 1
            //            //寻找对应的储位置为可用
            //            await _agvControlService.CompleteTeamUpPosition(itemHandle.dataPosition, itemHandle.currentMaterial, itemHandle.stacksNow);
            //        }
            //    }
            //}


            #region 处理行架下料
            var waithHandleData = new Dictionary<short, string>();
            if (resultPlcData.LIsCompleted == 1)
            {
                waithHandleData.Add(-1, resultPlcData.LFid);
            }
            if (resultPlcData.RIsCompleted == 1)
            {
                waithHandleData.Add(1, resultPlcData.LFid);
            }
            //if (waithHandleData.Count > 0)
            //{
            //    var request = new RestRequest("/TaskCallBack/BankMaterialOut", Method.Post);
            //    request.AddHeader("Content-Type", "application/json");
            //    //var jsonData = JsonConvert.SerializeObject(needCall);
            //    request.AddBody(waithHandleData);
            //    var response = await client.ExecutePostAsync(request);
            //    var result = response.Content;
            //    if (result.Contains("success"))
            //    {
            //        var dataInfos = _plcDataSevice.GetCollectionPlcDb("007");
            //        var s7Net = PlcBufferRack.Instance(dataInfos.plc_ip);
            //        var address = dataInfos.plc_dbadr;
            //        //处理db块
            //        if (waithHandleData.ContainsKey(-1))
            //        {
            //            s7Net.Write(address + ".0.0", (short)2);
            //            s7Net.Write(address + ".2.0","");
            //        }
            //        if (waithHandleData.ContainsKey(1))
            //        {
            //            s7Net.Write(address + ".36.0", (short)2);
            //            s7Net.Write(address + ".38.0", (short)2);
            //        }
            //    }
            //}
            
            #endregion

            Console.WriteLine("任务结束处理行架的PLC数据" + DateTime.Now);
            checkData.Add("任务结束处理行架的PLC数据" + DateTime.Now);
            _caheManager.Remove("strucssexucete_001_check");
            _caheManager.Set("strucssexucete_001_check", checkData, 1);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
