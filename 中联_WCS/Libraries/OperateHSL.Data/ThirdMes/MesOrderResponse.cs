using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Data.ThirdMes
{
    /// <summary>
    /// 所有的感觉都是这个返回
    /// </summary>
    public class MesOrderResponse
    {
        /// <summary>
        /// 200:成功;其他失败，例如500
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 例如，“接口鉴权失败
        /// </summary>
        public string msg { get; set; }
    }
}
