using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OperateHSL.Core.Configuration;
using OperateHSL.Core.Helpers;
using OperateHSL.Core.Infrastructure;
using OperateHSL.Core.Infrastructure.DependencyManagement;

namespace OperateHSL.Web.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="typeFinder"></param>
        /// <param name="services"></param>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, IServiceCollection services)
        {
            //注册httpcontext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Add(new ServiceDescriptor(serviceType: typeof(IWebHelper), implementationType: typeof(WebHelper), lifetime: ServiceLifetime.Transient));
            //register all settings
            builder.RegisterSource(new SettingsSource());
        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SettingsSource : IRegistrationSource
    {
        private static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="registrations"></param>
        /// <returns></returns>
        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Service service,
            Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAdapterForIndividualComponents { get { return false; } }
    }
}