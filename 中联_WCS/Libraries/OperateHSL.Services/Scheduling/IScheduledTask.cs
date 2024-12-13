using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling
{
    public interface IScheduledTask
    {
        int Schedule { get; }

        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
