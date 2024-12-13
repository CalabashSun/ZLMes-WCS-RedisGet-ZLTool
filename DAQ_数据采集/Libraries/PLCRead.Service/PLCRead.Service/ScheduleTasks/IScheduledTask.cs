using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Service.ScheduleTasks
{
    public interface IScheduledTask
    {
        int Schedule { get; }

        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
