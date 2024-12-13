using OperateHSL.Services.AgvService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class AGVDispatchTask : IScheduledTask
    {
        public int Schedule => 5000;
        private readonly IAgvControlService _agvControlService;

        public AGVDispatchTask(IAgvControlService agvControlService)
        {
            _agvControlService = agvControlService;
        }

        

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Agv调度任务开始" + DateTime.Now);
            await _agvControlService.AddrequireHAgvScheduleTaskAsync();
            Console.WriteLine("Agv调度任务结束" + DateTime.Now);
            Console.WriteLine("清空库位绑定任务开始" + DateTime.Now);
            await _agvControlService.ClearTrayPosition();
            Console.WriteLine("清空库位绑定任务结束" + DateTime.Now);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
