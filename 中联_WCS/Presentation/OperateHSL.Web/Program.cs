using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OperateHSL.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 程序入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            if (!HslCommunication.Authorization.SetAuthorizationCode("891c8f18-d6de-409f-81f4-6de405431905"))
            {
                return;
            }
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// 创建webhost
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => options.AddServerHeader = false)
                .UseIIS()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5014")
                .Build();
    }
}
