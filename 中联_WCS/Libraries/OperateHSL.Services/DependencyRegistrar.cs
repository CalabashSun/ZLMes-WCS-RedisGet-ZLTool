using Autofac;
using Microsoft.Extensions.DependencyInjection;
using OperateHSL.Core.Caching;
using OperateHSL.Core.DbContext;
using OperateHSL.Core.Infrastructure;
using OperateHSL.Core.Infrastructure.DependencyManagement;
using OperateHSL.Services.AgvPosition;
using OperateHSL.Services.AgvService;
using OperateHSL.Services.FIDInfoService;
using OperateHSL.Services.LogService;
using OperateHSL.Services.MesOrderService;
using OperateHSL.Services.PlcInfoService;

namespace OperateHSL.Services
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(serviceType: typeof(ICacheManager), implementationType: typeof(MemoryCacheManager), lifetime: ServiceLifetime.Singleton));
            services.Add(new ServiceDescriptor(serviceType: typeof(IDbContext), implementationType: typeof(DapperDbContext), lifetime: ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(serviceType: typeof(IApiLogService), implementationType: typeof(ApiLogService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IPlcDataService), implementationType: typeof(PlcDataService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IAgvControlService), implementationType: typeof(AgvControlService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IFIDService), implementationType: typeof(FIDService), lifetime: ServiceLifetime.Singleton));
            services.Add(new ServiceDescriptor(serviceType: typeof(IOrderOperateService), implementationType: typeof(OrderOperateService), lifetime: ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(serviceType: typeof(IAgvPositionService), implementationType: typeof(AgvPositionService), lifetime: ServiceLifetime.Transient));
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 2;
    }
}
