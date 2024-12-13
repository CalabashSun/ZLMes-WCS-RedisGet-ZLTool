using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OperateHSL.Core.Configuration;
using OperateHSL.Core.Helpers;
using OperateHSL.Core.Infrastructure;
using OperateHSL.Data.DataModel.ThirdPartModel;
using OperateHSL.Data.ThirdMes;
using OperateHSL.Services.AgvService;
using OperateHSL.Services.LogService;
using OperateHSL.Services.MesOrderService;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace OperateHSL.Web.Controllers
{
    /// <summary>
    /// 中科云谷mes接口交互
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class MesDataController : Controller
    {
        public static string mesUrl = EngineContext.Current.Resolve<mysql>().zlMesUrl;
        public static string mesSNUrl = EngineContext.Current.Resolve<mysql>().zlMesSNUrl;
        public static string mesCollectionUrl = EngineContext.Current.Resolve<mysql>().zlCollectionMesUrl;

        private readonly IApiLogService _logService;
        private readonly IOrderOperateService _orderOperateService;
        private readonly IAgvControlService _agvControlService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="apiLogService"></param>
        public MesDataController(IApiLogService apiLogService,
            IOrderOperateService orderOperateService,
            IAgvControlService agvControlService)
        {
            _logService = apiLogService;
            _orderOperateService = orderOperateService;
            _agvControlService = agvControlService;
        }


        /// <summary>
        /// 中科云谷下发订单数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> OrderInfo([FromBody]MesOrderRequest model)
        {
            var result = new MesOrderResponse();
            if (model == null)
            {
                result.code = "-1";
                result.msg = "数据不存在";
                return Json(result);
            }
            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "2",
                TaskNo = model.jobNo,
                DataType = "3"
            };
            var addResult =await _orderOperateService.AddMesOrder(model);
            if (addResult==200)
            {
                result.code = "200";
                result.msg = "成功";
            }
            else
            {
                if (addResult == 300)
                {
                    result.code = "500";
                    result.msg = "工单已存在，无需重复下发";
                }
                else
                {
                    result.code = "500";
                    result.msg = "参数错误";
                }
                
            }
            //记录日志
            logModel.CreateDt = DateTime.Now;
            logModel.UpdateDt = DateTime.Now;
            logModel.DataResult = JsonConvert.SerializeObject(result);
            _logService.AddApiLogRecord(logModel);
            return Json(result);
        }


        /// <summary>
        /// 中科云谷下发置空托盘指令
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmptyTray([FromBody]EmptyTrayRequest model)
        {
            var result = new MesOrderResponse();
            if (model == null)
            {
                result.code = "-1";
                result.msg = "数据不存在";
                return Json(result);
            }

            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "2",
                TaskNo = model.mesTaskId,
                DataType = "3"
            };


            var addResult = await _orderOperateService.EmptyTrayInfo(model.StartLoc,model.Vehicletype);
            if (addResult)
            {
                result.code = "200";
                result.msg = "成功";
            }
            else
            {
                result.code = "500";
                result.msg = "参数错误";
            }
            //记录日志
            logModel.CreateDt = DateTime.Now;
            logModel.UpdateDt = DateTime.Now;
            logModel.DataResult = JsonConvert.SerializeObject(result);
            _logService.AddApiLogRecord(logModel);
            return Json(result);
        }


        /// <summary>
        /// 合信下发置空托盘指令
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EmptyRickingTray([FromBody]EmptyRickingTrayRequest model)
        {
            var result = new MesOrderResponse();
            if (model == null)
            {
                result.code = "-1";
                result.msg = "数据不存在";
                return Json(result);
            }

            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "2",
                TaskNo = model.reqNo,
                DataType = "5"
            };


            var addResult = await _orderOperateService.EmptyRcikingTrayInfo(model.positionCode, model.trayDataType);
            if (addResult)
            {
                result.code = "200";
                result.msg = "成功";
            }
            else
            {
                result.code = "500";
                result.msg = "参数错误";
            }
            //记录日志
            logModel.CreateDt = DateTime.Now;
            logModel.UpdateDt = DateTime.Now;
            logModel.DataResult = JsonConvert.SerializeObject(result);
            _logService.AddApiLogRecord(logModel);
            return Json(result);
        }


        /// <summary>
        /// 合信下发关闭打磨房叫料送料指令
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CloseWeldingRoom([FromBody]CloseWelding model)
        {
            var result = new MesOrderResponse();
            if (model == null)
            {
                result.code = "-1";
                result.msg = "数据不存在";
                return Json(result);
            }

            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "2",
                TaskNo = DateTime.Now.ToString("yyyyMMddHHmmss"),
                DataType = "5"
            };


            await _agvControlService.CloseWeldingRoomData(model.position, model.type);
            //记录日志
            logModel.CreateDt = DateTime.Now;
            logModel.UpdateDt = DateTime.Now;
            logModel.DataResult = "打磨房信号已处理";
            _logService.AddApiLogRecord(logModel);
            result.code = "200";
            result.msg = "成功";
            return Json(result);
        }

        /// <summary>
        /// 合信下发下件区托盘情况送料指令
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> StructalSignal([FromBody]StructuralSignalHandle model)
        {
            var result = new MesOrderResponse();
            if (model == null)
            {
                result.code = "-1";
                result.msg = "数据不存在";
                return Json(result);
            }

            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "2",
                TaskNo = DateTime.Now.ToString("yyyyMMddHHmmss"),
                DataType = "5"
            };


            await _agvControlService.HandleStructuralSignal(model.position, model.type);
            //记录日志
            logModel.CreateDt = DateTime.Now;
            logModel.UpdateDt = DateTime.Now;
            logModel.DataResult = "下件区信号已处理";
            _logService.AddApiLogRecord(logModel);
            return Json(result);
        }


        /// <summary>
        /// 上报中科云谷下料线订单执行情况
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CallCollectionOrderState(string aufnr= "009000190200", string matnr="",string menge="10",string vornr="0010")
        {
            var postJson = new CollectionOrderRequest
            {
                AUFNR = aufnr,
                BGDAT = DateTime.Now.ToString("yyyyMMdd"),
                IEDZ = DateTime.Now.ToString("HHmmss"),
                MATNR= matnr,
                MENGE= menge,
                VORNR="0010"
            };
            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(postJson),
                RequestType = "1",
                TaskNo = aufnr,
                DataType = "3"
            };
            var setting = new JsonSerializerSettings()
            {
                ContractResolver = null,
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
            var client = new RestClient(mesCollectionUrl
                , configureSerialization: s => s.UseNewtonsoftJson(setting));
            var signInfo = EngineContext.Current.Resolve<mysql>().zlMesSecret;
            var appidInfo = EngineContext.Current.Resolve<mysql>().zlMesAppid;
            var request = new RestRequest();
            
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("x-open-sign", signInfo);
            request.AddHeader("x-open-appid", appidInfo);
            var timestamp = EncryptorHelper.GetCurrentUnixTimestamp();
            request.AddHeader("timestamp", timestamp);
            var md5Secret = EncryptorHelper.GetMD5Hash(timestamp + signInfo);
            request.AddHeader("x-open-sign-mode", "simple");
            //加密
            request.AddJsonBody(postJson);
            var response = await client.ExecutePostAsync(request);
            logModel.DataResult = response.Content;
            _logService.AddApiLogRecord(logModel);
            return Json(logModel.DataResult);
        }

        /// <summary>
        /// 上报中科云谷结构线订单执行情况
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult>  CallStructuralOrderState(string taskId= "202311201312",
            string jobNo= "202311201312",
            int qty=10,
            string reportType="0",
            string uloc="gx"
            )
        {
            var postJson = new TaskReportRequest
            {
                taskId= taskId,
                jobNo = jobNo,
                machineNo ="",
                qty = qty,
                reportType = reportType,
                source = "HX",
                plantNo="9301",
                uloc= uloc
            };
            var reportResult=  _orderOperateService.ReportTicketState(postJson);
            return Json(reportResult);
        }

        /// <summary>
        /// 上报中科云谷结构线订单执行情况 SN回传及组队订单完工
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> SNDataCallBack(string jobNo= "202311201312",
            string sn= "12WC220231120001"
            )
        {
            var postJson = new SNCallbackRequest
            {
                jobNo = jobNo,
                SN = sn,
                plantNo = "9301"
            };
            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(postJson),
                RequestType = "1",
                TaskNo = jobNo,
                DataType = "7",
                CreateDt=DateTime.Now,
                UpdateDt=DateTime.Now
            };
            var signInfo = EngineContext.Current.Resolve<mysql>().zlMesSecret;
            var appidInfo = EngineContext.Current.Resolve<mysql>().zlMesAppid;
            var request = new RestRequest();
            var setting = new JsonSerializerSettings()
            {
                ContractResolver = null,
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
            var client = new RestClient(mesSNUrl,
                configureSerialization: s => s.UseNewtonsoftJson(setting)
                );
            request.AddHeader("Content-Type", "application/json");
            var timestamp = EncryptorHelper.GetCurrentUnixTimestamp();
            var md5Secret = EncryptorHelper.GetMD5Hash(timestamp + signInfo);
            //request.AddHeader("x-open-sign", signInfo);
            request.AddHeader("x-open-sign", md5Secret);
            request.AddHeader("x-open-appid", appidInfo);
            
            request.AddHeader("timestamp", timestamp);
            
            request.AddHeader("x-open-sign-mode", "simple");
            //加密
            request.AddJsonBody(postJson);
            var response = await client.ExecutePostAsync(request);
            logModel.DataResult = response.Content;

            var rfidData =await _orderOperateService.TicketTaskRFIDData(sn);
            if (rfidData != null)
            {
                if (response.Content.Contains("200"))
                {
                    //查询对应RFID并报工成功
                    rfidData.reportstate = 1;
                    rfidData.reportStateDesc = response.Content;
                    rfidData.UpdateDt = DateTime.Now;
                    
                }
                else
                {
                    rfidData.reportstate = 2;
                    rfidData.reportStateDesc = response.Content;
                    rfidData.UpdateDt = DateTime.Now;
                }
            }


            //组队完工
            var postJsonTeamUp = new TaskReportRequest
            {
                taskId = "teamup" + DateTime.Now.ToString("yyyyMMddHHMMssfff"),
                jobNo = jobNo,
                machineNo = rfidData.ipccode.ToString(),
                qty = 1,
                reportType = "1",
                source = "HX",
                plantNo = "9301",
                uloc = rfidData.uloc//这里是组队的工序 理论上应该是下发的时候有的到时候可以写到rfid生成表里
            };
            var reportResult = await _orderOperateService.ReportTicketState(postJsonTeamUp);
            if (reportResult.Contains("200"))
            {
                rfidData.autoReportState = 1;
                rfidData.autoReportStateDesc = reportResult;

            }
            else
            {
                rfidData.autoReportState = 2;
                rfidData.autoReportStateDesc = reportResult;
            }
            rfidData.UpdateDt = DateTime.Now;
            _ = _orderOperateService.UpdateTicketTaskState(rfidData);
            _logService.AddApiLogRecord(logModel);
            return Json(response.Content);
        }

        /// <summary>
        /// 同步返回结果
        /// </summary>
        /// <param name="jobNo"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> SNDataCallBackSync(string jobNo = "202311201312",
            string sn = "12WC220231120001"
            )
        {
            var postJson = new SNCallbackRequest
            {
                jobNo = jobNo,
                SN = sn,
                plantNo = "9301"
            };
            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(postJson),
                RequestType = "1",
                TaskNo = jobNo,
                DataType = "7",
                CreateDt = DateTime.Now,
                UpdateDt = DateTime.Now
            };
            var signInfo = EngineContext.Current.Resolve<mysql>().zlMesSecret;
            var appidInfo = EngineContext.Current.Resolve<mysql>().zlMesAppid;
            var request = new RestRequest();
            var setting = new JsonSerializerSettings()
            {
                ContractResolver = null,
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
            var client = new RestClient(mesSNUrl,
                configureSerialization: s => s.UseNewtonsoftJson(setting)
                );
            request.AddHeader("Content-Type", "application/json");
            var timestamp = EncryptorHelper.GetCurrentUnixTimestamp();
            var md5Secret = EncryptorHelper.GetMD5Hash(timestamp + signInfo);
            request.AddHeader("x-open-sign", md5Secret);
            request.AddHeader("x-open-appid", appidInfo);

            request.AddHeader("timestamp", timestamp);

            request.AddHeader("x-open-sign-mode", "simple");
            //加密
            request.AddJsonBody(postJson);
            var response = await client.ExecutePostAsync(request);
            logModel.DataResult = response.Content;

            var rfidData = await _orderOperateService.TicketTaskRFIDData(sn);
            if (rfidData != null)
            {
                if (response.Content.Contains("200"))
                {
                    //查询对应RFID并报工成功
                    rfidData.reportstate = 1;
                    rfidData.reportStateDesc = response.Content;
                    rfidData.UpdateDt = DateTime.Now;
                    //组队完工
                    var postJsonTeamUp = new TaskReportRequest
                    {
                        taskId = "teamup" + DateTime.Now.ToString("yyyyMMddHHMMssfff"),
                        jobNo = jobNo,
                        machineNo = rfidData.ipccode.ToString(),
                        qty = 1,
                        reportType = "1",
                        source = "HX",
                        plantNo = "9301",
                        uloc = rfidData.uloc//这里是组队的工序 理论上应该是下发的时候有的到时候可以写到rfid生成表里
                    };
                    var reportResult = await _orderOperateService.ReportTicketState(postJsonTeamUp);
                    if (reportResult.Contains("200"))
                    {
                        rfidData.autoReportState = 1;
                        rfidData.autoReportStateDesc = reportResult;

                    }
                    else
                    {
                        rfidData.autoReportState = 2;
                        rfidData.autoReportStateDesc = reportResult;
                    }
                    rfidData.UpdateDt = DateTime.Now;
                    _ = _orderOperateService.UpdateTicketTaskState(rfidData);
                    _logService.AddApiLogRecord(logModel);
                    return Json(reportResult);
                }
                else
                {
                    rfidData.reportstate = 2;
                    rfidData.reportStateDesc = response.Content;
                    rfidData.UpdateDt = DateTime.Now;
                    _ = _orderOperateService.UpdateTicketTaskState(rfidData);
                }
                return Json(response.Content);
            }
            return Json(new { code = 500, msg="MES接口不通" });
        }

        /// <summary>
        /// 自动焊接口报工
        /// </summary>
        /// <param name="rfidCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AutoDataCallBak(string rfidCode)
        {
            if (string.IsNullOrEmpty(rfidCode))
            {
                return Json(new { code = 200,msg="rfid不存在" });
            }
            await _orderOperateService.ReportAutoTicket(rfidCode);
            return Json(new { code = 200 });
        }

        #region 手动报工
        /// <summary>
        /// 通过日志报工
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ReportMesByLog(int logId)
        {
            var logInfo = _logService.GetLogInfo(logId);
            if (logInfo == null)
            {
                var result = new {
                    data="500",
                    msg="日志数据不存在"
                };
                return Json(result);
            };
            var type = Convert.ToInt32(logInfo.DataType);
            var postJson = logInfo.DataInfo;
            var signInfo = EngineContext.Current.Resolve<mysql>().zlMesSecret;
            var appidInfo = EngineContext.Current.Resolve<mysql>().zlMesAppid;
            var request = new RestRequest();
            var setting = new JsonSerializerSettings()
            {
                ContractResolver = null,
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
            var postUrl=type==7? EngineContext.Current.Resolve<mysql>().zlMesSNUrl
                : EngineContext.Current.Resolve<mysql>().zlMesUrl;
          
            var client = new RestClient(postUrl, configureSerialization: s => s.UseNewtonsoftJson(setting));
            var timestamp = EncryptorHelper.GetCurrentUnixTimestamp();
            var md5Secret = EncryptorHelper.GetMD5Hash(timestamp + signInfo);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("x-open-sign", md5Secret);
            request.AddHeader("x-open-appid", appidInfo);
            
            request.AddHeader("timestamp", timestamp);
            
            request.AddHeader("x-open-sign-mode", "simple");
            object paramdata = new
            {
                xopensign = md5Secret,
                xopenappid = appidInfo,
                timestamp = timestamp
            };
            LogselfHelper.WriteLog(JsonConvert.SerializeObject(paramdata));
            //加密
            request.AddJsonBody(postJson);
            var response = await client.ExecutePostAsync(request);
            logInfo.DataResult = response.Content;
            logInfo.UpdateDt = DateTime.Now;
            _logService.UpdateApiLogRecord(logInfo);

            return Json(response.Content);
        }
        #endregion
    }
}