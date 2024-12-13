using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using OperateHSL.Services.Log4netService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperateHSL.Web.Infrastructure.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private IHostingEnvironment _env;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public ApiExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
            {
                return;
            }
            LogHelper.Error("程序运行发生异常", context.Exception); // 日志记录
            var exMsg = context.Exception.Message;
            context.ExceptionHandled = true;
        }
    }
}
