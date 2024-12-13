using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Operate.Services.PlcConnectService;
using OperateHSL.Core.Configuration;
using OperateHSL.Core.DbContext;
using OperateHSL.Core.Helpers;
using OperateHSL.Core.Infrastructure;
using OperateHSL.Data.AGV;
using OperateHSL.Data.AGVCallBack;
using OperateHSL.Data.DataModel.ThirdPartModel;
using OperateHSL.Services.AgvPosition;
using OperateHSL.Services.Log4netService;
using OperateHSL.Services.LogService;
using OperateHSL.Services.PlcInfoService;
using RestSharp;

namespace OperateHSL.Services.AgvService
{
    public class AgvControlService : IAgvControlService
    {

        public static string agvUrl = EngineContext.Current.Resolve<mysql>().agvUrl;
        public RestClient client = new RestClient(agvUrl);
        public static string continueAgvUrl = EngineContext.Current.Resolve<mysql>().continueAgvUrl;
        public static string podAgvUrl = EngineContext.Current.Resolve<mysql>().podAgvUrl;

        public RestClient continueClient = new RestClient(continueAgvUrl);
        public RestClient podClient = new RestClient(podAgvUrl);


        private readonly IDbContext _dbContext;
        private readonly IApiLogService _logService;
        private readonly IAgvPositionService _agvPositionService;
        private readonly IPlcDataService _plcDataSevice;

        public AgvControlService(IDbContext dbContext, IApiLogService apiLogService,
            IAgvPositionService agvPositionService,
            IPlcDataService plcDataService)
        {
            _dbContext = dbContext;
            _logService = apiLogService;
            _agvPositionService = agvPositionService;
            _plcDataSevice = plcDataService;
        }

        public Third_AgvTask GetAgvTask(string getSql)
        {
            var _conn = _dbContext.Conn;
            var result = _conn.Query<Third_AgvTask>(getSql).FirstOrDefault();
            return result;
        }

        public CancelTaskResponse CancelTask(CancelTaskRequest model)
        {
            throw new NotImplementedException();
        }

        public ContinueTaskResponse ContinueTask(ContinueTaskRequest model)
        {
            throw new NotImplementedException();
        }

