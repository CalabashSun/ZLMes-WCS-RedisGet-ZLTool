using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OperateHSL.Core.Configuration;
using OperateHSL.Core.DbContext;
using OperateHSL.Core.Helpers;
using OperateHSL.Core.Infrastructure;
using OperateHSL.Data.DataModel.ThirdPartModel;
using OperateHSL.Data.DataModel.Tickets;
using OperateHSL.Data.IMes;
using OperateHSL.Data.OrderInfo;
using OperateHSL.Data.ThirdMes;
using OperateHSL.Services.Log4netService;
using OperateHSL.Services.LogService;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperateHSL.Services.MesOrderService
{
    public interface IOrderOperateService
    {
        Task<int> AddMesOrder(MesOrderRequest model);

        Task<string> ReportTicketState(TaskReportRequest model);



        Task<bool> EmptyTrayInfo(string positionCode, string trayType);

        Task<bool> EmptyRcikingTrayInfo(string positionCode, string trayType);

        Task<TicketTaskRfid> TicketTaskRFIDData(string rfidCode);

        Task UpdateTicketTaskState(TicketTaskRfid model);

        Task ReportAutoTicket(string rfidCode);
    }
    public class OrderOperateService : IOrderOperateService
    {

        private readonly IDbContext _dbContext;
        private readonly IApiLogService _apiLogService;


        public OrderOperateService(IDbContext dbContext,IApiLogService apiLogService)
        {
            _dbContext = dbContext;
            _apiLogService = apiLogService;
        }

        public async Task<int> AddMesOrder(MesOrderRequest model)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                {
                    await _conn.OpenAsync();
                }
                //判断mesorder是否存在
                var isExistSql = $"select count(*) from Product_Tickets where MesJobNo='{model.jobNo}'";
                var isExistCount = _conn.QueryFirst<int>(isExistSql);
                if (isExistCount > 0)
                {
                    ///工单已存在
                    return 300;
                }
                using (var transaction = _conn.BeginTransaction())
                {
                    try
                    {
                        //新建产成品
                        var finishiProduct = new FinishProduct();
                        finishiProduct.DeliverQuantity = Convert.ToInt32(model.qty);
                        finishiProduct.ProductName = model.maktx;
                        finishiProduct.ProductCode = model.matnr;
                        finishiProduct.RequiredQuantity = finishiProduct.DeliverQuantity;
                        finishiProduct.MaterialCode = "B_"+ CommonHelper.GenerateRandomDigitCode(3);
                        finishiProduct.FinishProductCode= model.matnr;
                        var finishId = _conn.Insert<FinishProduct>(finishiProduct);
                        //新建产成品bom
                        foreach (var bomItem in model.boms)
                        {
                            var bomData = new BomInfoReplace();
                            bomData.CombineName = bomItem.matnr;
                            bomData.NameCombineName = bomItem.maktx;
                            bomData.Base_UOM_name = bomItem.baseUnit;
                            bomData.procedureName = bomItem.procedureName;
                            bomData.procedureNo = bomItem.procedureNo;
                            bomData.UsageQty = Convert.ToDecimal(bomItem.qty);
                            bomData.FinishProductId = Convert.ToInt32(finishId);
                            //_conn.Insert<BomInfoReplace>(bomData);
                        }
                        //新增工单
                        var productTicket = new Product_Tickets();
                        productTicket.MesJobNo = model.jobNo;
                        productTicket.TicketCode = "PTZL-" + CommonHelper.GetMchntTxnSsn();
                        productTicket.TicketName = "中联高机结构线订单";
                        productTicket.TicketPlanCount = Convert.ToInt32(model.qty);
                        productTicket.uloc = model.uloc;
                        productTicket.MesProductCode= model.matnr;
                        //获取当前产品对应的数据库编号
                        var selectDataSql = $"select * from bominfo_materialdata where MaterialCode='{model.matnr}'  limit 0,1";
                        var dataResult = _conn.QueryFirstOrDefault<bominfo_materialdata>(selectDataSql);
                        productTicket.MesProductDesc = dataResult==null?"":dataResult.FidCode;
                        var materialDataCode= dataResult == null ? "" : dataResult.DataCode;

                        var pticketId = _conn.Insert<Product_Tickets>(productTicket);
                        //这里面应该根据工序绑定的数据来动态的循环新增
                        for (int i = 0; i < 3; i++)
                        {
                            //新增工单任务
                            var ticketTask = new TicketsTask();
                            ticketTask.ProductCode = model.matnr;
                            ticketTask.ProductId = Convert.ToInt32(finishId);
                            ticketTask.ProductName = model.maktx;
                            ticketTask.TaskCount = Convert.ToDecimal(model.qty);
                            ticketTask.TaskSortId = -1;
                            ticketTask.Ass_Product_Tickets = Convert.ToInt32(pticketId);
                            ticketTask.TicketCode = productTicket.TicketCode;
                            ticketTask.TaskCode = "TTZL-" + CommonHelper.GetMchntTxnSsn();
                            if (i == 0)
                            {
                                ticketTask.TaskSortCode = "Picking";
                                ticketTask.uloc = model.uloc;
                                //首道工序为人工拣选不报工
                                ticketTask.TaskCompletedCount = Convert.ToDecimal(model.qty);
                                ticketTask.TaskCompletedTime = DateTime.Now;
                                ticketTask.TaskState = 1;
                            }
                            ticketTask.MesProductCode = model.matnr;
                            ticketTask.MesProductDesc = dataResult == null ? "" : dataResult.FidCode;
                            ticketTask.mesorder = model.jobNo;
                            //获取对应的工控机
                            if (!string.IsNullOrEmpty(productTicket.MesProductDesc))
                            {
                                if (i == 1)
                                {
                                    var ipcCodesSql = $"select IpcCode from third_ipcproduct where FidCode='{productTicket.MesProductDesc}'";
                                    var ipcCodes = _conn.Query<string>(ipcCodesSql).ToList();
                                    var ipcCodesResult =","+string.Join(",", ipcCodes)+",";

                                    var upMesProduct = productTicket.MesProductDesc.Substring(0, 2);
                                    //截取前两位
                                    var teamSort = upMesProduct == "04" ? "H10104" : upMesProduct == "06" ? "H10105"
                                        : upMesProduct == "08" ? "H10106" : upMesProduct == "10" ? "H10107"
                                        : upMesProduct == "12" ? "H10108" : "";
                                    ticketTask.uloc = teamSort;
                                    ticketTask.TaskSortCode = "TeamUp";
                                    ticketTask.ipccode = ipcCodesResult;
                                    ticketTask.TaskState = 2;
                                }
                                if (i == 2)
                                {
                                    ticketTask.uloc = "H10201";
                                    ticketTask.TaskSortCode = "Auto";
                                    ticketTask.TaskState = 2;
                                }
                            }
                            ticketTask.FidCode = materialDataCode;
                            _conn.Insert<TicketsTask>(ticketTask);
                        }

                        //新增产成品工单关联表
                        var ticketProduct = new Ticket_Product();
                        ticketProduct.PlanCount = Convert.ToInt32(model.qty);
                        ticketProduct.Ass_Product_Tickets = Convert.ToInt32(pticketId);
                        ticketProduct.Ass_FinishProduct = Convert.ToInt32(finishId);
                        _conn.Insert<Ticket_Product>(ticketProduct);
                        transaction.Commit();
                        return 200;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogHelper.Info(ex.Message);
                        return 500;
                    }
                }

            }
        }

        /// <summary>
        /// 置空托盘点位
        /// </summary>
        /// <param name="positionCode"></param>
        /// <returns></returns>
        public async Task<bool> EmptyTrayInfo(string positionCode,string trayType)
        {
            var trayTypeInt = Convert.ToInt32(trayType);
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                {
                    await _conn.OpenAsync();
                }
                //判断mesorder是否存在
                var positionSql = $"select * from third_agvposition where PositionCode='{positionCode}'";
                var positionData = _conn.QueryFirstOrDefault<Third_AgvPosition>(positionSql);
                if (positionData==null)
                {
                    return false;
                }
                positionData.PositionState = 2;
                positionData.UpdateDt = DateTime.Now;
                positionData.ReqCode = "MESEmptyTray" + DateTime.Now.ToString("yyyyMMddHHmmss");
                positionData.TrayType = trayTypeInt;
                positionData.TrayData = 1;
                await _conn.UpdateAsync<Third_AgvPosition>(positionData);
                return true;
            }
        }

        /// <summary>
        /// 上报工单状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> ReportTicketState(TaskReportRequest model)
        {
            var logType = model.uloc == "H10201" ? "6" : "8";
            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "1",
                TaskNo = model.jobNo,
                DataType = logType,
                CreateDt=DateTime.Now,
                UpdateDt=DateTime.Now
            };
            string mesUrl = EngineContext.Current.Resolve<mysql>().zlMesUrl;
            var client = new RestClient(mesUrl);
            var signInfo = EngineContext.Current.Resolve<mysql>().zlMesSecret;
            var appidInfo = EngineContext.Current.Resolve<mysql>().zlMesAppid;
            var request = new RestRequest();
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
            request.AddJsonBody(model);
            var response = await client.ExecutePostAsync(request);
            logModel.DataResult = response.Content;
            _apiLogService.AddApiLogRecord(logModel);
            if (response == null)
            {
                return "";
            }
            return response.Content;
        }

        /// <summary>
        /// 合信置空涂装区域托盘
        /// </summary>
        /// <param name="positionCode"></param>
        /// <param name="trayType"></param>
        /// <returns></returns>
        public async Task<bool> EmptyRcikingTrayInfo(string positionCode, string trayType)
        {
            var trayTypeInt = 0;
            if (!string.IsNullOrEmpty(trayType))
            {
                trayTypeInt = Convert.ToInt32(trayType);
            }
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                {
                    await _conn.OpenAsync();
                }
                //判断mesorder是否存在
                var positionSql = $"select * from third_agvposition where PositionCode='{positionCode}'";
                var positionData = _conn.QueryFirstOrDefault<Third_AgvPosition>(positionSql);
                if (positionData == null)
                {
                    return false;
                }
                positionData.PositionState = 2;
                positionData.UpdateDt = DateTime.Now;
                positionData.ReqCode = "HcEmptyTray" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (trayTypeInt != 0)
                {
                    positionData.TrayType = trayTypeInt;
                }
                positionData.TrayData = 1;
                await _conn.UpdateAsync<Third_AgvPosition>(positionData);
                return true;
            }
        }

        /// <summary>
        /// 工单RFID数据
        /// </summary>
        /// <param name="rfidCode"></param>
        /// <returns></returns>
        public async Task<TicketTaskRfid> TicketTaskRFIDData(string rfidCode)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                {
                    await _conn.OpenAsync();
                }
                var selectSql = $"select * from TicketTaskRfid where rfidcode='{rfidCode}'";
                var result =await _conn.QueryFirstOrDefaultAsync<TicketTaskRfid>(selectSql);
                return result;
            }
        }

        /// <summary>
        /// 更新RFID工单状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateTicketTaskState(TicketTaskRfid model)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                {
                    await _conn.OpenAsync();
                }
                _=_conn.UpdateAsync<TicketTaskRfid>(model);
            }
        }

        /// <summary>
        /// 上报自动焊工单
        /// </summary>
        /// <param name="rfidCode"></param>
        /// <returns></returns>
        public async Task ReportAutoTicket(string rfidCode)
        {
            var mesUrl= EngineContext.Current.Resolve<mysql>().hecinMesUrl;
            var autoUrl = mesUrl + $"/FormMesOrder/ReportAutoTicket?rfidCode=" + rfidCode;
            var request = new RestRequest();
            RestClient client = new RestClient(autoUrl);
            var result =await client.GetAsync(request);
            var resultData = JsonConvert.DeserializeObject<MesMessage>(result.Content);
            if (resultData.Status == 100)
            {
                //像中联MES报工
                var postJson = new TaskReportRequest
                {
                    taskId = "auto" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    jobNo = resultData.Msg,
                    machineNo = "",
                    qty = 1,
                    reportType = "1",
                    source = "HX",
                    plantNo = "9301",
                    uloc = "H10201"
                };
                await ReportTicketState(postJson);
            }
            else
            {
                var logModel = new Third_ApiRecord
                {
                    DataInfo = rfidCode,
                    RequestType = "1",
                    TaskNo = "",
                    DataType = "3",
                    DataResult= result.Content,
                    CreateDt=DateTime.Now,
                    UpdateDt=DateTime.Now
                };
                _apiLogService.AddApiLogRecord(logModel);
            }
        }
    }
}
