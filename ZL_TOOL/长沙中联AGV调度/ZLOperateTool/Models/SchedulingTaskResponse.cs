using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLOperateTool.Models
{
    public class SchedulingTaskResponse
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 请求编号
        /// </summary>
        public string reqCode { get; set; }
        /// <summary>
        /// 自定义返回（返回任务单号）
        /// </summary>
        public string data { get; set; }
    }
}
