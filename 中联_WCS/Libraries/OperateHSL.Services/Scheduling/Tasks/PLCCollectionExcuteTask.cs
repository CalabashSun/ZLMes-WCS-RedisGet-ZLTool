using Newtonsoft.Json;
using Operate.Services.PlcConnectService;
using OperateHSL.Core.Caching;
using OperateHSL.Core.Configuration;
using OperateHSL.Core.Infrastructure;
using OperateHSL.Core.Tool;
using OperateHSL.Data.IMes;
using OperateHSL.Data.PLC;
using OperateHSL.Data.UsedModel.TaskOrder;
using OperateHSL.Services.LogService;
using OperateHSL.Services.PlcInfoService;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{
    public class PLCCollectionExcuteTask : IScheduledTask
    {
        private readonly ICacheManager _caheManager;
        private readonly IApiLogService _logService;
        public static string hecinMesUrl = EngineContext.Current.Resolve<mysql>().hecinMesUrl;
        private readonly IPlcDataService _plcDataSevice;
        public static RestClient client = new RestClient(hecinMesUrl);
        public PLCCollectionExcuteTask(ICacheManager cacheManager,IApiLogService logService,
            IPlcDataService plcDataService
            )
        {
            _caheManager = cacheManager;
            _logService = logService;
            _plcDataSevice = plcDataService;
        }


        public int Schedule => 50000000;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkData = new List<string>();
            Console.WriteLine("下料线任务处理开始" + DateTime.Now);
            checkData.Add("下料线任务处理开始" + DateTime.Now);

            var handleData = _caheManager.Get<byte[]>("slingcingresult_001");
            var dataInfos = _plcDataSevice.GetCollectionPlcDb("001");
            var s7Net = PlcBufferRack.Instance(dataInfos.plc_ip);
            if (handleData != null)
            {
                var address = dataInfos.plc_dbadr;
                //处理订单
                Task task1 = Task.Factory.StartNew(async () =>
                {
                    #region 订单 
                    var needOrder = new List<int>();
                    var completedOrder = new Dictionary<int,string>();
                    #region 订单处理
                    //1号订单位
                    var OrderOnePLCAdd = AnalysisPLCData.GetData(1102, "Bool", handleData);
                    var OrderOnePLCState = AnalysisPLCData.GetData(1104, "Int", handleData);
                    var OrderNoOne = AnalysisPLCData.GetData(2, "String[32]", handleData);
                    if ((Convert.ToBoolean(OrderOnePLCAdd) == false && OrderOnePLCState != "2" && string.IsNullOrEmpty(OrderNoOne))
                        || (Convert.ToBoolean(OrderOnePLCAdd) == false && OrderOnePLCState == "2" && !string.IsNullOrEmpty(OrderNoOne))
                        )
                    {
                        needOrder.Add(1);
                        if (!string.IsNullOrEmpty(OrderNoOne))
                        {
                            //异步通知订单完成
                            completedOrder.Add(1,OrderNoOne);
                        }
                    }
                    if (Convert.ToBoolean(OrderOnePLCAdd) == true)
                    {
                        s7Net.Write(address + ".280.0", false);
                    }
                    //2号订单位
                    var OrderTwoPLCAdd = AnalysisPLCData.GetData((decimal)1102.1, "Bool", handleData);
                    var OrderTwoPLCState = AnalysisPLCData.GetData(1106, "Int", handleData);
                    var OrderNoTwo = AnalysisPLCData.GetData(44, "String[32]", handleData);
                    if ((Convert.ToBoolean(OrderTwoPLCAdd) == false && OrderTwoPLCState != "2" && string.IsNullOrEmpty(OrderNoTwo))
                        || (Convert.ToBoolean(OrderTwoPLCAdd) == false && OrderTwoPLCState == "2" && !string.IsNullOrEmpty(OrderNoTwo))
                        )
                    {
                        needOrder.Add(2);
                        if (!string.IsNullOrEmpty(OrderNoTwo))
                        {
                            //异步通知订单完成
                            completedOrder.Add(2,OrderNoTwo);
                        }
                    }
                    if (Convert.ToBoolean(OrderTwoPLCAdd) == true)
                    {
                        s7Net.Write(address + ".280.1", false);
                    }
                    //3号订单位
                    var OrderThreePLCAdd = AnalysisPLCData.GetData((decimal)1102.2, "Bool", handleData);
                    var OrderThreePLCState = AnalysisPLCData.GetData(1108, "Int", handleData);
                    var OrderNoThree = AnalysisPLCData.GetData(86, "String[32]", handleData);
                    if ((Convert.ToBoolean(OrderThreePLCAdd) == false && OrderThreePLCState != "2" && string.IsNullOrEmpty(OrderNoThree))
                        || (Convert.ToBoolean(OrderThreePLCAdd) == false && OrderThreePLCState == "2" && !string.IsNullOrEmpty(OrderNoThree))
                        )
                    {
                        needOrder.Add(3);
                        if (!string.IsNullOrEmpty(OrderNoThree))
                        {
                            //异步通知订单完成
                            completedOrder.Add(3,OrderNoThree);
                        }
                    }
                    if (Convert.ToBoolean(OrderThreePLCAdd) == true)
                    {
                        s7Net.Write(address + ".280.2", false);
                    }
                    //4号订单位
                    var OrderFourPLCAdd = AnalysisPLCData.GetData((decimal)1102.3, "Bool", handleData);
                    var OrderFourPLCState = AnalysisPLCData.GetData(1110, "Int", handleData);
                    var OrderNoFour = AnalysisPLCData.GetData(128, "String[32]", handleData);
                    if ((Convert.ToBoolean(OrderFourPLCAdd) == false && OrderFourPLCState != "2" && string.IsNullOrEmpty(OrderNoFour))
                        || (Convert.ToBoolean(OrderFourPLCAdd) == false && OrderFourPLCState == "2" && !string.IsNullOrEmpty(OrderNoFour))
                        )
                    {
                        needOrder.Add(4);
                        if (!string.IsNullOrEmpty(OrderNoFour))
                        {
                            //异步通知订单完成
                            completedOrder.Add(4,OrderNoFour);
                        }
                    }
                    if (Convert.ToBoolean(OrderFourPLCAdd) == true)
                    {
                        s7Net.Write(address + ".280.3", false);
                    }
                    //5号订单位
                    var OrderFivePLCAdd = AnalysisPLCData.GetData((decimal)1102.4, "Bool", handleData);
                    var OrderFivePLCState = AnalysisPLCData.GetData(1112, "Int", handleData);
                    var OrderNoFive = AnalysisPLCData.GetData(170, "String[32]", handleData);
                    if ((Convert.ToBoolean(OrderFivePLCAdd) == false && OrderFivePLCState != "2" && string.IsNullOrEmpty(OrderNoFive))
                        || (Convert.ToBoolean(OrderFivePLCAdd) == false && OrderFivePLCState == "2" && !string.IsNullOrEmpty(OrderNoFive))
                        )
                    {
                        needOrder.Add(5);
                        if (!string.IsNullOrEmpty(OrderNoFive))
                        {
                            //异步通知订单完成
                            completedOrder.Add(5,OrderNoFive);
                        }
                    }
                    if (Convert.ToBoolean(OrderFivePLCAdd) == true)
                    {
                        s7Net.Write(address + ".280.4", false);
                    }
                    //6号订单位
                    var OrderSixPLCAdd = AnalysisPLCData.GetData((decimal)1102.5, "Bool", handleData);
                    var OrderSixPLCState = AnalysisPLCData.GetData(1114, "Int", handleData);
                    var OrderNoSix = AnalysisPLCData.GetData(212, "String[32]", handleData);
                    if ((Convert.ToBoolean(OrderSixPLCAdd) == false && OrderSixPLCState != "2" && string.IsNullOrEmpty(OrderNoSix))
                        || (Convert.ToBoolean(OrderSixPLCAdd) == false && OrderSixPLCState == "2" && !string.IsNullOrEmpty(OrderNoSix))
                        )
                    {
                        needOrder.Add(6);
                        if (!string.IsNullOrEmpty(OrderNoSix))
                        {
                            //异步通知订单完成
                            completedOrder.Add(6,OrderNoSix);
                        }
                    }
                    if (Convert.ToBoolean(OrderFivePLCAdd) == true)
                    {
                        s7Net.Write(address + ".280.5", false);
                    }
                    #endregion
                    #region 向plc写订单
                    if (needOrder.Count > 0)
                    {
                        //像IMes要订单 拿到订单之后遍历写入数据
                        var listOrder = new List<PLCOrder>();
                        var request = new RestRequest("/PublishOrder/PushCollectionOrderData", Method.Get);
                        request.AddHeader("Content-Type", "application/json");
                        foreach (var item in needOrder)
                        {
                            request.AddParameter("places", item);
                        }
                        var response = await client.ExecuteGetAsync(request);
                        var result = response.Content;
                        if (result != "" && result != "[]")
                        {
                            listOrder = JsonConvert.DeserializeObject<List<PLCOrder>>(result);
                        }
                        if (listOrder.Count > 0)
                        {
                            foreach (var item in needOrder)
                            {
                                var orderDetail = listOrder.FirstOrDefault(p => p.OrderPlace == item);
                                if (item == 1)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".2.0", orderDetail.OrderNo);
                                        s7Net.Write(address + ".36.0", orderDetail.ONumber);
                                        s7Net.Write(address + ".38.0", orderDetail.OProduct);
                                        s7Net.Write(address + ".40.0", orderDetail.FProduct);
                                        s7Net.Write(address + ".42.0", orderDetail.FNumber);
                                        s7Net.Write(address + ".280.0", true);
                                    }
                                }
                                else if (item == 2)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".44.0", orderDetail.OrderNo);
                                        s7Net.Write(address + ".78.0", orderDetail.ONumber);
                                        s7Net.Write(address + ".80.0", orderDetail.OProduct);
                                        s7Net.Write(address + ".82.0", orderDetail.FProduct);
                                        s7Net.Write(address + ".84.0", orderDetail.FNumber);
                                        s7Net.Write(address + ".280.1", true);
                                    }
                                }
                                else if (item == 3)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".86.0", orderDetail.OrderNo);
                                        s7Net.Write(address + ".120.0", orderDetail.ONumber);
                                        s7Net.Write(address + ".122.0", orderDetail.OProduct);
                                        s7Net.Write(address + ".124.0", orderDetail.FProduct);
                                        s7Net.Write(address + ".126.0", orderDetail.FNumber);
                                        s7Net.Write(address + ".280.2", true);
                                    }
                                }
                                else if (item == 4)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".128.0", orderDetail.OrderNo);
                                        s7Net.Write(address + ".162.0", orderDetail.ONumber);
                                        s7Net.Write(address + ".164.0", orderDetail.OProduct);
                                        s7Net.Write(address + ".166.0", orderDetail.FProduct);
                                        s7Net.Write(address + ".168.0", orderDetail.FNumber);
                                        s7Net.Write(address + ".280.3", true);
                                    }
                                }
                                else if (item == 5)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".170.0", orderDetail.OrderNo);
                                        s7Net.Write(address + ".204.0", orderDetail.ONumber);
                                        s7Net.Write(address + ".206.0", orderDetail.OProduct);
                                        s7Net.Write(address + ".208.0", orderDetail.FProduct);
                                        s7Net.Write(address + ".210.0", orderDetail.FNumber);
                                        s7Net.Write(address + ".280.4", true);
                                    }
                                }
                                else if (item == 6)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".212.0", orderDetail.OrderNo);
                                        s7Net.Write(address + ".246.0", orderDetail.ONumber);
                                        s7Net.Write(address + ".248.0", orderDetail.OProduct);
                                        s7Net.Write(address + ".250.0", orderDetail.FProduct);
                                        s7Net.Write(address + ".252.0", orderDetail.FNumber);
                                        s7Net.Write(address + ".280.5", true);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region 完成订单
                    //可以根据落料数量来完成订单 这个先留着
                    if (completedOrder.Count > 0)
                    {
                        //像IMes要订单 拿到订单之后遍历写入数据
                        var orderCompleteds = new List<OrderCompleted>();
                        var request = new RestRequest("/PublishOrder/CompetedOrders", Method.Get);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddBody(completedOrder) ;
                        var response = await client.ExecutePostAsync(request);
                        var result = response.Content;
                        if (result != "" && result != "[]")
                        {
                            orderCompleteds = JsonConvert.DeserializeObject<List<OrderCompleted>>(result);
                        }
                        if (orderCompleteds.Count > 0)
                        {
                            foreach (var item in needOrder)
                            {
                                var orderDetail = orderCompleteds.FirstOrDefault(p => p.places == item);
                                if (item == 1)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".2.0", "");
                                        s7Net.Write(address + ".36.0",(short)0);
                                        s7Net.Write(address + ".38.0", (short)0);
                                        s7Net.Write(address + ".40.0", (short)0);
                                        s7Net.Write(address + ".42.0", (short)0);
                                    }
                                }
                                else if (item == 2)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".44.0", "");
                                        s7Net.Write(address + ".78.0", (short)0);
                                        s7Net.Write(address + ".80.0", (short)0);
                                        s7Net.Write(address + ".82.0", (short)0);
                                        s7Net.Write(address + ".84.0", (short)0);
                                    }
                                }
                                else if (item == 3)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".86.0", "");
                                        s7Net.Write(address + ".120.0", (short)0);
                                        s7Net.Write(address + ".122.0", (short)0);
                                        s7Net.Write(address + ".124.0", (short)0);
                                        s7Net.Write(address + ".126.0", (short)0);
                                    }
                                }
                                else if (item == 4)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".128.0", "");
                                        s7Net.Write(address + ".162.0", (short)0);
                                        s7Net.Write(address + ".164.0", (short)0);
                                        s7Net.Write(address + ".166.0", (short)0);
                                        s7Net.Write(address + ".168.0", (short)0);
                                    }
                                }
                                else if (item == 5)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".170.0", "");
                                        s7Net.Write(address + ".204.0", (short)0);
                                        s7Net.Write(address + ".206.0", (short)0);
                                        s7Net.Write(address + ".208.0", (short)0);
                                        s7Net.Write(address + ".210.0", (short)0);
                                    }
                                }
                                else if (item == 6)
                                {
                                    if (orderDetail != null)
                                    {
                                        s7Net.Write(address + ".212.0", "");
                                        s7Net.Write(address + ".246.0", (short)0);
                                        s7Net.Write(address + ".248.0", (short)0);
                                        s7Net.Write(address + ".250.0", (short)0);
                                        s7Net.Write(address + ".252.0", (short)0);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #endregion
                    await Task.Delay(10);
                });

                //处理出库
                Task task2 = Task.Factory.StartNew(async () =>
                {
                    #region 叫料
                    var needCall = new Dictionary<int, short>();
                    #region 叫料处理
                    //1号叫料位
                    var BankOnePlcSend = AnalysisPLCData.GetData((decimal)366.4, "Bool", handleData);
                    var BankOnePlcProduct = AnalysisPLCData.GetData((decimal)368, "Int", handleData);
                    var BankExcuteStateOne = AnalysisPLCData.GetData((decimal)254, "Int", handleData);
                    if (Convert.ToBoolean(BankOnePlcSend) == true && (BankExcuteStateOne == "3" || BankExcuteStateOne == "0"))
                    {
                        needCall.Add(1, Convert.ToInt16(BankOnePlcProduct));
                        //清空数据
                        if (BankExcuteStateOne == "3")
                        {
                            s7Net.Write(address + ".254.0", (short)0);
                            s7Net.Write(address + ".256.0", (short)0);
                            s7Net.Write(address + ".258.0", (short)0);
                            s7Net.Write(address + ".306.0", (short)0);
                        }
                    }
                    var BankSendOne = AnalysisPLCData.GetData((decimal)366.1, "Bool", handleData);
                    if (Convert.ToBoolean(BankSendOne) == true)
                    {
                        s7Net.Write(address + ".280.6", false);
                    }
                    //2号叫料位
                    var BankTwoPlcSend = AnalysisPLCData.GetData((decimal)370, "Bool", handleData);
                    var BankTwoPlcProduct = AnalysisPLCData.GetData((decimal)372, "Int", handleData);
                    var BankExcuteStateTwo = AnalysisPLCData.GetData((decimal)260, "Int", handleData);
                    if (Convert.ToBoolean(BankTwoPlcSend) == true && (BankExcuteStateTwo == "3" || BankExcuteStateTwo == "0"))
                    {
                        needCall.Add(2, Convert.ToInt16(BankTwoPlcProduct));
                        //清空数据
                        if (BankExcuteStateTwo == "3")
                        {
                            s7Net.Write(address + ".260.0", (short)0);
                            s7Net.Write(address + ".262.0", (short)0);
                            s7Net.Write(address + ".264.0", (short)0);
                            s7Net.Write(address + ".308.0", (short)0);
                        }
                    }
                    var BankSendTwo = AnalysisPLCData.GetData((decimal)366.2, "Bool", handleData);
                    if (Convert.ToBoolean(BankSendTwo) == true)
                    {
                        s7Net.Write(address + ".280.7", false);
                    }
                    //3号叫料位
                    var BankThreePlcSend = AnalysisPLCData.GetData((decimal)374, "Bool", handleData);
                    var BankThreePlcProduct = AnalysisPLCData.GetData((decimal)376, "Int", handleData);
                    var BankExcuteStateThree = AnalysisPLCData.GetData((decimal)266, "Int", handleData);
                    if (Convert.ToBoolean(BankTwoPlcSend) == true && (BankExcuteStateThree == "3" || BankExcuteStateThree == "0"))
                    {
                        needCall.Add(3, Convert.ToInt16(BankThreePlcProduct));
                        //清空数据
                        if (BankExcuteStateThree == "3")
                        {
                            s7Net.Write(address + ".260.0", (short)0);
                            s7Net.Write(address + ".262.0", (short)0);
                            s7Net.Write(address + ".264.0", (short)0);
                            s7Net.Write(address + ".310.0", (short)0);
                        }
                    }
                    var BankSendThree = AnalysisPLCData.GetData((decimal)366.3, "Bool", handleData);
                    if (Convert.ToBoolean(BankSendTwo) == true)
                    {
                        s7Net.Write(address + ".281.0", false);
                    }
                    //像立库叫料
                    if (needCall.Count > 0)
                    {
                        var bankOrders = new List<PLCBankCall>();
                        var request = new RestRequest("/TaskCallBack/BankMaterialOut", Method.Post);
                        request.AddHeader("Content-Type", "application/json");
                        //var jsonData = JsonConvert.SerializeObject(needCall);
                        request.AddBody(needCall);
                        var response = await client.ExecutePostAsync(request);
                        var result = response.Content;
                        if (result != "" && result != "[]" && result != null && !result.Contains("101"))
                        {
                            bankOrders = JsonConvert.DeserializeObject<List<PLCBankCall>>(result);
                        }

                        if (needCall.ContainsKey(1))
                        {
                            var bankOrder = bankOrders.FirstOrDefault(p => p.orderPlace == 1);
                            if (bankOrder != null)
                            {
                                s7Net.Write(address + ".254.0", (short)1);
                                s7Net.Write(address + ".256.0", bankOrder.orderNo);
                                s7Net.Write(address + ".258.0", bankOrder.pipCount);
                                s7Net.Write(address + ".280.6", true);
                                s7Net.Write(address + ".306.0", needCall.First(p => p.Key == 1).Value);
                            }
                        }
                        if (needCall.ContainsKey(2))
                        {
                            var bankOrder = bankOrders.FirstOrDefault(p => p.orderPlace == 2);
                            if (bankOrder != null)
                            {
                                s7Net.Write(address + ".260.0", (short)1);
                                s7Net.Write(address + ".262.0", bankOrder.orderNo);
                                s7Net.Write(address + ".264.0", bankOrder.pipCount);
                                s7Net.Write(address + ".280.7", true);
                                s7Net.Write(address + ".308.0", needCall.First(p => p.Key == 2).Value);
                            }
                        }
                        if (needCall.ContainsKey(3))
                        {
                            var bankOrder = bankOrders.FirstOrDefault(p => p.orderPlace == 3);
                            if (bankOrder != null)
                            {
                                s7Net.Write(address + ".266.0", (short)1);
                                s7Net.Write(address + ".268.0", bankOrder.orderNo);
                                s7Net.Write(address + ".270.0", bankOrder.pipCount);
                                s7Net.Write(address + ".281.0", true);
                                s7Net.Write(address + ".310.0", needCall.First(p => p.Key == 3).Value);
                            }
                        }
                    }

                    var handleBankOrder = new Dictionary<int, short>();
                    //更新状态
                    var BankOrderOne = AnalysisPLCData.GetData((decimal)256, "Int", handleData);
                    var BankOrderState1 = AnalysisPLCData.GetData((decimal)254, "Int", handleData);
                    if (!string.IsNullOrEmpty(BankOrderOne) && BankOrderState1 != "0" && BankOrderState1 != "3")
                    {
                        handleBankOrder.Add(1, Convert.ToInt16(BankOrderOne));
                    }
                    //更新状态
                    var BankOrderTwo = AnalysisPLCData.GetData((decimal)262, "Int", handleData);
                    var BankOrderState2 = AnalysisPLCData.GetData((decimal)260, "Int", handleData);
                    if (!string.IsNullOrEmpty(BankOrderTwo) && BankOrderState2 != "0" && BankOrderState2 != "3")
                    {
                        handleBankOrder.Add(2, Convert.ToInt16(BankOrderTwo));
                    }

                    //更新订单状态
                    var BankOrderThree = AnalysisPLCData.GetData((decimal)268, "Int", handleData);
                    var BankOrderState3 = AnalysisPLCData.GetData((decimal)266, "Int", handleData);
                    if (!string.IsNullOrEmpty(BankOrderThree) && BankOrderState3 != "0" && BankOrderState3 != "3")
                    {
                        handleBankOrder.Add(3, Convert.ToInt16(BankOrderThree));
                    }
                    if (handleBankOrder.Count > 0)
                    {
                        //向imes请求订单状态数据
                        var bankStates = new List<BankOrderState>();
                        var request = new RestRequest("/TaskCallBack/BankMaterialOutState", Method.Post);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddBody(handleBankOrder);
                        var response = await client.ExecutePostAsync(request);
                        var result = response.Content;
                        if (result != "" && result != "[]" && result != null && !result.Contains("101"))
                        {
                            bankStates = JsonConvert.DeserializeObject<List<BankOrderState>>(result);
                        }
                        if (handleBankOrder.ContainsKey(1))
                        {
                            var bankOrder = bankStates.FirstOrDefault(p => p.orderPlace == 1);
                            if (bankOrder.orderState > 1 && bankOrder.orderState.ToString() != BankExcuteStateOne)
                            {
                                s7Net.Write(address + ".254.0", (short)bankOrder.orderState);
                            }
                        }
                        if (handleBankOrder.ContainsKey(2))
                        {
                            var bankOrder = bankStates.FirstOrDefault(p => p.orderPlace == 2);
                            if (bankOrder.orderState > 1 && bankOrder.orderState.ToString() != BankExcuteStateTwo)
                            {
                                s7Net.Write(address + ".260.0", (short)bankOrder.orderState);
                            }
                        }
                        if (handleBankOrder.ContainsKey(3))
                        {
                            var bankOrder = bankStates.FirstOrDefault(p => p.orderPlace == 3);
                            if (bankOrder.orderState > 1 && bankOrder.orderState.ToString() != BankExcuteStateThree)
                            {
                                s7Net.Write(address + ".266.0", (short)bankOrder.orderState);
                            }
                        }
                    }

                    #endregion
                    #endregion
                    await Task.Delay(10);
                });


                //处理下料线料满空转换
                Task task3 = Task.Factory.StartNew(async () =>
                {
                    var needTrans = new List<AgvOrder>();
                    //先1号缓存位判断是否下料
                    var placeOut1 = AnalysisPLCData.GetData(680, "Bool", handleData);
                    var placeState1 = AnalysisPLCData.GetData(282, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut1)&&placeState1=="1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder1 = AnalysisPLCData.GetData(646, "String[32]", handleData);
                        var placeCount1 = Convert.ToInt16(AnalysisPLCData.GetData(682, "Int", handleData));
                        needTran.dataCount = placeCount1;
                        needTran.orderNo = placeOrder1;
                        needTran.places = 1;
                        needTrans.Add(needTran);
                    }


                    //先2号缓存位判断是否下料
                    var placeOut2 = AnalysisPLCData.GetData(718, "Bool", handleData);
                    var placeState2 = AnalysisPLCData.GetData(284, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut2) && placeState2 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder2 = AnalysisPLCData.GetData(684, "String[32]", handleData);
                        var placeCount2 = Convert.ToInt16(AnalysisPLCData.GetData(720, "Int", handleData));
                        needTran.dataCount = placeCount2;
                        needTran.orderNo = placeOrder2;
                        needTran.places = 2;
                        needTrans.Add(needTran);
                    }


                    //先3号缓存位判断是否下料
                    var placeOut3 = AnalysisPLCData.GetData(756, "Bool", handleData);
                    var placeState3 = AnalysisPLCData.GetData(286, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut3) && placeState3 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder3 = AnalysisPLCData.GetData(722, "String[32]", handleData);
                        var placeCount3 = Convert.ToInt16(AnalysisPLCData.GetData(758, "Int", handleData));
                        needTran.dataCount = placeCount3;
                        needTran.orderNo = placeOrder3;
                        needTran.places = 3;
                        needTrans.Add(needTran);
                    }


                    //先4号缓存位判断是否下料
                    var placeOut4 = AnalysisPLCData.GetData(794, "Bool", handleData);
                    var placeState4 = AnalysisPLCData.GetData(288, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut4) && placeState4 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder4 = AnalysisPLCData.GetData(760, "String[32]", handleData);
                        var placeCount4 = Convert.ToInt16(AnalysisPLCData.GetData(796, "Int", handleData));
                        needTran.dataCount = placeCount4;
                        needTran.orderNo = placeOrder4;
                        needTran.places = 4;
                        needTrans.Add(needTran);
                    }


                    //先5号缓存位判断是否下料
                    var placeOut5 = AnalysisPLCData.GetData(832, "Bool", handleData);
                    var placeState5 = AnalysisPLCData.GetData(290, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut5) && placeState5 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder5 = AnalysisPLCData.GetData(798, "String[32]", handleData);
                        var placeCount5 = Convert.ToInt16(AnalysisPLCData.GetData(834, "Int", handleData));
                        needTran.dataCount = placeCount5;
                        needTran.orderNo = placeOrder5;
                        needTran.places = 5;
                        needTrans.Add(needTran);
                    }


                    //先6号缓存位判断是否下料
                    var placeOut6 = AnalysisPLCData.GetData(870, "Bool", handleData);
                    var placeState6 = AnalysisPLCData.GetData(292, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut6) && placeState6 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder6 = AnalysisPLCData.GetData(836, "String[32]", handleData);
                        var placeCount6 = Convert.ToInt16(AnalysisPLCData.GetData(872, "Int", handleData));
                        needTran.dataCount = placeCount6;
                        needTran.orderNo = placeOrder6;
                        needTran.places = 6;
                        needTrans.Add(needTran);
                    }


                    //先7号缓存位判断是否下料
                    var placeOut7 = AnalysisPLCData.GetData(908, "Bool", handleData);
                    var placeState7 = AnalysisPLCData.GetData(294, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut7) && placeState7 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder7 = AnalysisPLCData.GetData(874, "String[32]", handleData);
                        var placeCount7 = Convert.ToInt16(AnalysisPLCData.GetData(910, "Int", handleData));
                        needTran.dataCount = placeCount7;
                        needTran.orderNo = placeOrder7;
                        needTran.places = 7;
                    }


                    //先8号缓存位判断是否下料
                    var placeOut8 = AnalysisPLCData.GetData(946, "Bool", handleData);
                    var placeState8 = AnalysisPLCData.GetData(296, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut8) && placeState8 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder8 = AnalysisPLCData.GetData(912, "String[32]", handleData);
                        var placeCount8 = Convert.ToInt16(AnalysisPLCData.GetData(948, "Int", handleData));
                        needTran.dataCount = placeCount8;
                        needTran.orderNo = placeOrder8;
                        needTran.places = 8;
                        needTrans.Add(needTran);
                    }


                    //先9号缓存位判断是否下料
                    var placeOut9 = AnalysisPLCData.GetData(984, "Bool", handleData);
                    var placeState9 = AnalysisPLCData.GetData(298, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut9) && placeState9 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder9 = AnalysisPLCData.GetData(950, "String[32]", handleData);
                        var placeCount9 = Convert.ToInt16(AnalysisPLCData.GetData(986, "Int", handleData));
                        needTran.dataCount = placeCount9;
                        needTran.orderNo = placeOrder9;
                        needTran.places = 9;
                        needTrans.Add(needTran);
                    }


                    //先10号缓存位判断是否下料
                    var placeOut10 = AnalysisPLCData.GetData(1022, "Bool", handleData);
                    var placeState10 = AnalysisPLCData.GetData(300, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut10) && placeState10 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder10 = AnalysisPLCData.GetData(988, "String[32]", handleData);
                        var placeCount10 = Convert.ToInt16(AnalysisPLCData.GetData(1024, "Int", handleData));
                        needTran.dataCount = placeCount10;
                        needTran.orderNo = placeOrder10;
                        needTran.places = 10;
                        needTrans.Add(needTran);
                    }


                    //先11号缓存位判断是否下料
                    var placeOut11 = AnalysisPLCData.GetData(1060, "Bool", handleData);
                    var placeState11 = AnalysisPLCData.GetData(302, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut11) && placeState11 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder11 = AnalysisPLCData.GetData(1026, "String[32]", handleData);
                        var placeCount11 = Convert.ToInt16(AnalysisPLCData.GetData(1062, "Int", handleData));
                        needTran.dataCount = placeCount11;
                        needTran.orderNo = placeOrder11;
                        needTran.places = 11;
                        needTrans.Add(needTran);
                    }


                    //先12号缓存位判断是否下料
                    var placeOut12 = AnalysisPLCData.GetData(1098, "Bool", handleData);
                    var placeState12 = AnalysisPLCData.GetData(302, "Int", handleData);
                    //如果中联需要订单下工单分开报工 那也只能按框的数量报工
                    if (Convert.ToBoolean(placeOut12) && placeState12 == "1")
                    {
                        var needTran = new AgvOrder();
                        //获取切割机1订单//获取料框1矩管数量
                        var placeOrder12 = AnalysisPLCData.GetData(1064, "String[32]", handleData);
                        var placeCount12 = Convert.ToInt16(AnalysisPLCData.GetData(1100, "Int", handleData));
                        needTran.dataCount = placeCount12;
                        needTran.orderNo = placeOrder12;
                        needTran.places = 12;
                        needTrans.Add(needTran);
                    }

                    if (needTrans.Count > 0)
                    {
                        //请求AGV空满转换 调用imes接口生成AGV调度任务 但是AGV任务的执行放在wcs来写 
                        //向imes请求订单状态数据3
                        var agvStates = new List<AgvOrderCallBack>();
                        var request = new RestRequest("/TaskCallBack/LineAddAgvTask", Method.Post);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddBody(needTrans);
                        var response = await client.ExecutePostAsync(request);
                        var result = response.Content;
                        if (result != "" && result != "[]" && result != null && !result.Contains("101"))
                        {
                            agvStates = JsonConvert.DeserializeObject<List<AgvOrderCallBack>>(result);
                        }
                        if (agvStates.Count > 0)
                        {
                            for (int i = 1; i < 13; i++)
                            {
                                if (needTrans.Count(p => p.places == i) > 0
                                && agvStates.Count(p => p.places == i) > 0)
                                {
                                    var plcHex = "DB2."+282 + (i - 1) * 2+".0";
                                    s7Net.Write(plcHex, 2);
                                }
                            }
                        }

                    }
                    await Task.Delay(10);
                });


                await Task.WhenAll(task1, task2, task3);
            }


            Console.WriteLine("下料线任务处理结束" + DateTime.Now);
            checkData.Add("下料线任务处理结束" + DateTime.Now);
            _caheManager.Remove("slingcingtask_001_check");
            _caheManager.Set("slingcingtask_001_check", checkData, 1);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
