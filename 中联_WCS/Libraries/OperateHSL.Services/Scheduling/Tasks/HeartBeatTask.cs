using OperateHSL.Core.Caching;
using OperateHSL.Services.AgvPosition;
using OperateHSL.Services.AgvService;
using OperateHSL.Services.Log4netService;
using OperateHSL.Services.PlcInfoService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class HeartBeatTask : IScheduledTask
    {
        public int Schedule => 9000;
        private readonly ICacheManager _caheManager;
        private readonly IAgvControlService _agvControlService;
        public HeartBeatTask(IPlcDataService plcDataService, ICacheManager cacheManager
            , IAgvControlService agvControlService
            , IAgvPositionService agvPositionService)
        {
            _caheManager = cacheManager;
            _agvControlService = agvControlService;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("桁架下料区光栅信号开始处理" + DateTime.Now);
          //  await _agvControlService.GSScheduleTaskAsync();
            Console.WriteLine("桁架下料区光栅信结束处理" + DateTime.Now);
        }
    }
}
