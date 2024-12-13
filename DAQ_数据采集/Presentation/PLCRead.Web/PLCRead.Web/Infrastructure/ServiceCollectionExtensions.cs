using PLCRead.Core.Configuration;
using PLCRead.Core.Infrastructure;
using PLCRead.Service.ScheduleTasks.Tasks;
using PLCRead.Service.ScheduleTasks;
using System.Threading.RateLimiting;
using PLCRead.Core.Helpr;
using System.Reflection;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using PLCRead.Core.Caching;
using PLCRead.Service.RedisCache;
using System.Net;

namespace PLCReadInfo.Web.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure base application settings
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="builder">A builder for web applications and services</param>
        public static void ConfigureApplicationSettings(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            //let the operating system decide what TLS protocol version to use
            //see https://docs.microsoft.com/dotnet/framework/network-programming/tls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            //create default file provider
            CommonHelper.DefaultFileProvider = new ProFileProvider(builder.Environment);

            //register type finder
            var typeFinder = new WebAppTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;
            services.AddSingleton<ITypeFinder>(typeFinder);

            //add configuration parameters
            var configurations = typeFinder
                .FindClassesOfType<IConfig>()
                .Select(configType => (IConfig)Activator.CreateInstance(configType))
                .ToList();

            foreach (var config in configurations)
                builder.Configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);

            var appSettings = AppSettingsHelper.SaveAppSettings(configurations, CommonHelper.DefaultFileProvider, false);
            services.AddSingleton(appSettings);
        }



        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="builder">A builder for web applications and services</param>
        public static void ConfigureApplicationServices(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            //register file
            services.AddScoped<IProFileProvider, ProFileProvider>();



            //register type finder
            var typeFinder = new WebAppTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;
            services.AddSingleton<ITypeFinder>(typeFinder);


            // Add Redis
            var appSettings = Singleton<AppSettings>.Instance;
            var connectionString = appSettings.Get<DistributedCacheConfig>().ConnectionString;
            var instanceName = appSettings.Get<DistributedCacheConfig>().InstanceName;
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = instanceName ?? string.Empty;
            });
            services.AddDistributedMemoryCache();

            //add accessor to HttpContext
            services.AddHttpContextAccessor();
            #region 注册单例模式
            services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
            services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
            services.AddSingleton<IStaticCacheManager, RedisCacheManager>();
            #endregion



            



            //find startup configurations provided by other assemblies
            var startupConfigurations = typeFinder.FindClassesOfType<IProStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IProStartup)Activator.CreateInstance(startup))
                .Where(startup => startup != null)
                .OrderBy(startup => startup.Order);

            //configure services
            foreach (var instance in instances)
                instance.ConfigureServices(services, builder.Configuration);



        }
    }
}
