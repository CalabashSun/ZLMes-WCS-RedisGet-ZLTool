using PLCRead.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Service.ScheduleTasks.Tasks
{
    public class HeartBeatTask : IScheduledTask
    {

        private IStaticCacheManager _redisManager;
        public int Schedule => 50000;



        public HeartBeatTask(IStaticCacheManager redisManager)
        {
            _redisManager= redisManager;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("后台调度保活程序"+DateTime.Now);

            var errorData=new List<string>() {"cala","bash" };
            //var prefexErrorData = new string[] { "eletricerrordata" };
            //var cacheKeyErrorData = new CacheKey("eletricerrordata_001", prefexErrorData);
            //await _redisManager.SetAsync<List<string>>(cacheKeyErrorData, errorData);

            await _redisManager.SetAsync<List<string>>("eletricerrordata", errorData);

            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
