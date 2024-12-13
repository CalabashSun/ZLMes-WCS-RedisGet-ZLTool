using HslCommunication;
using Operate.Services.PlcConnectService;
using OperateHSL.Core.Caching;
using OperateHSL.Core.Redising;
using OperateHSL.Data.PLC;
using OperateHSL.Services.AgvPosition;
using OperateHSL.Services.PlcInfoService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class PLCStructuralLineTask : IScheduledTask
    {
        private IPlcDataService _plcDataSevice;
        private IAgvPositionService _agvPositionService;
        private readonly ICacheManager _caheManager;
        public PLCStructuralLineTask(IPlcDataService plcDataService,
            IAgvPositionService agvPositionService,
            ICacheManager cacheManager)
        {
            _plcDataSevice = plcDataService;
            _agvPositionService = agvPositionService;
            _caheManager = cacheManager;
        }

        public int Schedule => 7000;
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();
            checkData.Add("处理结构线物料下件数据开始" + DateTime.Now);

            var redisHelper = new RedisHelper();
            var otherData = new List<StructuralOtherData>();
            var emptyData = new List<StructuralOtherData>();
            //处理左侧 
            var task1 = Task.Factory.StartNew(async () =>
            {
                var exist = redisHelper.HashExists("plchangjiarightresult_002", "data");
                if (exist)
                {
                    var resultRightDatas = await redisHelper.HashGeAsync<List<CompletedMaterial>>("plchangjialeftresult_001", "data");
                    //处理数据
                    int i = 4;
                    //标识料框为禁止放料
                    var s7NetRight = PlcBufferRack.Instance("10.99.109.240");
                    checkData.Add("处理结构线下件左侧数据开始" + DateTime.Now);
                    foreach (var itemright in resultRightDatas)
                    {
                        if (itemright.MaxFlag && itemright.StacksMax == itemright.StacksNow && itemright.Allow)
                        {
                            for (int zz = 0; zz < 3; zz++)
                            {
                                s7NetRight.Write("DB111." + i + ".2", false);
                            }
                            //处理点位数据
                            var isExistTask = _plcDataSevice.PositionIsExistAgvTask(itemright.StacksNumber);
                            if (!isExistTask)
                            {
                                var postionInfo = await _agvPositionService.GetPositionInfo(itemright.StacksNumber);
                                if (postionInfo != null&&!string.IsNullOrEmpty(itemright.DataModel))
                                {
                                    postionInfo.PositionState = 2;
                                    postionInfo.UpdateDt = DateTime.Now;
                                    postionInfo.ReqCode = "WorkPiece" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                    postionInfo.TrayType = itemright.Alone ? 60 : 50;
                                    postionInfo.TrayData = 2;
                                    postionInfo.TrayMaterial = itemright.DataModel;
                                    postionInfo.PositionRemark = "物料：" + itemright.DataModel + ",数量：" + itemright.StacksNow;
                                    await _agvPositionService.UpdatePositionInfo(postionInfo);
                                }
                            }
                        }
                        else
                        {
                            if (itemright.Allow == true)
                            {
                                var otherSigle = new StructuralOtherData();
                                otherSigle.postion = itemright.StacksNumber;
                                otherSigle.trayType = itemright.Alone ? 60 : 50;
                                otherData.Add(otherSigle);
                            }
                            else
                            {
                                var emptySigle = new StructuralOtherData();
                                emptySigle.postion = itemright.StacksNumber;
                                emptySigle.trayType = itemright.Alone ? 60 : 50;
                                emptyData.Add(emptySigle);
                            }

                        }
                        i = i + 50;
                    }
                    checkData.Add("处理结构线下件左侧数据结束" + DateTime.Now);
                }

            });
            //处理右侧
            var task2 = Task.Factory.StartNew(async () =>
            {
                var exist = redisHelper.HashExists("plchangjiarightresult_002", "data");
                if (exist)
                {
                    var resultRightDatas = redisHelper.HashGet<List<CompletedMaterial>>("plchangjiarightresult_002", "data");
                    //处理数据
                    int i = 4;
                    //标识料框为禁止放料
                    var s7NetRight = PlcBufferRack.Instance("10.99.109.247");
                    checkData.Add("处理结构线下件右侧数据开始" + DateTime.Now);
                    foreach (var itemright in resultRightDatas)
                    {
                        if (itemright.MaxFlag && itemright.StacksMax == itemright.StacksNow && itemright.Allow)
                        {
                            var resultwrite = s7NetRight.Write("DB111." + i + ".2", false);
                            //判断该点位是否有未完成的agv任务
                            var isExistTask = _plcDataSevice.PositionIsExistAgvTask(    itemright.StacksNumber);
                            if (!isExistTask)
                            {
                                //处理点位数据
                                var postionInfo = await _agvPositionService.GetPositionInfo(itemright.StacksNumber);
                                if (postionInfo != null && !string.IsNullOrEmpty(itemright.DataModel))
                                {
                                    postionInfo.PositionState = 2;
                                    postionInfo.UpdateDt = DateTime.Now;
                                    postionInfo.ReqCode = "WorkPiece" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                    postionInfo.TrayType = itemright.Alone ? 60 : 50;
                                    postionInfo.TrayData = 2;
                                    postionInfo.TrayMaterial = itemright.DataModel;
                                    postionInfo.PositionRemark = "物料：" + itemright.DataModel + ",数量：" + itemright.StacksNow;
                                    await _agvPositionService.UpdatePositionInfo(postionInfo);
                                }
                            }
                        }
                        else
                        {
                            if (itemright.Allow == true)
                            {
                                var otherSigle = new StructuralOtherData();
                                otherSigle.postion = itemright.StacksNumber;
                                otherSigle.trayType = itemright.Alone ? 60 : 50;
                                otherData.Add(otherSigle);
                            }
                            else
                            {
                                var emptySigle = new StructuralOtherData();
                                emptySigle.postion = itemright.StacksNumber;
                                emptySigle.trayType = itemright.Alone ? 60 : 50;
                                emptyData.Add(emptySigle);
                            }
                        }
                        i = i + 50;
                    }
                    checkData.Add("处理结构线下件右侧数据结束" + DateTime.Now);
                }

            });

            await Task.WhenAll(task1, task2);

            await _agvPositionService.UpdateChoosedPosition(otherData);
            await _agvPositionService.UpdateChoosedPositionEmpty(emptyData);


            checkData.Add("处理结构线物料下件数据结束" + DateTime.Now);
            _caheManager.Remove("strucssread_001_check");
            _caheManager.Set("strucssread_001_check", checkData, 1);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
