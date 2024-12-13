using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OperateHSL.Core.Configuration;
using OperateHSL.Core.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using OperateHSL.Services.Scheduling;
using OperateHSL.Services.Scheduling.Tasks;
using OperateHSL.Web.Infrastructure.Filter;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace OperateHSL.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwagger();
        }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Configured service provider</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureStartupConfig<mysql>(configuration.GetSection("mysql"));
            services.AddProjectMvc();
            //add accessor to HttpContext
            services.AddHttpContextAccessor();
            //注册自动执行任务
            services.AddTasks();
            //create, initialize and configure the engine

            //注册过滤器
            //services.AddSingleton<RequestLogAttribute>();
            var engine = EngineContext.Create();
            engine.Initialize(services);
            var serviceProvider = engine.ConfigureServices(services, configuration);

            return serviceProvider;
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);
            services.AddProjectMvc();


            return config;
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        /// <summary>
        /// 注册mvc
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IMvcBuilder AddProjectMvc(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ApiExceptionFilter));
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();//json字符串大小写原样输出
                options.SerializerSettings.Formatting = Formatting.None;
            });
            return mvcBuilder;
        }
        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="services"></param>
        public static void AddTasks(this IServiceCollection services)
        {
            //FID
            //组队下件
            services.AddSingleton<IScheduledTask, PLCFIDReadTask>();
            services.AddSingleton<IScheduledTask, PLCStructuralLineTask>();

            //AGV
            //下件区调度
           //services.AddSingleton<IScheduledTask, WeldingRepairLineTask>();
           // services.AddSingleton<IScheduledTask, AGVDispatchTask>();
           // //打磨房调度
           // services.AddSingleton<IScheduledTask, WeldingRepairLinePerformTask>();
           // //打磨完成缓存区到上挂区
           // services.AddSingleton<IScheduledTask, UpCatchTask>();
           // //成品区空托盘回流
           // services.AddSingleton<IScheduledTask, PLCCollectionTask>();
           
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });
        }
        /// <summary>
        /// swagger注册
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v0.0.1", new Info
                {
                    Version = "v0.0.1",
                    Title = "合信-Api-Document",
                    Description = "合信接口项目",
                    Contact = new Contact { Name = "Calabash", Email = "924347693@qq.com", Url = "" }
                });
                p.DescribeAllParametersInCamelCase();
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "HecinApi.xml");
                p.IncludeXmlComments(xmlPath);
            });
        }

    }
}