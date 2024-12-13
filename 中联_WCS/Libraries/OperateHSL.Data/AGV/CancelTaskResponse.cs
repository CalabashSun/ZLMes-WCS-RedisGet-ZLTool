using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.AGV
{
    public  class CancelTaskResponse
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
    }
}
