using HslCommunication;
using Operate.Services.PlcConnectService;
using OperateHSL.Services.PlcInfoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class PLCFIDSetTask : IScheduledTask
    {
        private readonly IPlcDataService _plcDataSevice;

        public PLCFIDSetTask(IPlcDataService plcDataService)
        {
            _plcDataSevice = plcDataService;
        }
        public int Schedule => 8200;
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var fidLineDatas = _plcDataSevice.GetTransportData();
            var ipAddress = fidLineDatas.GroupBy(p => p.OrginalIpAddress).Select(m=>m.Key).ToList();
            foreach (var itemIp in ipAddress)
            {
                var thisIpData = fidLineDatas.Where(p => p.OrginalIpAddress == itemIp).ToList();
                var s7Net = PlcBufferRack.Instance(itemIp);
                if (s7Net != null)
                {
                    foreach (var itemDetails in thisIpData)
                    {
                        //read and update
                        OperateResult<short> currentCount = s7Net.ReadInt16("DB110." + itemDetails.DbWriteAddress + ".0");
                        if (currentCount.IsSuccess)
                        {
                            if (currentCount.Content != itemDetails.SumCount)
                            {
                                _plcDataSevice.UpdateTransportRecord(itemDetails.Id,currentCount.Content);
                            }
                        }
                    }
                }
            }
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
