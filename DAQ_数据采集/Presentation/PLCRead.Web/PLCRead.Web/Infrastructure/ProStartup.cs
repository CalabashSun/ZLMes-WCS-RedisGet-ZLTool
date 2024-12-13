using Autofac.Core;
using HslCommunication.Instrument.IEC.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using PLCRead.Core.Caching;
using PLCRead.Core.Configuration;
using PLCRead.Core.Infrastructure;
using PLCRead.Service.RedisCache;
using System.Runtime;
using System.Threading.Tasks;
using System;
using PLCRead.Core.Helpr;
using PLCRead.Service.ScheduleTasks.Tasks;
using PLCRead.Service.ScheduleTasks;
using PLCRead.Core.DbContext;
using PLCRead.Service.ElecService;

namespace PLCReadInfo.Web.Infrastructure
{
    public class ProStartup:IProStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.Add(new ServiceDescriptor(serviceType: typeof(IDbContext), implementationType: typeof(DapperDbContext), lifetime: ServiceLifetime.Scoped));
            #region 注册定时任务
            //// Add Task
            services.AddSingleton<IScheduledTask, HeartBeatTask>();
            ////贴板数据
            //services.AddSingleton<IScheduledTask, PLCFlitchTask>();
            ////组队数据
            //services.AddSingleton<IScheduledTask, PLCTeamUpTask>();
            ////自动焊数据
            //services.AddSingleton<IScheduledTask, AutomaticTask>();
            ////绗架数据
            //services.AddSingleton<IScheduledTask, PLCTrussTask>();
            //电表数据
            services.AddSingleton<IScheduledTask, EletricTask>();


            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });
            #endregion


            #region 注册services服务
            services.Add(new ServiceDescriptor(serviceType: typeof(IDbContext), implementationType: typeof(DapperDbContext), lifetime: ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(serviceType: typeof(IPlcElecService), implementationType: typeof(PlcElecService), lifetime: ServiceLifetime.Transient));
            #endregion
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 2000;
    }
}
