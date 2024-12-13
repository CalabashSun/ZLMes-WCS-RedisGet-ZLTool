using HslCommunication;
using HslCommunication.Profinet.Siemens;
using Operate.Services.PlcConnectService;
using OperateHSL.Core.Caching;
using OperateHSL.Core.Helpers;
using OperateHSL.Core.Tool;
using OperateHSL.Data.PLC;
using OperateHSL.Services.FIDInfoService;
using OperateHSL.Services.Log4netService;
using OperateHSL.Services.MesOrderService;
using OperateHSL.Services.PlcInfoService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperateHSL.Services.Scheduling.Tasks
{

    public class PLCFIDReadTask : IScheduledTask
    {
        private readonly IFIDService _fidService;
        private readonly IPlcDataService _plcDataSevice;
        private readonly IOrderOperateService _orderOperateService;
        private readonly ICacheManager _caheManager;
        public PLCFIDReadTask(IFIDService fidService
            , IPlcDataService plcDataService
            , ICacheManager cacheManager
            , IOrderOperateService orderOperateService)
        {
            _fidService = fidService;
            _plcDataSevice = plcDataService;
            _caheManager = cacheManager;
            _orderOperateService = orderOperateService;
        }
        public int Schedule => 5200;
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            //fid读取 code是004
            var checkData = new List<string>();
            var dataInfos = _plcDataSevice.GetCollectionPlcDbList("004");
            var fidIps = _plcDataSevice.GetPLCFIDIP();
            checkData.Add("fid设备读取开始" + DateTime.Now+" </ br>");


            #region 单例模式测试
            //Parallel.ForEach(dataInfos, dataItem =>
            //{
            //    var s7Net = PlcBufferRack.Instance(dataItem.plc_ip);
            //    var dataLength = Convert.ToUInt16(dataItem.db_length);
            //    OperateResult<byte[]> resultS7net = s7Net.Read(dataItem.plc_dbadr + ".0", dataLength);
            //    if (resultS7net.IsSuccess)
            //    {
            //        var result = s7Net.ByteTransform.TransBool(resultS7net.Content, 0);
            //    }
            //});
            #endregion

            #region 逻辑代码
            ParallelLoopResult resultRead = Parallel.ForEach(dataInfos, dataItem =>
            {
                //操控读取plc数据
                var s7Net = PlcBufferRack.Instance(dataItem.plc_ip);
                if (s7Net != null)
                {
                    var dataLength = Convert.ToUInt16(dataItem.db_length);
                    OperateResult<byte[]> resultS7net = s7Net.Read(dataItem.plc_dbadr + ".0", dataLength);
                    var dataInfo = dataItem.plc_dbInfos;
                    if (resultS7net.IsSuccess)
                    {
                        var recvs = resultS7net.Content;
                        var saveData1 = new PLCFIDOne();
                        var saveData2 = new PLCFIDTwo();
                        PropertyInfo property = null;
                        if (dataItem.db_length == "38")
                        {
                            foreach (var itemDeviceData1 in dataInfo)
                            {
                                property = saveData1.GetType().GetProperty(itemDeviceData1.data_model);
                                Type propertyType = property.PropertyType;
                                var dataResult = AnalysisPLCData.GetData(itemDeviceData1.data_offset, itemDeviceData1.data_type, recvs);
                                property.SetValue(saveData1, Convert.ChangeType(dataResult, propertyType), null);
                            }
                        }
                        if (dataItem.db_length == "76")
                        {
                            foreach (var itemDeviceData2 in dataInfo)
                            {
                                property = saveData2.GetType().GetProperty(itemDeviceData2.data_model);
                                Type propertyType = property.PropertyType;
                                var dataResult = AnalysisPLCData.GetData(itemDeviceData2.data_offset, itemDeviceData2.data_type, recvs);
                                property.SetValue(saveData2, Convert.ChangeType(dataResult, propertyType), null);
                            }
                        }
                        if (dataItem.db_length == "38")
                        {
                            //判断是否有扫码需求
                            var needSeek = saveData1.isReadFidOne;
                            if (needSeek)
                            {
                                checkData.Add(dataItem.plc_ip + " 请求读取了fid数据" + DateTime.Now + "  </ br>");
                                //寻找对应的ip
                                var ipAddress = fidIps.First(p => p.PLCIpAddress == dataItem.plc_ip);
                                if (ipAddress != null)
                                {
                                    var dataIp = ipAddress.FidIpAddress;
                                    var result = _plcDataSevice.GetLastReadData(dataIp);
                                    //var result = "88888888888888888";
                                    //写PLC数据
                                    var address1 = dataItem.plc_dbadr + "." + "2.0";
                                    var address2 = dataItem.plc_dbadr + "." + "4.0";
                                    if (result.Contains("RFID") || result == "")
                                    {
                                    }
                                    else
                                    {
                                        int writeDataTimes = 1;
                                        var writeData = s7Net.Write(address2, result);
                                        while (!writeData.IsSuccess && writeDataTimes <= 3)
                                        {
                                            writeData = s7Net.Write(address2, result);
                                            writeDataTimes++;
                                        }
                                        if (writeData.IsSuccess)
                                        {
                                            int writeResultTimes = 1;
                                            short successResult = 1;
                                            var writeResult = s7Net.Write(address1, successResult);
                                            while (!writeData.IsSuccess && writeResultTimes <= 3)
                                            {
                                                writeResult = s7Net.Write(address1, successResult);
                                                writeResultTimes++;
                                            }
                                            if (!writeResult.IsSuccess)
                                            {
                                                LogHelper.Info(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为" + result + "但是写入《结果》失败了" + DateTime.Now);
                                            }
                                        }
                                        if (!writeData.IsSuccess)
                                        {
                                            LogHelper.Info(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为" + result + "但是写入《数据》失败了" + DateTime.Now);
                                        }

                                    }
                                    //LogHelper.Info(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为" + result);
                                    checkData.Add(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为：" + result + DateTime.Now + "  </ br>");
                                }

                            }
                        }
                        if (dataItem.db_length == "76")
                        {
                            //判断是否有扫码需求
                            var needSeek1 = saveData2.isReadFidOne;
                            if (needSeek1)
                            {
                                checkData.Add(dataItem.plc_ip + " 请求读取了fid数据" + DateTime.Now + "  </ br>");
                                //寻找对应的ip
                                var ipAddress = fidIps.First(p => p.PLCIpAddress == dataItem.plc_ip && p.SortId == 1);
                                if (ipAddress != null)
                                {
                                    var dataIp = ipAddress.FidIpAddress;
                                    var result = _plcDataSevice.GetLastReadData(dataIp);
                                    //写PLC数据
                                    var address1 = dataItem.plc_dbadr + "." + "2.0";
                                    var address2 = dataItem.plc_dbadr + "." + "4.0";
                                    if (result.Contains("RFID") || result == "")
                                    {
                                    }
                                    else
                                    {
                                        //自动焊报工
                                        if (dataItem.plc_ip == "10.99.109.240" || dataItem.plc_ip == "10.99.109.247")
                                        {
                                            _=_orderOperateService.ReportAutoTicket(result);
                                        }

                                        int writeDataTimes = 1;
                                        var writeData = s7Net.Write(address2, result);
                                        while (!writeData.IsSuccess && writeDataTimes <= 3)
                                        {
                                            writeData = s7Net.Write(address2, result);
                                            writeDataTimes++;
                                        }
                                        if (writeData.IsSuccess)
                                        {
                                            int writeResultTimes = 1;
                                            short successResult = 1;
                                            var writeResult = s7Net.Write(address1, successResult);
                                            while (!writeData.IsSuccess && writeResultTimes <= 3)
                                            {
                                                writeResult = s7Net.Write(address1, successResult);
                                                writeResultTimes++;
                                            }
                                            if (!writeResult.IsSuccess)
                                            {
                                                LogHelper.Info(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为" + result + "但是写入《结果》失败了" + DateTime.Now);
                                            }
                                        }
                                        if (!writeData.IsSuccess)
                                        {
                                            LogHelper.Info(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为" + result + "但是写入《数据》失败了" + DateTime.Now);
                                        }
                                    }
                                    
                                    checkData.Add(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为" + result + DateTime.Now + "  </ br>");
                                }
                            }
                            //判断是否有扫码需求
                            var needSeek2 = saveData2.isReadFidTwo;
                            if (needSeek2)
                            {
                                checkData.Add(dataItem.plc_ip + " 第二个plc请求读取了fid数据" + DateTime.Now + "  </ br>");
                                //寻找对应的ip
                                var ipAddress = fidIps.First(p => p.PLCIpAddress == dataItem.plc_ip && p.SortId == 2);
                                if (ipAddress != null)
                                {
                                    var dataIp = ipAddress.FidIpAddress;
                                    var result = _plcDataSevice.GetLastReadData(dataIp);
                                    //写PLC数据
                                    var address1 = dataItem.plc_dbadr + "." + "40.0";
                                    var address2 = dataItem.plc_dbadr + "." + "42.0";
                                    if (result.Contains("RFID") || result == "")
                                    {
                                    }
                                    else
                                    {
                                        //自动焊报工
                                        if (dataItem.plc_ip == "10.99.109.240" || dataItem.plc_ip == "10.99.109.247")
                                        {
                                            _ = _orderOperateService.ReportAutoTicket(result);
                                        }

                                        int writeDataTimes = 1;
                                        var writeData = s7Net.Write(address2, result);
                                        while (!writeData.IsSuccess && writeDataTimes <= 3)
                                        {
                                            writeData = s7Net.Write(address2, result);
                                            writeDataTimes++;
                                        }
                                        if (writeData.IsSuccess)
                                        {
                                            int writeResultTimes = 1;
                                            short successResult = 1;
                                            var writeResult = s7Net.Write(address1, successResult);
                                            while (!writeResult.IsSuccess && writeResultTimes <= 3)
                                            {
                                                writeResult = s7Net.Write(address1, successResult);
                                                writeResultTimes++;
                                            }
                                            if (!writeResult.IsSuccess)
                                            {
                                                LogHelper.Info(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为" + result + "但是写入《结果》失败了" + DateTime.Now);
                                            }
                                        }
                                        if (!writeData.IsSuccess)
                                        {
                                            LogHelper.Info(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为" + result + "但是写入《数据》失败了" + DateTime.Now);
                                        }
                                    }
                                    //LogHelper.Info(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为" + result);
                                    checkData.Add(dataItem.plc_ip + "扫码仪" + dataIp + " 读取到的fid的数据为" + result + DateTime.Now + "  </ br>");
                                }
                            }
                        }
                    }
                    else
                    {
                        checkData.Add(dataItem.plc_ip + ",通讯异常" + ",原因：" + resultS7net.Message + DateTime.Now + "  </ br>");
                        PlcBufferRack.RemovePlcIp(dataItem.plc_ip);
                    }
                }
                else
                {
                    checkData.Add(dataItem.plc_ip + ",通讯异常" + ",原因：Plc连接失败"+ DateTime.Now + "  </ br>");
                    PlcBufferRack.RemovePlcIp(dataItem.plc_ip);
                }
                
            });
            #endregion

            //把fid信息写入plc
            checkData.Add("fid设备读取结束" + DateTime.Now + "  </ br>");
            _caheManager.Remove("fid_001_check");
            _caheManager.Set("fid_001_check", checkData, 1);
            await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
        }
    }
}
