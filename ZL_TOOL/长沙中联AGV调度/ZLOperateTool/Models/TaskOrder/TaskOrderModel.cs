using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLOperateTool.Models.TaskOrder
{
    public class TaskOrderModel
    {
        public int Id { get; set; }

        public string mesorder { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string uloc { get; set; }

        public string TaskCount { get; set; }
        
        public string TaskCompletedCount { get; set; }

        public string TaskCompletedTime { get; set; }

        public string UpdateDt { get; set; }

        public string CreateDt { get; set; }

        public string FidCode { get; set; }

        public string MesProductDesc { get; set; }
    }

    public class PageData
    {
        public JArray DataList { get; set; }

        public int TotalCount { get; set; }

        public int TotalPage { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int per_page { get; set; }

        public int current_page{ get; set; }

        public int total_page { get; set; }
    }

    public class PageInfo
    {
        public JObject Data { get; set; }

        public int Status { get; set; }

        public string Msg { get; set; }
    }
}
