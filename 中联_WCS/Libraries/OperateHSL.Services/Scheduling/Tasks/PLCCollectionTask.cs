using HslCommunication;
using Newtonsoft.Json;
using Operate.Services.PlcConnectService;
using OperateHSL.Core.Tool;
using OperateHSL.Data.DataModel.ThirdPartModel;
using OperateHSL.Services.AgvPosition;
using OperateHSL.Services.AgvService;
using OperateHSL.Services.Log4netService;
using OperateHSL.Services.PlcInfoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class PLCCollectionTask : IScheduledTask
    {

        private IPlcDataService _plcDataSevice;
        private readonly IAgvPositionService _agvPositionService;
        private readonly IAgvControlService _agvControlService;
        public PLCCollectionTask(IPlcDataService plcDataService, IAgvPositionService agvPositionService, IAgvControlService agvControlService)
        {
            _plcDataSevice = plcDataService;
            _agvPositionService = agvPositionService;
            _agvControlService = agvControlService;

        }
        public int Schedule => 15000;
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("成品区空托盘回流任务开始" + DateTime.Now);

            var checkData = new List<string>();

            checkData.Add("成品区空托盘回流任务开始");
            //查找行架下料缓存区空位置
            var struEmptyTray = await _agvPositionService.GetStructuralEmptyPostion();
            if (struEmptyTray != null)
            {
                //寻找成品区空托盘的点位
                //回流到行架下补框位
                var rickingStartSend = await _agvPositionService.GetRickingEndEmtyBoxPositionData(struEmptyTray.TrayType);
                //if (struEmptyTray.PositionCode == "1184" || struEmptyTray.PositionCode == "1192")
                //{
                //    rickingStartSend = await _agvPositionService.GetRickingEndEmtyBoxPositionData(60);
                //}

                if (rickingStartSend != null)
                {
                    //新增agv任务 
                    //生成AGV任务
                    Third_AgvTask third_AgvTask = new Third_AgvTask()
                    {
                        StartStation = rickingStartSend.PositionCode,
                        Status = 0,
                        TaskStatus = 0,
                        TaskType = "成品缓存区到机器组队缓存区",
                        TaskNo = $"CHToRZH" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        EndStation = struEmptyTray.PositionCode,
                        CreateDt = DateTime.Now,
                        TaskStep = 1,
                        DataSource = 11,
                        PlaceHex = 1,
                        ReqCode = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        ReportTicketCode = "",
                        
                    };
                    await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);
                    checkData.Add($"成品区点位{rickingStartSend.PositionCode}回流到点位{struEmptyTray.PositionCode}");
                    //修改起始点和终点状态
                    struEmptyTray.PositionState = 3;
                    struEmptyTray.PositionRemark = rickingStartSend.PositionRemark;
                    //struEmptyTray.TrayType = rickingStartSend.TrayType;
                    struEmptyTray.TrayData = 1;
                    struEmptyTray.UpdateDt = DateTime.Now;
                    await _agvPositionService.UpdatePositionInfo(struEmptyTray);
                    rickingStartSend.PositionState = 3;
                    rickingStartSend.TrayData = 0;
                    rickingStartSend.TrayType = 0;
                    rickingStartSend.UpdateDt = DateTime.Now;
                    await _agvPositionService.UpdatePositionInfo(rickingStartSend);
                    await _agvControlService.HandleStructuralSignal(struEmptyTray.PositionCode, 7);

                }
                else
                {


                    checkData.Add("成品区没有空料框");
                }
            }
            else
            {
                checkData.Add("机器组队缓存区不需要回流料况");
            }


            Console.WriteLine("成品区空托盘回流任务结束" + DateTime.Now);
            await Task.Delay(TimeSpan.FromMinutes(Schedule), cancellationToken);
        }
    }
}
