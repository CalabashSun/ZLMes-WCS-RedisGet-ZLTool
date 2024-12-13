using HslCommunication;
using Modbus.Device;
using PLCRead.Core.Caching;
using PLCRead.Core.Configuration;
using PLCRead.Core.Tool;
using PLCRead.Data.AutoWelding;
using PLCRead.Data.Database;
using PLCRead.Data.Flitch;
using PLCRead.Service.ElecService;
using PLCRead.Service.PlcConnectService;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PLCRead.Service.ScheduleTasks.Tasks
{
    public class EletricTask : IScheduledTask
    {
        private IStaticCacheManager _redisManager;
        private IPlcElecService _elecService;
        public EletricTask(IStaticCacheManager redisManager)
        {
            _redisManager = redisManager;
        }

        public int Schedule => 300000;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // 定义电表设备的起始地址和数量
            try
            {
                var dataInfos=new List<PLCElecConfig>
                {
                    new PLCEFlitch1Config(),
                    new PLCEFlitch2Config(),
                    new PLCEFlitch3Config(),

                    new PLCETeamUp1Config(),
                    new PLCETeamUp2Config(),
                    new PLCETeamUp3Config(),
                    new PLCETeamUp4Config(),
                    new PLCETeamUp5Config(),
                    new PLCETeamUp6Config(),

                    new PLCEAuto1Config(),
                    new PLCEAuto2Config(),
                    new PLCEAuto3Config(),
                    new PLCEAuto4Config(),
                    new PLCEAuto5Config(),
                    new PLCEAuto6Config(),
                    new PLCEAuto7Config(),
                    new PLCEAuto8Config(),
                    new PLCEAuto9Config(),
                    new PLCEAuto10Config(),
                    new PLCEAuto11Config(),
                    new PLCEAuto12Config(),
                    new PLCEAuto13Config(),
                    new PLCEAuto14Config(),
                    new PLCEAuto15Config(),
                    new PLCEAuto16Config(),
                    new PLCEAuto17Config(),
                    new PLCEAuto18Config(),
                    new PLCEAuto20Config(),
                    new PLCEAuto21Config(),
                    new PLCEAuto22Config(),
                    new PLCEAuto23Config(),
                    new PLCEAuto24Config(),
                    new PLCEAuto25Config(),
                    new PLCEAuto26Config(),
                    new PLCEAuto27Config(),
                    new PLCEAuto28Config()
                };
                var electrlDatas = new List<PLC_ElecData>();


                var errorData = new List<string>();

                ParallelLoopResult resultRead = Parallel.ForEach(dataInfos, dataItem => {
                    var modbus = new HslCommunication.ModBus.ModbusTcpNet(dataItem.PlcIp);
                    modbus.DataFormat = HslCommunication.Core.DataFormat.ABCD;
                    var uReadData = modbus.ReadFloat("108", 3);

                    if (uReadData != null && uReadData.IsSuccess)
                    {
                        var electrlData = new PLC_ElecData();
                        electrlData.APhaseV = (decimal)uReadData.Content[0];
                        electrlData.BPhaseV = (decimal)uReadData.Content[1];
                        electrlData.CPhaseV = (decimal)uReadData.Content[2];
                        var aReadData = modbus.ReadFloat("116", 3);
                        if (aReadData != null && aReadData.IsSuccess)
                        {
                            electrlData.APhaseA = (decimal)aReadData.Content[0];
                            electrlData.BPhaseA = (decimal)aReadData.Content[1];
                            electrlData.CPhaseA = (decimal)aReadData.Content[2];

                            var toatalReadData = modbus.ReadFloat("160", 2);
                            if (toatalReadData != null && toatalReadData.IsSuccess)
                            {
                                electrlData.UnusefulElec = (decimal)toatalReadData.Content[0];
                                electrlData.UsefulElec = (decimal)toatalReadData.Content[0];
                                electrlData.TotalElec = electrlData.UnusefulElec + electrlData.UsefulElec;

                            }

                        }
                        electrlData.CreateDt = DateTime.Now;
                        electrlData.UpdateDt = DateTime.Now;
                        electrlData.HourData = DateTime.Now.Hour;
                        electrlData.IpAddress = dataItem.PlcIp;
                        electrlData.PlcHex = dataItem.ipcId;
                        electrlData.IpDesc = dataItem.plcDesc;
                        electrlDatas.Add(electrlData);
                        _elecService.AddElecData(electrlData);
                    }
                    else
                    {
                        errorData.Add(dataItem.plcDesc + ":" + dataItem.PlcIp + ",通讯异常" + ",原因：Plc连接失败" + DateTime.Now);
                    }


                });
                var prefexErrorData = new string[] { "eletricerrordata" };
                var cacheKeyErrorData = new CacheKey("eletricerrordata_001", prefexErrorData);
                await _redisManager.SetAsync<List<string>>(cacheKeyErrorData, errorData);

                var prefexdata = new string[] { "eletricdata" };
                var cacheKeydata = new CacheKey("eletricdata_001", prefexdata);
                await _redisManager.SetAsync<List<PLC_ElecData>>(cacheKeydata, electrlDatas);


                await Task.Delay(TimeSpan.FromMilliseconds(Schedule), cancellationToken);
                #region test
                //HslCommunication.ModBus.ModbusTcpNet modbus = new HslCommunication.ModBus.ModbusTcpNet("10.99.109.112");
                //modbus.DataFormat = HslCommunication.Core.DataFormat.ABCD;

                ////108-----A相电压

                ////110----B相电压

                ////112 ----C相电压
                //OperateResult<float[]> uReadData = modbus.ReadFloat("108", 3);


                ////116-----A相电流

                ////118----B相电流

                ////120 ----C相电流
                //var aReadData = modbus.ReadFloat("116", 3);

                ////160  总 有 功 电 度 累 计 值

                ////162  总 无 功 电 度 累 计 值

                //var toatalReadData= modbus.ReadFloat("160",2);


                //Console.WriteLine("success");

                // 创建TCP客户端对象
                //TcpClient tcpClient = new TcpClient("10.99.109.112", 502);  // 串口转网口设备的IP地址和端口号

                //// 创建Modbus主站对象
                //IModbusMaster modbusMaster = ModbusIpMaster.CreateIp(tcpClient);



                //ushort[] registers = modbusMaster.ReadHoldingRegisters(1, StartAddress, 10);
                //var result = ConvertToABCDFormatFloat(registers);



                //// 逐个读取电表设备的数据
                //for (byte deviceAddress = 1; deviceAddress <= 100; deviceAddress++)
                //{
                //    // 读取寄存器数据
                //    ushort[] registers = modbusMaster.ReadHoldingRegisters(deviceAddress, StartAddress, NumRegisters);

                //    // 处理读取到的数据
                //    Console.WriteLine($"Device Address: {deviceAddress}");
                //    for (int i = 0; i < registers.Length; i++)
                //    {
                //        Console.WriteLine($"Register {StartAddress + i}: {registers[i]}");
                //    }
                //}

                // 关闭TCP连接
                //tcpClient.Close();
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
