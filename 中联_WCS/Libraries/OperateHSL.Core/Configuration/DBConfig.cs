using System;
using System.Collections.Generic;
using System.Text;

namespace OperateHSL.Core.Configuration
{
    public partial class mysql
    {
        public string connection { get; set; }

        public string weburl { get; set; }

        public string redishost { get; set; }

        public string agvUrl {get;set;}

        public string continueAgvUrl { get; set; }

        public string podAgvUrl { get; set; }

        public string agvStateUrl { get; set; }

        public string hecinMesUrl { get; set; }
        /// <summary>
        /// 中联mes url
        /// </summary>
        public string zlMesUrl { get; set; }
        /// <summary>
        /// 上报snUrl
        /// </summary>
        public string zlMesSNUrl { get; set; }

        public string zlCollectionMesUrl { get; set; }
        /// <summary>
        /// 中联mes密钥
        /// </summary>
        public string zlMesSecret { get; set; }
        /// <summary>
        /// 中联mes appid
        /// </summary>
        public string zlMesAppid { get; set; }
        /// <summary>
        /// agv调度方式
        /// </summary>
        public string agvTransType { get; set; }
    }
}
