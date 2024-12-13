using HslCommunication;
using Newtonsoft.Json;
using Operate.Services.PlcConnectService;
using OperateHSL.Core.Caching;
using OperateHSL.Core.Redising;
using OperateHSL.Data.DataModel.PLCModel;
using OperateHSL.Data.DataModel.ThirdPartModel;
using OperateHSL.Services.AgvPosition;
using OperateHSL.Services.AgvService;
using OperateHSL.Services.PlcInfoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class WeldingRepairLineTask : IScheduledTask
    {

        private readonly ICacheManager _caheManager;
        private readonly IAgvControlService _agvControlService;
        public WeldingRepairLineTask(IPlcDataService plcDataService, ICacheManager cacheManager
            ,IAgvControlService agvControlService
            ,IAgvPositionService agvPositionService)
        {
            _caheManager = cacheManager;
            _agvControlService = agvControlService;
        }

        public int Schedule => 7000;
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();

            checkData.Add("开始执行空托盘调度任务");

            checkData.Add("开始执行下件区满框送走");

            await _agvControlService.AddTransStrusFullTrayTask();

            checkData.Add("结束执行下件区满框送走");

            checkData.Add("开始执行下件区空托盘回流");

            await _agvControlService.AddTransRickingEmptyTrayTask();

            checkData.Add("结束执行下件区空托盘回流");




            checkData.Add("结束执行空托盘调度任务");
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
