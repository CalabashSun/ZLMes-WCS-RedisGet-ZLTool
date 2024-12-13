using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.AGV
{
    public class CancelTaskRequest
    {
        /// <summary>
        /// 请求编号，每个请求都要一个唯一
        /// 编号， 同一个请求重复提交， 使
        /// 用同一编号。
        /// </summary>
        public string reqCode { get; set; }
        /// <summary>
        /// 请求时间截 格式: “yyyy-MM-dd HH:mm:ss”。
        /// </summary>
        public string reqTime { get; set; }
        /// <summary>
        /// 客户端编号，如 PDA，HCWMS 等
        /// </summary>
        public string clientCode { get; set; }
        /// <summary>
        /// 令牌号, 由调度系统颁发。
        /// </summary>
        public string tokenCode { get; set; }
        /// <summary>
        /// 取消类型0 表示：取消后货架直接放地上1 表示：AGV 仍然背着货架， 根据回库区域执行回库指令， 只有潜伏车支持。默认的取消模式为 0
        /// </summary>
        public string forceCancel { get; set; }
        /// <summary>
        /// forcecancel=1 时有意义回库区域编号，如果为空，采用货架配置的库区。
        /// </summary>
        public string matterArea { get; set; }
        /// <summary>
        /// 取消该 AGV 正在执行的任务单
        /// </summary>
        public string agvCode { get; set; }
        /// <summary>
        /// 任务单编号, 取消该任务单
        /// </summary>
        public string taskCode { get; set; }
    }
}