        public SchedulingTaskResponse SchedulingTask(SchedulingTaskRquest model)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 下发第一层AGV任务------------ 废弃------------
        /// </summary>
        public async Task AddAgvScheduleTask()
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                //循环AGV任务表
                var selectSql = "select * from third_agvtask where `Status`=0 and TaskStep=1 order by CreateDt limit 0,10";
                var waitTaskList = conn.Query<Third_AgvTask>(selectSql).ToList();
                client.AcceptedContentTypes = new[] { "application/json", "text/json", "text/x-json", "text/javascript", "text/plain", "*/*" };
                //下发任务信息给agv
                foreach (var itemOrder in waitTaskList)
                {
                    var postData = new SchedulingTaskRquest();
                    postData.reqCode = itemOrder.ReqCode;
                    postData.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //ZL1结构线小托盘 ZL2 下料线矩管托盘 ZL3带网的大托盘
                    postData.taskTyp = itemOrder.DataSource == 1 ? "ZL1" : "ZL2";
                    if (itemOrder.DataSource == 2)
                    {

                        postData.taskTyp = "ZL3";
                    }



                    if (itemOrder.DataSource == 11)
                    {
                        postData.priority = "2";
                    }
                    else if (itemOrder.DataSource == 6)
                    {
                        postData.priority = "3";

                    }

                    string[] agvValue = { itemOrder.StartStation, itemOrder.EndStation };
                    postData.userCallCodePath = agvValue;
                    var resultJson = await PostToAgv(postData);
                    //处理返回值
                    if (resultJson.code == "0")
                    {
                        itemOrder.UpdateDt = DateTime.Now;
                        itemOrder.Status = 1;
                        //告知下料线plc禁止落料
                        await HandlePlcData(2, itemOrder.PlaceHex);
                        await conn.UpdateAsync<Third_AgvTask>(itemOrder);
                    }
                }
            }


        }
        /// <summary>
        /// AGV任务回告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task<DataCallBackResponse> AgvTaskCallBack(DataCallBackRequest model, DataCallBackResponse result)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                {
                    await _conn.OpenAsync();
                }
                //处理agv返回结果
                var taskOrder = model.taskCode;
                var selectSql = $"select * from third_agvtask where TaskNo='{taskOrder}' and TaskType!='continueagvtask'";
                var agvTask = _conn.Query<Third_AgvTask>(selectSql).FirstOrDefault();
                if (agvTask == null)
                {
                    result.code = "1";
                    result.message = "原始AGV任务不存在";
                    result.reqCode = model.reqCode;
                    return result;
                }
                else
                {
                    var updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //这边处理还挺多的 关联的表也很多 如果是叫料任务还要回馈叫料设备的状态
                    switch (model.method)
                    {
                        case "start":
                            agvTask.Status = 1;
                            break;
                        case "outbin":
                            agvTask.Status = 2;
                            if (agvTask.DataSource == 10 || agvTask.DataSource == 5)
                            {
                                //料走 给下件区信号
                                _ = HanldeStructuralPlcData(agvTask.DataSource, agvTask.StartStation);

                            }
                            //置空初始点位
                            var emptyPositonSql = $"update third_agvposition set PositionState=1,UpdateDt='{updateTime}' where PositionCode='{agvTask.StartStation}'";
                            await _conn.ExecuteAsync(emptyPositonSql);
                            break;
                        case "end":
                            agvTask.Status = 3;
                            var endPositonSql = $"update third_agvposition set PositionState=2,UpdateDt='{updateTime}' where PositionCode='{agvTask.EndStation}'";
                            await _conn.ExecuteAsync(endPositonSql);

                            if (agvTask.DataSource == 10 || agvTask.DataSource == 5)
                            {
                                _ = HandlePCodeData(model.podCode, agvTask.EndStation, agvTask.TaskNo);
                            }
                            if (agvTask.DataSource == 5 || agvTask.DataSource == 6)
                            {
                                //送料完成 需要重置plc数据
                                _ = _agvPositionService.HanldeWeldingPlcData(agvTask.DataSource, agvTask.PlaceHex);
                                // _ = HanldeWeldingPlcData(agvTask.DataSource, agvTask.PlaceHex);
                            }
                            if (agvTask.DataSource == 7 || agvTask.DataSource == 9 || agvTask.DataSource == 11)
                            {
                                //来框完成给下件区信号
                                _ = HanldeStructuralPlcData(agvTask.DataSource, agvTask.EndStation);
                            }

                            if (agvTask.DataSource == 11)
                            {
                                int doorIds = -1;

                                int endPointInts = Convert.ToInt32(agvTask.EndStation);

                                #region 选择门         
                                if (endPointInts >= 1166 && endPointInts <= 1181)
                                {
                                    doorIds = 1;
                                    _ = DoorTaskCallBackSelf(doorIds, 2);
                                }
                                else if (endPointInts >= 1182 && endPointInts <= 1189)
                                {
                                    doorIds = 2;
                                    _ = DoorTaskCallBackSelf(doorIds, 2);
                                }
                                else if (endPointInts >= 1190 && endPointInts <= 1197)
                                {
                                    doorIds = 3;
                                    _ = DoorTaskCallBackSelf(doorIds, 2);
                                }
                                else if (endPointInts >= 1198 && endPointInts <= 1213)
                                {
                                    doorIds = 4;
                                    _ = DoorTaskCallBackSelf(doorIds, 2);
                                }
                                if (doorIds != -1)
                                {
                                    //记录日志
                                    var logMode = new Third_ApiRecord
                                    {
                                        DataInfo = doorIds + "号门关闭光栅门",
                                        RequestType = "2",
                                        TaskNo = "doorClose" + DateTime.Now.ToString("yyyyMMddHHMMss"),
                                        DataType = "4",
                                        DataResult = "该操作是桁架补框关闭光栅",
                                        CreateDt = DateTime.Now,
                                        UpdateDt = DateTime.Now
                                    };

                                    _logService.AddApiLogRecord(logMode);

                                }


                            }

                            #endregion



                            #region 按照步骤下发任务 ----------------废弃----------
                            //if (agvTask.TaskStep == 1 && taskOrder.Contains("_"))
                            //{
                            //    //发送第二波任务
                            //    var taskNo2 = taskOrder.Split('_')[0] + "_2";
                            //    var secondTask = $"select * from third_agvtask where TaskNo='{taskNo2}'";
                            //    var agvTask2 = _conn.Query<Third_AgvTask>(secondTask).FirstOrDefault();
                            //    if (agvTask2 == null)
                            //    {
                            //        LogHelper.Info("未找到对应的第二级任务，任务号为：" + taskNo2);
                            //    }
                            //    else
                            //    {
                            //        var postData = new SchedulingTaskRquest();
                            //        postData.reqCode = agvTask2.ReqCode;
                            //        postData.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //        postData.taskTyp = agvTask2.DataSource == 1 ? "ZL2" : "ZL2";
                            //        string[] agvValue = { agvTask2.StartStation, agvTask2.EndStation };
                            //        postData.userCallCodePath = agvValue;
                            //        var resultJson = await PostToAgv(postData);
                            //        //处理返回值
                            //        if (resultJson.code == "0")
                            //        {
                            //            agvTask2.UpdateDt = DateTime.Now;
                            //            agvTask2.Status = 1;
                            //            await _conn.UpdateAsync<Third_AgvTask>(agvTask2);
                            //        }
                            //    }
                            //}
                            //if (agvTask.TaskStep == 2 && taskOrder.Contains("_"))
                            //{
                            //    //空满转换完成
                            //    _ = HandlePlcData(1, agvTask.PlaceHex);
                            //}
                            #endregion
                            break;
                        case "cancel":
                            agvTask.Status = 4;
                            if (agvTask.DataSource == 10 || agvTask.DataSource == 9 || agvTask.DataSource == 5)
                            {
                                //取消任务进行中状态
                                var startPosition = await _agvPositionService.GetPositionInfo(agvTask.StartStation);
                                if (startPosition.PositionState == 3)
                                {
                                    startPosition.PositionState = 1;
                                    startPosition.UpdateDt = DateTime.Now;
                                    startPosition.PositionRemark = "任务被取消，请核对点位状态";
                                    _ = _agvPositionService.UpdatePositionInfo(startPosition);
                                }
                                //取消任务进行中状态
                                var endPosition = await _agvPositionService.GetPositionInfo(agvTask.EndStation);
                                if (endPosition.PositionState == 3)
                                {
                                    endPosition.PositionState = 2;
                                    endPosition.UpdateDt = DateTime.Now;
                                    endPosition.PositionRemark = "任务被取消，请核对点位状态";
                                    _ = _agvPositionService.UpdatePositionInfo(endPosition);
                                }

                            }
                            //把初始点位和结束点位恢复原状 要告诉AGV不允许取消任务，即使AGV出故障要在AGV修好之后把任务强制完成掉
                            break;
                        case "apply":
                        case "applyOut":
                        case "releaseOut":
                            var doorId = -1;
                            var startPointInt = Convert.ToInt32(agvTask.StartStation);
                            var endPointInt = Convert.ToInt32(agvTask.EndStation);

                            #region 选择门
                            if ((startPointInt >= 1166 && startPointInt <= 1181)
                                || (endPointInt >= 1166 && endPointInt <= 1181))
                            {
                                doorId = 1;
                            }
                            else if ((startPointInt >= 1182 && startPointInt <= 1189)
                                || (endPointInt >= 1182 && endPointInt <= 1189))
                            {
                                doorId = 2;
                            }
                            else if ((startPointInt >= 1190 && startPointInt <= 1197)
                                || (endPointInt >= 1190 && endPointInt <= 1197))
                            {
                                doorId = 3;
                            }
                            else if ((startPointInt >= 1198 && startPointInt <= 1213)
                                || (endPointInt >= 1198 && endPointInt <= 1198))
                            {
                                doorId = 4;
                            }
                            #endregion

                            if (model.method == "apply" || model.method == "applyOut")
                            {
                                //开门
                                _ = DoorTaskCallBackSelf(doorId, 1);
                                //新增继续执行任务
                                //生成AGV任务
                                Third_AgvTask third_AgvTask = new Third_AgvTask()
                                {
                                    StartStation = "",
                                    Status = 0,
                                    TaskStatus = 0,
                                    TaskType = "continueagvtask",
                                    TaskNo = agvTask.TaskNo,
                                    EndStation = "",
                                    CreateDt = DateTime.Now,
                                    TaskStep = 1,
                                    DataSource = 20,
                                    PlaceHex = -1,
                                    ReqCode = DateTime.Now.ToString("yyyyMMddHHmmss"),
                                    ReportTicketCode = ""
                                };
                                await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);
                            }
                            if (model.method == "releaseOut")
                            {
                                //出门
                                _ = DoorTaskCallBackSelf(doorId, 2);
                            }
                            break;
                        default:
                            break;
                    }

                    agvTask.UpdateDt = DateTime.Now;
                    _conn.Update<Third_AgvTask>(agvTask);

                }
                result.code = "0";
                result.message = "成功";
                result.reqCode = model.reqCode;
                return result;
            }

        }

        /// <summary>
        /// 处理光栅门数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task<DataCallBackResponse> DoorTaskCallBack(DataHanldeDoor model, DataCallBackResponse result)
        {
            await Task.Delay(10);
            #region 操作plc放行
            var s7Net = PlcBufferRack.Instance("10.99.109.240");
            var s7Net2 = PlcBufferRack.Instance("10.99.109.247");
            if (model.doorId == 1)
            {
                if (model.openType == 1)
                {
                    s7Net.Write("DB110.76.2", true);
                }
                else
                {
                    s7Net.Write("DB110.76.2", false);
                }
            }
            else if (model.doorId == 2)
            {
                if (model.openType == 1)
                {
                    s7Net.Write("DB110.76.3", true);
                }
                else
                {
                    s7Net.Write("DB110.76.3", false);
                }
            }
            else if (model.doorId == 3)
            {
                if (model.openType == 1)
                {
                    s7Net2.Write("DB110.76.2", true);
                }
                else
                {
                    s7Net2.Write("DB110.76.2", false);
                }
            }
            else if (model.doorId == 4)
            {
                if (model.openType == 1)
                {
                    s7Net2.Write("DB110.76.3", true);
                }
                else
                {
                    s7Net2.Write("DB110.76.3", false);
                }
            }

            #endregion

            result.code = "0";
            result.message = "成功";
            result.reqCode = model.reqCode;
            return result;
        }

        public async Task DoorTaskCallBackSelf(int doorId, int openType)
        {
            #region 操作plc放行
            var s7Net = PlcBufferRack.Instance("10.99.109.240");
            var s7Net2 = PlcBufferRack.Instance("10.99.109.247");
            if (doorId == 1)
            {
                if (openType == 1)
                {
                    s7Net.Write("DB110.DBX76.2", true);
                    s7Net.Write("DB110.DBX76.3", true);
                }
                else
                {
                    s7Net.Write("DB110.DBX76.2", false);
                    s7Net.Write("DB110.DBX76.3", false);
                }

            }
            else if (doorId == 2)
            {
                if (openType == 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        s7Net.Write("DB110.DBX76.3", true);
                        s7Net.Write("DB110.DBX76.2", true);
                    }

                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        s7Net.Write("DB110.DBX76.3", false);
                        s7Net.Write("DB110.DBX76.2", false);
                    }

                }
            }
            else if (doorId == 3)
            {
                if (openType == 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        s7Net2.Write("DB110.DBX76.2", true);
                        s7Net2.Write("DB110.DBX76.3", true);
                    }

                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        s7Net2.Write("DB110.DBX76.2", false);
                        s7Net2.Write("DB110.DBX76.3", false);
                    }
                }
            }
            else if (doorId == 4)
            {
                if (openType == 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        s7Net2.Write("DB110.DBX76.3", true);
                        s7Net2.Write("DB110.DBX76.2", true);
                    }

                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        s7Net2.Write("DB110.DBX76.3", false);
                        s7Net2.Write("DB110.DBX76.2", false);
                    }
                }
            }

            #endregion

            #region 记录日志
            var operateReason = openType == 1 ? "打开" : "关闭";
            //记录日志
            var logModel = new Third_ApiRecord
            {
                DataInfo = doorId + "号门请求" + operateReason + "光栅门",
                RequestType = "2",
                TaskNo = "doorcontrol" + DateTime.Now.ToString("yyyyMMddHHMMss"),
                DataType = "4",
                DataResult = "该操作目前设置为全开全关",
                CreateDt = DateTime.Now,
                UpdateDt = DateTime.Now
            };
            _logService.AddApiLogRecord(logModel);
            #endregion

            await Task.Delay(10);
        }


        public async Task DoorTaskClose(int doorId)
        {
            #region 操作plc放行
            var s7Net = PlcBufferRack.Instance("10.99.109.240");
            var s7Net2 = PlcBufferRack.Instance("10.99.109.247");
            if (doorId == 2)
            {
                var resullt = s7Net.ReadBool("DB110.DBX76.3").Content;
                if (Convert.ToBoolean(resullt) == true)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        s7Net.Write("DB110.DBX76.3", false);

                    }
                }
                var resullt1 = s7Net.ReadBool("DB110.DBX76.2").Content;
                if (Convert.ToBoolean(resullt1) == true)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        s7Net.Write("DB110.DBX76.2", false);

                    }
                }
            }
            else if (doorId == 3)
            {
                var resullt = s7Net2.ReadBool("DB110.DBX76.3").Content;
                if (Convert.ToBoolean(resullt) == true)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        s7Net2.Write("DB110.DBX76.3", false);

                    }
                }
                var resullt1 = s7Net2.ReadBool("DB110.DBX76.2").Content;
                if (Convert.ToBoolean(resullt1) == true)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        s7Net2.Write("DB110.DBX76.2", false);

                    }
                }

            }

            await Task.Delay(10);
            #endregion



        }


        public async Task<SchedulingTaskResponse> PostToAgv(SchedulingTaskRquest model)
        {
            //生成AGV请求数据
            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model);
            var response = await client.ExecutePostAsync(request);
            var resultContent = response.Content;
            if (resultContent == "" || resultContent == "[]" || resultContent == "")
            {

            }
            //记录日志
            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "2",
                TaskNo = model.reqCode,
                DataType = "1",
                DataResult = response.Content,
                CreateDt = DateTime.Now,
                UpdateDt = DateTime.Now
            };
            _logService.AddApiLogRecord(logModel);
            try
            {
                var resultJson = JsonConvert.DeserializeObject<SchedulingTaskResponse>(response.Content);
                return resultJson;
            }
            catch (Exception ex)
            {
                LogHelper.Error("解析AGV添加任务返回数据时发生错误，AGV返回值：" + response.Content, ex);
                return null;
            }
        }


        public async Task<SchedulingTaskResponse> PostContinueToAgv(ContinueTaskRequest model)
        {
            //生成AGV请求数据
            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model);
            var response = await continueClient.ExecutePostAsync(request);
            var resultContent = response.Content;
            if (resultContent == "" || resultContent == "[]" || resultContent == "")
            {

            }
            //记录日志
            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "2",
                TaskNo = model.reqCode,
                DataType = "1",
                DataResult = response.Content,
                CreateDt = DateTime.Now,
                UpdateDt = DateTime.Now
            };
            _logService.AddApiLogRecord(logModel);
            try
            {
                var resultJson = JsonConvert.DeserializeObject<SchedulingTaskResponse>(response.Content);
                return resultJson;
            }
            catch (Exception ex)
            {
                LogHelper.Error("解析AGV继续任务返回数据时发生错误，AGV返回值：" + response.Content, ex);
                return null;
            }
        }


        public async Task<SchedulingTaskResponse> BindPodAgvTask(BindPodAndBerth model)
        {
            podClient.AcceptedContentTypes = new[] { "application/json", "text/json", "text/x-json", "text/javascript", "text/plain", "*/*" };
            //生成AGV请求数据
            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model);
            var response = await podClient.ExecutePostAsync(request);
            var resultContent = response.Content;
            if (resultContent == "" || resultContent == "[]" || resultContent == "")
            {

            }
            //记录日志
            var logModel = new Third_ApiRecord
            {
                DataInfo = JsonConvert.SerializeObject(model),
                RequestType = "2",
                TaskNo = model.reqCode,
                DataType = "1",
                DataResult = response.Content,
                CreateDt = DateTime.Now,
                UpdateDt = DateTime.Now
            };
            _logService.AddApiLogRecord(logModel);
            try
            {
                var resultJson = JsonConvert.DeserializeObject<SchedulingTaskResponse>(response.Content);
                return resultJson;
            }
            catch (Exception ex)
            {
                LogHelper.Error("解析清空托盘绑定出错，AGV返回值：" + response.Content, ex);
                return null;
            }
        }

        /// <summary>
        /// plc通知是否可以落料
        /// </summary>
        /// <returns></returns>
        public async Task HandlePlcData(short type, int placeHex)
        {
            var s7Net = PlcBufferRack.Instance("10.99.110.171");
            if (placeHex == 1)
            {
                s7Net.Write("DB2.282.0", type);
            }
            else if (placeHex == 2)
            {
                s7Net.Write("DB2.284.0", type);
            }
            else if (placeHex == 3)
            {
                s7Net.Write("DB2.286.0", type);
            }
            else if (placeHex == 4)
            {
                s7Net.Write("DB2.288.0", type);
            }
            else if (placeHex == 5)
            {
                s7Net.Write("DB2.290.0", type);
            }
            else if (placeHex == 6)
            {
                s7Net.Write("DB2.292.0", type);
            }
            else if (placeHex == 7)
            {
                s7Net.Write("DB2.294.0", type);
            }
            else if (placeHex == 8)
            {
                s7Net.Write("DB2.296.0", type);
            }
            else if (placeHex == 9)
            {
                s7Net.Write("DB2.298.0", type);
            }
            else if (placeHex == 10)
            {
                s7Net.Write("DB2.300.0", type);
            }
            else if (placeHex == 11)
            {
                s7Net.Write("DB2.302.0", type);
            }
            else if (placeHex == 12)
            {
                s7Net.Write("DB2.304.0", type);
            }
            await Task.Delay(10);
        }

        /// <summary>
        /// 处理打磨信号反馈
        /// </summary>
        /// <param name="type">5:叫料 6：送料</param>
        /// <param name="placeHex"></param>
        /// <returns></returns>
        public async Task HanldeWeldingPlcData(int type, int placeHex)
        {
            var s7Net = PlcBufferRack.Instance("10.99.108.161");
            var handleType = 8;
            if (type == 6)
            {
                handleType = 12;
            }
            if (placeHex == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    s7Net.Write("DB100." + handleType + ".0", true);
                }

            }
            else if (placeHex == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    s7Net.Write("DB100." + handleType + ".1", true);
                }

            }
            else if (placeHex == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    s7Net.Write("DB100." + handleType + ".2", true);
                }

            }
            else if (placeHex == 4)
            {
                for (int i = 0; i < 3; i++)
                {
                    s7Net.Write("DB100." + handleType + ".3", true);
                }

            }
            else if (placeHex == 5)
            {
                for (int i = 0; i < 3; i++)
                {
                    s7Net.Write("DB100." + handleType + ".4", true);
                }

            }
            else if (placeHex == 6)
            {
                for (int i = 0; i < 3; i++)
                {
                    s7Net.Write("DB100." + handleType + ".5", true);
                }

            }
            else if (placeHex == 7)
            {
                for (int i = 0; i < 3; i++)
                {
                    s7Net.Write("DB100." + handleType + ".6", true);
                }

            }
            else if (placeHex == 8)
            {
                for (int i = 0; i < 3; i++)
                {
                    s7Net.Write("DB100." + handleType + ".7", true);
                }

            }
            else if (placeHex == 9)
            {
                for (int i = 0; i < 3; i++)
                {
                    var handleNew = handleType + 1;
                    s7Net.Write("DB100." + handleNew + ".0", true);
                }

            }
            else if (placeHex == 10)
            {
                for (int i = 0; i < 3; i++)
                {
                    var handleNew = handleType + 1;
                    s7Net.Write("DB100." + handleNew + ".1", true);
                }

            }
            else if (placeHex == 11)
            {
                for (int i = 0; i < 3; i++)
                {
                    var handleNew = handleType + 1;
                    s7Net.Write("DB100." + handleNew + ".2", true);
                }

            }
            else if (placeHex == 12)
            {
                for (int i = 0; i < 3; i++)
                {
                    var handleNew = handleType + 1;
                    s7Net.Write("DB100." + handleNew + ".3", true);
                }

            }
            await Task.Delay(10);
        }


        public async Task HandlePCodeData(string podCode, string positionCode, string taskCode)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                {
                    await _conn.OpenAsync();
                }
                var podData = new Third_AgvPodCode
                {
                    LastTaskCode = taskCode,
                    podCode = podCode,
                    PositionCode = positionCode,
                    CreateDt = DateTime.Now,
                    UpdateDt = DateTime.Now
                };
                _ = _conn.InsertAsync<Third_AgvPodCode>(podData);
            }
        }


        /// <summary>
        /// 处理下件送走以及送回
        /// </summary>
        /// <param name="type">10:送走 9,7：送回</param>
        /// <param name="placeHex"></param>
        /// <returns></returns>
        public async Task HanldeStructuralPlcData(int type, string positionCode)
        {
            var positionInfo = await _agvPositionService.GetAgvPlcPostions();
            var currentData = positionInfo.FirstOrDefault(p => p.AgvAddress == positionCode);
            if (currentData == null)
            {
                return;
            }
            else
            {
                var s7Net = PlcBufferRack.Instance(currentData.PlcAddress);
                if (type == 10 || type == 5)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        s7Net.Write(currentData.PlcIpInfo, false);
                    }
                }
                else if (type == 7 || type == 9 || type == 11)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        s7Net.Write(currentData.PlcIpInfo, true);
                    }
                }
            }

            await Task.Delay(10);
        }


        #region 组队AGV储位管理
        /// <summary>
        /// 组队完成下料
        /// </summary>
        /// <param name="positionHex">plc中对应的agv点位</param>
        /// <param name="materialName">物料名称</param>
        /// <param name="materialCount">物料数量</param>
        /// <returns></returns>
        public async Task CompleteTeamUpPosition(int positionHex, string materialName, int materialCount)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                {
                    await _conn.OpenAsync();
                }

                //判断储位状态是否处于待托运状态
                var currentPoistionSql = $"select * from Third_AgvPosition where PositionHex='{positionHex}'";
                var currentPoistion = _conn.QueryFirstOrDefault<Third_AgvPosition>(currentPoistionSql);
                if (currentPoistion == null)
                {
                    LogHelper.Error("没有找到对应的组队托盘：" + positionHex, null);
                }
                else
                {
                    currentPoistion.PositionRemark = materialName;
                    currentPoistion.TrayDataCount = materialCount;
                    if (currentPoistion.PositionState != 2 || currentPoistion.TrayData != 2)
                    {
                        currentPoistion.PositionState = 2;
                        currentPoistion.TrayData = 2;
                    }
                    _conn.Update<Third_AgvPosition>(currentPoistion);
                }
            }
        }
        #endregion


        #region 行架下料AGV储位管理

        /// <summary>
        /// 修改储位数据
        /// </summary>
        /// <param name="position"></param>
        /// <param name="materialName"></param>
        /// <param name="materialCount"></param>
        /// <returns></returns>
        public async Task TrussesDownPosition(string position, string materialName, int materialCount)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                {
                    await _conn.OpenAsync();
                }
                //判断行架下料储位状态是否处于运行中
                var currentPoistionSql = $"select * from Third_AgvPosition where PositionCode='{position}'";
                var currentPoistion = _conn.Query<Third_AgvPosition>(currentPoistionSql).FirstOrDefault();
                var postionState = currentPoistion.PositionState;
                if (postionState == 3)
                {
                    LogHelper.Info($"行架下料区位置:{position}有任务在执行");
                }
                else
                {
                    currentPoistion.PositionRemark = materialName;
                    currentPoistion.TrayDataCount = materialCount;
                    if (currentPoistion.PositionState != 2 || currentPoistion.TrayData != 2)
                    {
                        currentPoistion.PositionState = 2;
                        currentPoistion.TrayData = 2;
                    }
                    _conn.Update<Third_AgvPosition>(currentPoistion);
                }
            }
        }




        /// <summary>
        /// 补焊房任务执行调度agv
        /// </summary>
        /// <returns></returns>
        public async Task AddrequireHAgvScheduleTaskAsync()
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }


                //循环AGV任务表
                var selectSql = "select * from third_agvtask where `Status`=0 and TaskStep=1 and " +
                    "(DataSource=1 ||DataSource=3 || DataSource=5 || DataSource=6|| DataSource=7 || DataSource=8 || DataSource=9 || DataSource=10|| DataSource=11 || DataSource=20) order by CreateDt limit 0,3";
                var waitTaskList = conn.Query<Third_AgvTask>(selectSql).ToList();
                client.AcceptedContentTypes = new[] { "application/json", "text/json", "text/x-json", "text/javascript", "text/plain", "*/*" };
                //下发任务信息给agv
                foreach (var itemOrder in waitTaskList)
                {
                    if (itemOrder.DataSource != 20)
                    {
                        var postData = new SchedulingTaskRquest();
                        postData.reqCode = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        postData.taskCode = itemOrder.TaskNo;
                        postData.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        var taskTyp = "ZL1";
                        if (itemOrder.DataSource == 5 || itemOrder.DataSource == 6 || itemOrder.DataSource == 7
                            || itemOrder.DataSource == 8 || itemOrder.DataSource == 9
                            )
                        {
                            taskTyp = "ZL1";
                        }
                        else if (itemOrder.DataSource == 1 || itemOrder.DataSource == 3 || itemOrder.DataSource == 4)
                        {
                            taskTyp = "ZL2";
                        }
                        else if (itemOrder.DataSource == 10)
                        {
                            taskTyp = "ZL5";
                        }
                        else if (itemOrder.DataSource == 11)
                        {
                            taskTyp = "ZL6";
                        }
                        postData.taskTyp = taskTyp;
                        string[] agvValue = { itemOrder.StartStation, itemOrder.EndStation };
                        postData.userCallCodePath = agvValue;
                        var resultJson = await PostToAgv(postData);
                        //处理返回值
                        if (resultJson == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (resultJson != null && resultJson.message.Contains("成功"))
                            {
                                itemOrder.UpdateDt = DateTime.Now;
                                itemOrder.Status = 1;
                                await conn.UpdateAsync<Third_AgvTask>(itemOrder);
                                //置空起始点位
                                var startPosition = await _agvPositionService.GetPositionInfo(itemOrder.StartStation);
                                var endPosition = await _agvPositionService.GetPositionInfo(itemOrder.EndStation);
                                endPosition.PositionState = 3;
                                endPosition.UpdateDt = DateTime.Now;
                                endPosition.ReqCode = itemOrder.TaskNo;
                                if (itemOrder.DataSource == 9 || itemOrder.DataSource == 11)
                                {
                                    endPosition.TrayData = 1;
                                }
                                else
                                {
                                    endPosition.TrayData = 2;
                                }
                                if (itemOrder.DataSource != 11)
                                {
                                    endPosition.TrayType = startPosition.TrayType;
                                }
                                if (itemOrder.DataSource == 3)
                                {
                                    endPosition.TrayType = 3;

                                }

                                endPosition.PositionRemark = startPosition.PositionRemark;
                                _ = _agvPositionService.UpdatePositionInfo(endPosition);

                                startPosition.PositionState = 3;
                                startPosition.UpdateDt = DateTime.Now;
                                startPosition.ReqCode = itemOrder.TaskNo;
                                startPosition.UpdateDt = DateTime.Now;
                                startPosition.TrayData = 0;
                                _ = _agvPositionService.UpdatePositionInfo(startPosition);
                            }
                        }
                    }
                    else
                    {
                        var postContinueData = new ContinueTaskRequest();
                        postContinueData.reqCode = itemOrder.ReqCode;
                        postContinueData.taskCode = itemOrder.TaskNo;
                        postContinueData.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        var resultJson = await PostContinueToAgv(postContinueData);
                        //处理返回值
                        if (resultJson == null)
                        {
                            continue;
                        }
                        else
                        {
                            itemOrder.UpdateDt = DateTime.Now;
                            itemOrder.Status = 3;
                            await conn.UpdateAsync<Third_AgvTask>(itemOrder);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 光栅信号处理
        /// </summary>
        /// <returns></returns>
        public async Task GSScheduleTaskAsync()
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                //查找正在执行的任务
                var selectSql = "select * from third_agvtask where (`Status`=2  or `Status`=1) and ((EndStation>= 1166  && EndStation <= 1189) or (StartStation>= 1166  && StartStation <= 1189))";
                var waitTaskList = conn.Query<Third_AgvTask>(selectSql).ToList();
                if (waitTaskList.Count == 0)
                {
                    //关闭北侧光栅
                    _ = DoorTaskClose(2);
                }
                var selectSql1 = "select * from third_agvtask where (`Status`=2  or `Status`=1) and ((EndStation>= 1190  && EndStation <= 1213) or (StartStation>= 1166  && StartStation <= 1189))";

                var waitTaskList1 = conn.Query<Third_AgvTask>(selectSql1).ToList();
                if (waitTaskList1.Count == 0)
                {
                    //关闭南侧光栅
                    _ = DoorTaskClose(3);
                }

            }
        }


        /// <summary>
        /// 清空AGV托盘占用
        /// </summary>
        /// <returns></returns>
        public async Task ClearTrayPosition()
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                //循环AGV任务表
                var selectSql = "select * from third_agvpodcode where datastate=0 order by createdt desc limit 0,2";
                var waitPodList = conn.Query<Third_AgvPodCode>(selectSql).ToList();

                //下发任务信息给agv
                foreach (var itemPod in waitPodList)
                {
                    var postData = new BindPodAndBerth
                    {
                        reqCode = "req" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        podCode = itemPod.podCode,
                        positionCode = itemPod.PositionCode,
                        podDir = "1",
                        indBind = "0"
                    };
                    var resultJson = await BindPodAgvTask(postData);
                    //处理返回值
                    if (resultJson == null)
                    {
                        continue;
                    }
                    else
                    {
                        itemPod.UpdateDt = DateTime.Now;
                        itemPod.DataState = 1;
                        await conn.UpdateAsync<Third_AgvPodCode>(itemPod);
                    }
                }
            }
        }
        #endregion

        #region 接口关闭信号
        public async Task CloseWeldingRoomData(int position, int type)
        {
            var s7Net = PlcBufferRack.Instance("10.99.108.161");
            if (position == -1)
            {
                var handleType = 8;
                if (type == 2)
                {
                    handleType = 12;
                }
                s7Net.Write("DB100." + handleType + ".0", true);
                s7Net.Write("DB100." + handleType + ".1", true);
                s7Net.Write("DB100." + handleType + ".2", true);
                s7Net.Write("DB100." + handleType + ".3", true);
                s7Net.Write("DB100." + handleType + ".4", true);
                s7Net.Write("DB100." + handleType + ".5", true);
                s7Net.Write("DB100." + handleType + ".6", true);
                s7Net.Write("DB100." + handleType + ".7", true);
                var newHandle = handleType + 1;
                s7Net.Write("DB100." + newHandle + ".0", true);
                s7Net.Write("DB100." + newHandle + ".1", true);
                s7Net.Write("DB100." + newHandle + ".2", true);
                s7Net.Write("DB100." + newHandle + ".3", true);
            }
            else
            {
                var typeNew = type == 1 ? 5 : 6;
                await HanldeWeldingPlcData(typeNew, position);
            }
        }

        /// <summary>
        /// 处理下件区信号
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task HandleStructuralSignal(string position, int type)
        {
            var typeNew = type == 1 ? 5 : 7;
            await HanldeStructuralPlcData(typeNew, position);
        }
        #endregion

        #region 处理置空托盘AGV任务
        /// <summary>
        /// 下件区空托盘补位
        /// </summary>
        /// <returns></returns>
        public async Task AddTransRickingEmptyTrayTask()
        {
            //1.优先缓存区托盘补位
            var struEmptyTray = await _agvPositionService.GetStructuralEmptyTray();
            var struEmptyStepOnePositon = await _agvPositionService.GetStructuralEmptyPosition();
            var usedOneCode = new List<string>();
            foreach (var itemStepOnePostion in struEmptyStepOnePositon)
            {
                var hasTask = _plcDataSevice.PositionEndIsExistAgvTask(itemStepOnePostion.PositionCode);
                if (hasTask)
                {
                    continue;
                }
                else
                {
                    //1190 1191
                    var areaPartTwo = itemStepOnePostion.AreaPartTwo;
                    var choosePartTwo = areaPartTwo == 1 ? 3 : 4;
                    var strucEmpty = struEmptyTray.FirstOrDefault(p => p.TrayType == itemStepOnePostion.TrayType
                    && !usedOneCode.Contains(p.PositionCode)
                    && p.AreaPartTwo == choosePartTwo);
                    if (strucEmpty != null)
                    {
                        usedOneCode.Add(strucEmpty.PositionCode);
                        //生成AGV任务
                        Third_AgvTask third_AgvTask = new Third_AgvTask()
                        {
                            StartStation = strucEmpty.PositionCode,
                            Status = 0,
                            TaskStatus = 0,
                            TaskType = "下件区缓存位补空托盘",
                            TaskNo = $"StrToStr" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                            EndStation = itemStepOnePostion.PositionCode,
                            CreateDt = DateTime.Now,
                            TaskStep = 1,
                            DataSource = 9,
                            PlaceHex = -1,
                            ReqCode = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                            ReportTicketCode = ""
                        };
                        await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);

                        await HanldeStructuralPlcData(10, strucEmpty.PositionCode);

                        //修改起始点和终点状态
                        strucEmpty.PositionState = 3;
                        strucEmpty.PositionRemark = "下件区缓存位补空托盘";
                        strucEmpty.UpdateDt = DateTime.Now;
                        await _agvPositionService.UpdatePositionInfo(strucEmpty);
                        itemStepOnePostion.PositionState = 3;
                        itemStepOnePostion.PositionRemark = "";
                        itemStepOnePostion.UpdateDt = DateTime.Now;
                        await _agvPositionService.UpdatePositionInfo(itemStepOnePostion);
                    }
                }
            }


            //2处理下件区空托盘回流
            //获取下件区有多少空置点位
            //var struEmptyPositon = await _agvPositionService.GetStructuralEmptyPosition();
            //if (struEmptyPositon != null)
            //{
            //    var emtyPositionCount = struEmptyPositon.Count();
            //    if (emtyPositionCount > 0)
            //    {
            //        //2.获取下件区空托盘数据
            //        var rickingEmptyTrays = await _agvPositionService.GetRickingEmptyTray(emtyPositionCount);
            //        if (rickingEmptyTrays != null)
            //        {

            //            var endWait = struEmptyPositon.Where(p => (p.AreaPartTwo == 3 || p.AreaPartTwo == 4)).ToList();
            //            var usedCode = new List<string>();

            //            #region 优先补给下件区托盘
            //            //var endUse = struEmptyPositon.Where(p => (p.AreaPartTwo == 1 || p.AreaPartTwo == 3)).ToList();
            //            //foreach (var itemUse in endUse)
            //            //{
            //            //    //获取使用托盘托盘类型
            //            //    var trayType = itemUse.TrayType;
            //            //    var rickingEmpty = rickingEmptyTrays.FirstOrDefault(p => p.TrayType == trayType && !usedCode.Contains(p.PositionCode));
            //            //    if (rickingEmpty != null)
            //            //    {
            //            //        usedCode.Add(rickingEmpty.PositionCode);
            //            //        //生成AGV任务
            //            //        Third_AgvTask third_AgvTask = new Third_AgvTask()
            //            //        {
            //            //            StartStation = rickingEmpty.PositionCode,
            //            //            Status = 0,
            //            //            TaskStatus = 0,
            //            //            TaskType = "上挂区送走空托盘",
            //            //            TaskNo = $"RikToStr" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            //            //            EndStation = itemUse.PositionCode,
            //            //            CreateDt = DateTime.Now,
            //            //            TaskStep = 1,
            //            //            DataSource = 8,
            //            //            PlaceHex = -1,
            //            //            ReqCode = DateTime.Now.ToString("yyyyMMddHHmmss"),
            //            //            ReportTicketCode = ""
            //            //        };
            //            //        await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);
            //            //        //修改起始点和终点状态
            //            //        itemUse.PositionState = 3;
            //            //        itemUse.PositionRemark = "上挂区返还空托盘";
            //            //        itemUse.UpdateDt = DateTime.Now;
            //            //        await _agvPositionService.UpdatePositionInfo(itemUse);
            //            //        rickingEmpty.PositionState = 3;
            //            //        rickingEmpty.PositionRemark = "";
            //            //        rickingEmpty.UpdateDt = DateTime.Now;
            //            //        await _agvPositionService.UpdatePositionInfo(rickingEmpty);

            //            //    }
            //            //}
            //            #endregion
            //            //先分段补给 后续在考虑优先补给下件区
            //            foreach (var itemWait in endWait)
            //            {
            //                //获取使用托盘托盘类型
            //                var trayType = itemWait.TrayType;
            //                var rickingEmpty = rickingEmptyTrays.FirstOrDefault(p => p.TrayType == trayType && !usedCode.Contains(p.PositionCode));
            //                if (rickingEmpty != null)
            //                {
            //                    usedCode.Add(rickingEmpty.PositionCode);
            //                    //生成AGV任务
            //                    Third_AgvTask third_AgvTask = new Third_AgvTask()
            //                    {
            //                        StartStation = rickingEmpty.PositionCode,
            //                        Status = 0,
            //                        TaskStatus = 0,
            //                        TaskType = "上挂区送走空托盘到缓存位置",
            //                        TaskNo = $"RikToStrWait" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            //                        EndStation = itemWait.PositionCode,
            //                        CreateDt = DateTime.Now,
            //                        TaskStep = 1,
            //                        DataSource = 8,
            //                        PlaceHex = -1,
            //                        ReqCode = DateTime.Now.ToString("yyyyMMddHHmmss"),
            //                        ReportTicketCode = ""
            //                    };
            //                    await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);
            //                    //修改起始点和终点状态
            //                    itemWait.PositionState = 3;
            //                    itemWait.PositionRemark = "上挂区返还空托盘到缓存位置";
            //                    itemWait.UpdateDt = DateTime.Now;
            //                    await _agvPositionService.UpdatePositionInfo(itemWait);
            //                    rickingEmpty.PositionState = 3;
            //                    rickingEmpty.PositionRemark = "";
            //                    rickingEmpty.UpdateDt = DateTime.Now;
            //                    await _agvPositionService.UpdatePositionInfo(rickingEmpty);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 下件区满托盘送到人工打磨缓存区
        /// </summary>
        /// <returns></returns>
        public async Task AddTransStrusFullTrayTask()
        {
            //获取所有满框托盘
            var fullTrays = await _agvPositionService.GetStructrualEndPositionDatas();
            //获取缓存区所有空置点位
            var emptyPositions = await _agvPositionService.GetRepaireEmptyPositionDatas();

            var usedPositions = new List<string>();

            foreach (var fullItem in fullTrays)
            {
                var hasTask = _plcDataSevice.PositionStartIsExistAgvTask(fullItem.PositionCode);
                if (hasTask)
                {
                    continue;
                }
                else
                {
                    var emptyPositionItem = emptyPositions.FirstOrDefault(p => !usedPositions.Contains(p.PositionCode));
                    if (emptyPositionItem != null)
                    {
                        usedPositions.Add(emptyPositionItem.PositionCode);
                        //生成AGV任务
                        //生成AGV任务
                        Third_AgvTask third_AgvTask = new Third_AgvTask()
                        {
                            StartStation = fullItem.PositionCode,
                            Status = 0,
                            TaskStatus = 0,
                            TaskType = "下件区送走托盘到打磨缓存区",
                            TaskNo = $"StToRu" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                            EndStation = emptyPositionItem.PositionCode,
                            CreateDt = DateTime.Now,
                            TaskStep = 1,
                            DataSource = 10,
                            PlaceHex = 1,
                            ReqCode = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                            ReportTicketCode = ""
                        };
                        await _plcDataSevice.AddAgvTaskAsync(third_AgvTask);
                        //修改起始点和终点状态
                        emptyPositionItem.PositionState = 3;
                        emptyPositionItem.PositionRemark = fullItem.PositionRemark;
                        //emptyPositionItem.TrayType = fullItem.TrayType;
                        emptyPositionItem.UpdateDt = DateTime.Now;
                        await _agvPositionService.UpdatePositionInfo(emptyPositionItem);
                        fullItem.PositionState = 3;
                        fullItem.UpdateDt = DateTime.Now;
                        await _agvPositionService.UpdatePositionInfo(fullItem);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        #endregion
    }
}