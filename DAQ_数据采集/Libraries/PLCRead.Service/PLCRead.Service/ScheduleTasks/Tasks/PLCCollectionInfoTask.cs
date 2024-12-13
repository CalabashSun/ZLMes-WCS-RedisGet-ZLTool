using HslCommunication;
using PLCRead.Core.Caching;
using PLCRead.Core.Configuration;
using PLCRead.Service.PlcConnectService;
using PLCRead.Service.RedisCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Service.ScheduleTasks.Tasks
{
    public class PLCCollectionInfoTask : IScheduledTask
    {
        private readonly IStaticCacheManager _redisManager;
        public PLCCollectionInfoTask(IStaticCacheManager redisManager)
        {
            _redisManager = redisManager;
        }

        public int Schedule => 5000;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();
            checkData.Add("下料线plc交互类数据采集任务开始" + DateTime.Now);
            //清空下料线交互类数据采集缓存
            var s7Net = PlcBufferRack.Instance(collection.orderPlc);
            if (s7Net != null)
            {
                var dataLength = Convert.ToUInt16(collection.orderPlcLength);
                OperateResult<byte[]> result = s7Net.Read(collection.orderPlcAddress + ".0", dataLength);
                if (result.IsSuccess)
                {
                    var recvs = result.Content;
                    var prefex = new string[] { "slingcing" };
                    var cacheKey = new CacheKey("slingcingresult_001",prefex);
                    await _redisManager.SetAsync<byte[]>(cacheKey, recvs);
                }
                else
                {
                    //日志记录一下错误数据
                    Console.WriteLine(collection.orderPlc + ",通讯异常" + ",原因：Plc连接失败");
                    checkData.Add(collection.orderPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                    PlcBufferRack.RemovePlcIp(collection.orderPlc);
                }
            }
            else
            {
                Console.WriteLine(collection.orderPlc + ",通讯异常" + ",原因：Plc连接失败");
                checkData.Add(collection.orderPlc + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                PlcBufferRack.RemovePlcIp(collection.orderPlc);
            }




            Console.WriteLine("下料线plc交互类数据采集任务结束" + DateTime.Now);
            checkData.Add("下料线plc交互类数据采集任务结束" + DateTime.Now);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
