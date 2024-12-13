using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class BankTask : IScheduledTask
    {
        public int Schedule => 2000;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Bank立库读取任务开始" + DateTime.Now);
            Console.WriteLine("Bank立库读取任务结束" + DateTime.Now);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
