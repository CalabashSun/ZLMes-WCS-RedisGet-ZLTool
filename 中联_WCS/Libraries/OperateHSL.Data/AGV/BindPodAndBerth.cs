using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.AGV
{
    public  class BindPodAndBerth
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

        public string podCode { get; set; }

        public string positionCode { get; set; }

        public string podDir { get; set; }

        public string indBind { get; set; }
    }
}
