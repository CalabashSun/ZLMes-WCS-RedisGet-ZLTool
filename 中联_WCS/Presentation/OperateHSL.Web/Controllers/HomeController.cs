using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OperateHSL.Core.Caching;
using OperateHSL.Data.DataModel.PLCModel;
using OperateHSL.Data.PLC;
using OperateHSL.Services.Log4netService;
using OperateHSL.Services.MesOrderService;
using OperateHSL.Services.PlcInfoService;
using OperateHSL.Web.Models;

namespace OperateHSL.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ICacheManager _caheManager;
        private readonly IPlcDataService _plcDataSevice;
        private readonly IOrderOperateService _orderOperateService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheManager"></param>
        /// <param name="plcDataService"></param>
        public HomeController(ICacheManager cacheManager
            ,IPlcDataService plcDataService
            ,IOrderOperateService orderOperateService)
        {
            _caheManager = cacheManager;
            _plcDataSevice = plcDataService;
            _orderOperateService = orderOperateService;
        }

        public IActionResult Index()
        {
            //_orderOperateService.ReportAutoTicket("12WC1202412031527074");
            //_plcDataSevice.GetLastReadData("10.99.109.212");
            //var fidLineReadDatas = _plcDataSevice.GetTransportData();
            //var ipAddressRead = fidLineReadDatas.GroupBy(p => p.OrginalIpAddress).Select(m => m.Key).ToList();
            //ParallelLoopResult rfidRead = Parallel.ForEach(ipAddressRead, itemIpRead =>
            //{
            //    var thisIpDataRead = fidLineReadDatas.Where(p => p.OrginalIpAddress == itemIpRead).ToList();
            //    foreach (var itemReadDetails in thisIpDataRead)
            //    {
            //        short currentCount = (short)itemReadDetails.SumCount;
            //        if (itemIpRead == "10.99.109.43")
            //        {
            //            currentCount = 0;
            //        }
            //        if (currentCount != itemReadDetails.SumCount)
            //        {
            //            _plcDataSevice.UpdateTransportRecord(itemReadDetails.Id,currentCount);
            //        }
            //    }
            //});
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Guangshan()
        {
            return View();
        }


        public IActionResult GetCheckData()
        {
            var slicing_001 = _caheManager.Get<List<string>>("slingcingresult_001_check");
            var slicing_002 = _caheManager.Get<List<string>>("slingcingtask_001_check");
            var fid_001 = _caheManager.Get<List<string>>("fid_001_check");

            var strussRead = _caheManager.Get<List<string>>("strucssread_001_check");
            var strussExcute = _caheManager.Get<List<string>>("strucssexucete_001_check");

             var upCatchTaskExcute = _caheManager.Get<List<string>>("UpCatchTaskEnd"); 
            var weldingRead = _caheManager.Get<List<string>>("weldingRepairPerform_001_check");
            var fidReadData = new List<string>();

            #region RFID描述替换
            if (fid_001 != null)
            {
                foreach (var item in fid_001)
                {
                    var newItem = item;
                    #region 人工组队 + 换向线
                    if (newItem.Contains("10.99.109.193"))
                    {
                        newItem = item.Replace("10.99.109.193", "10.99.109.193人工组队倍速链PLC");
                    }
                    if (newItem.Contains("10.99.109.210"))
                    {
                        newItem = item.Replace("10.99.109.210", "10.99.109.210组队南侧倍速链RFID设备");
                    }
                    if (newItem.Contains("10.99.109.211"))
                    {
                        newItem = item.Replace("10.99.109.211", "10.99.109.211组队北侧倍速链RFID设备");
                    }

                    if (newItem.Contains("10.99.109.187"))
                    {
                        newItem = item.Replace("10.99.109.187", "10.99.109.187换向线倍速链PLC");
                    }
                    if (newItem.Contains("10.99.109.212"))
                    {
                        newItem = item.Replace("10.99.109.212", "10.99.109.212换向线北侧RFID设备");
                    }
                    if (newItem.Contains("10.99.109.213"))
                    {
                        newItem = item.Replace("10.99.109.213", "10.99.109.213换向线南侧RFID设备");
                    }
                    #endregion

                    #region 自动焊北侧
                    if (newItem.Contains("10.99.109.79"))
                    {
                        newItem = item.Replace("10.99.109.79", "10.99.109.79自动焊北侧第一个倍速链PLC");
                    }
                    if (newItem.Contains("10.99.109.218"))
                    {
                        newItem = item.Replace("10.99.109.218", "10.99.109.218自动焊北侧第一个RFID设备");
                    }
                    if (newItem.Contains("10.99.109.61"))
                    {
                        newItem = item.Replace("10.99.109.61", "10.99.109.61自动焊北侧第二个倍速链PLC");
                    }

                    if (newItem.Contains("10.99.109.217"))
                    {
                        newItem = item.Replace("10.99.109.217", "10.99.109.217自动焊北侧第二个RFID设备");
                    }
                    if (newItem.Contains("10.99.109.43"))
                    {
                        newItem = item.Replace("10.99.109.43", "10.99.109.43自动焊北侧第三个倍速链PLC");
                    }
                    if (newItem.Contains("10.99.109.216"))
                    {
                        newItem = item.Replace("10.99.109.216", "10.99.109.216自动焊北侧第三个RFID设备");
                    }
                    if (newItem.Contains("10.99.109.25"))
                    {
                        newItem = item.Replace("10.99.109.25", "10.99.109.25自动焊北侧第四个倍速链PLC");
                    }

                    if (newItem.Contains("10.99.109.215"))
                    {
                        newItem = item.Replace("10.99.109.215", "10.99.109.215自动焊北侧第四个RFID设备");
                    }
                    if (newItem.Contains("10.99.109.7"))
                    {
                        newItem = item.Replace("10.99.109.7", "10.99.109.7自动焊北侧第五个倍速链PLC");
                    }
                    if (newItem.Contains("10.99.109.214"))
                    {
                        newItem = item.Replace("10.99.109.214", "10.99.109.214自动焊北侧第五个RFID设备");
                    }
                    #endregion


                    #region 自动焊南侧
                    if (newItem.Contains("10.99.109.163"))
                    {
                        newItem = item.Replace("10.99.109.163", "10.99.109.163自动焊南侧第一个倍速链PLC");
                    }
                    if (newItem.Contains("10.99.109.223"))
                    {
                        newItem = item.Replace("10.99.109.223", "10.99.109.223自动焊南侧第一个RFID设备");
                    }
                    if (newItem.Contains("10.99.109.145"))
                    {
                        newItem = item.Replace("10.99.109.145", "10.99.109.145自动焊南侧第二个倍速链PLC");
                    }

                    if (newItem.Contains("10.99.109.222"))
                    {
                        newItem = item.Replace("10.99.109.222", "10.99.109.222自动焊南侧第二个RFID设备");
                    }
                    if (newItem.Contains("10.99.109.127"))
                    {
                        newItem = item.Replace("10.99.109.127", "10.99.109.127自动焊南侧第三个倍速链PLC");
                    }
                    if (newItem.Contains("10.99.109.221"))
                    {
                        newItem = item.Replace("10.99.109.221", "10.99.109.221自动焊南侧第三个RFID设备");
                    }
                    if (newItem.Contains("10.99.109.109"))
                    {
                        newItem = item.Replace("10.99.109.109", "10.99.109.109自动焊南侧第四个倍速链PLC");
                    }

                    if (newItem.Contains("10.99.109.220"))
                    {
                        newItem = item.Replace("10.99.109.220", "10.99.109.220自动焊南侧第四个RFID设备");
                    }
                    if (newItem.Contains("10.99.109.91"))
                    {
                        newItem = item.Replace("10.99.109.91", "10.99.109.91自动焊南侧第五个倍速链PLC");
                    }
                    if (newItem.Contains("10.99.109.219"))
                    {
                        newItem = item.Replace("10.99.109.219", "10.99.109.219自动焊南侧第五个RFID设备");
                    }
                    #endregion

                    #region 下料线
                    if (newItem.Contains("10.99.109.240"))
                    {
                        newItem = item.Replace("10.99.109.240", "10.99.109.240下料线北侧倍速链PLC");
                    }
                    if (newItem.Contains("10.99.109.236"))
                    {
                        newItem = item.Replace("10.99.109.236", "10.99.109.236下料线北侧倍速链第二个RFID设备");
                    }
                    if (newItem.Contains("10.99.109.237"))
                    {
                        newItem = item.Replace("10.99.109.237", "10.99.109.237下料线北侧倍速链第一个RFID设备");
                    }

                    if (newItem.Contains("10.99.109.247"))
                    {
                        newItem = item.Replace("10.99.109.247", "10.99.109.247下料线南侧倍速链PLC");
                    }
                    if (newItem.Contains("10.99.109.238"))
                    {
                        newItem = item.Replace("10.99.109.238", "10.99.109.238下料线南侧倍速链第二个RFID设备");
                    }
                    if (newItem.Contains("10.99.109.239"))
                    {
                        newItem = item.Replace("10.99.109.239", "10.99.109.239下料线南侧倍速链第一个RFID设备");
                    }
                    #endregion
                    fidReadData.Add(newItem);
                }
            }
            
            #endregion

            var rfidReadRecord = _plcDataSevice.GetRfidReadData();
            var plcFidInfo = _plcDataSevice.GetPLCFIDIP();
            var fidGetData = new List<string>();

            foreach (var itemGet in plcFidInfo)
            {
                var getResult = rfidReadRecord.FirstOrDefault(p => p.IpAddress == itemGet.FidIpAddress);
                var dataDesc = "无";
                if (getResult != null)
                {
                    dataDesc = getResult.ReadResult;
                }
                var resultRead = itemGet.FidIpAddress + "," + itemGet.FidAddressDesc + "读取到的数据为：" + dataDesc;
                fidGetData.Add(resultRead);
            }

            var jsonResult = new JObject();

            jsonResult["s001"] = JsonConvert.SerializeObject(slicing_001);
            jsonResult["s002"] = JsonConvert.SerializeObject(slicing_002);
            jsonResult["s003"] = JsonConvert.SerializeObject(fidReadData);
            jsonResult["s006"] = JsonConvert.SerializeObject(fidGetData);
            jsonResult["s004"] = JsonConvert.SerializeObject(strussRead);
            jsonResult["s005"] = JsonConvert.SerializeObject(strussExcute);
            jsonResult["s006"] = JsonConvert.SerializeObject(weldingRead);
            jsonResult["s007"] = JsonConvert.SerializeObject(upCatchTaskExcute);

            return Json(jsonResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
