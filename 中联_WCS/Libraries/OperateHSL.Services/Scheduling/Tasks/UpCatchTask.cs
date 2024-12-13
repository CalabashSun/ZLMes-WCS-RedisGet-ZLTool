using OperateHSL.Core.Caching;
using OperateHSL.Data.DataModel.ThirdPartModel;
using OperateHSL.Services.AgvPosition;
using OperateHSL.Services.AgvService;
using OperateHSL.Services.PlcInfoService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class UpCatchTask : IScheduledTask
    {
        public int Schedule => 13000;
        private readonly ICacheManager _caheManager;
        private readonly IAgvControlService _agvControlService;
        private readonly IPlcDataService _plcDataSevice;
        private readonly IAgvPositionService _agvPositionService;

        public UpCatchTask(ICacheManager cacheManager,
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

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();
            Console.WriteLine("上挂任务开始处理" + DateTime.Now);
            checkData.Add("上挂任务开始处理" + DateTime.Now);
            //查找上挂区是否有空闲的点位
            //寻找上挂区是否有可用空置点位
            var rickingEndSend = await _agvPositionService.GetRickingUpCatchPositionData();
            if (rickingEndSend == null)
            {
                checkData.Add("上挂区域没有可用空置点位" + DateTime.Now);
                return;
            }
            else
            {
                //寻找打磨完成缓存区是否有需要上挂的物料，如果有送到上挂区

                var DMComplish = await _agvPositionService.GetDMComplishPositionData();
                if (DMComplish == null)
                {
                    checkData.Add("打磨完成缓存区没有实物托盘" + DateTime.Now);
                    return;
                }
                else
                {
                    Random random = new Random();
                    //生成AGV任务
                    Third_AgvTask third_AgvTask = new Third_AgvTask()
                    {
                        StartStation = DMComplish.PositionCode,
                        Status = 0,
                        TaskStatus = 0,
                        TaskType = "打磨完成缓存区到上挂区",
                        TaskNo = $"DMCToUp" + DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1, 100).ToString(),
                        EndStation = rickingEndSend.PositionCode,
                        CreateDt = DateTime.Now,
                        TaskStep = 1,
                        DataSource = 6,
                        PlaceHex =  1,
                        ReqCode = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1, 100).ToString(),
                        ReportTicketCode = ""
                    };
                    await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);

                    //修改起始点和终点状态
                    DMComplish.PositionState = 3;
                    DMComplish.TrayData = 0;                 
                    DMComplish.UpdateDt = DateTime.Now;
                    await _agvPositionService.UpdatePositionInfo(DMComplish);
                    rickingEndSend.PositionState = 3;
                    rickingEndSend.TrayType = DMComplish.TrayType;
                    rickingEndSend.TrayData = 2;
                    rickingEndSend.PositionRemark = DMComplish.PositionRemark;
                    rickingEndSend.UpdateDt = DateTime.Now;
                    await _agvPositionService.UpdatePositionInfo(rickingEndSend);

                }
            }
            Console.WriteLine("上挂任务结束处理" + DateTime.Now);
            checkData.Add("上挂任务结束处理" + DateTime.Now);
            _caheManager.Remove("UpCatchTaskEnd");
            _caheManager.Set("UpCatchTaskEnd", checkData, 1);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
           
        }
    }
}
