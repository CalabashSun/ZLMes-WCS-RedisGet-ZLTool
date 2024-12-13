using MySqlX.XDevAPI;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLOperateTool.Models;

namespace ZLOperateTool.Helper
{
    public class PostTaskAgv
    {
        public static SchedulingTaskResponse PostToAgv(SchedulingTaskRquest model)
        {
            var agvUrl= ConfigurationManager.AppSettings["agvUrl"];
            //生成AGV请求数据
            var request = new RestRequest();
            RestClient client = new RestClient(agvUrl);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model);
            var response = client.ExecutePost(request);
            var resultContent = response.Content;
            if (resultContent == "" || resultContent == "[]" || resultContent == "")
            {
                return null;
            }
            try
            {
                var resultJson = JsonConvert.DeserializeObject<SchedulingTaskResponse>(resultContent);
                return resultJson;
            }
            catch
            {
                return null;
            }
        }
    }
}
