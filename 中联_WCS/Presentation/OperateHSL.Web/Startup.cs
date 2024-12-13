using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OperateHSL.Services.Log4netService;
using OperateHSL.Web.Infrastructure.Extensions;

namespace OperateHSL.Web
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {

        public static ILoggerRepository logrepository { get; set; }
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("configs/appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("configs/dbappsetting.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }



        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            logrepository = LogManager.CreateRepository("CoreLogRepository");
            XmlConfigurator.Configure(logrepository, new FileInfo("configs/log4net.config"));
            Log4NetRepository.loggerRepository = logrepository;
            services.ApplicationServices(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            return services.ConfigureApplicationServices(Configuration);
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v0.0.1/swagger.json", "ZFCT API V0.0.1");
            });
        }
    }
}
