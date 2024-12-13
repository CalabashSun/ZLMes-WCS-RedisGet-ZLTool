using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PLCRead.Core.Caching;
using PLCRead.Core.Configuration;
using PLCRead.Core.Helpr;
using PLCRead.Core.Infrastructure;
using PLCRead.Service.RedisCache;
using PLCRead.Service.ScheduleTasks;
using PLCRead.Service.ScheduleTasks.Tasks;
using PLCReadInfo.Web.Infrastructure;
using System.Reflection;

namespace PLCRead.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (!HslCommunication.Authorization.SetAuthorizationCode("891c8f18-d6de-409f-81f4-6de405431905"))
            {
                return;
            }
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile(PLCConfigurationDefaults.AppSettingsFilePath, true, true);

            builder.Configuration.AddEnvironmentVariables();


            //load application settings
            builder.Services.ConfigureApplicationSettings(builder);






            // ÅäÖÃ¶Ë¿ÚºÅ
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(5010);
            });
            builder.Services.ConfigureApplicationServices(builder);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddMvcCore();
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            await app.RunAsync();

        }
    }
}


