using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using OperateHSL.Core.Caching;
using OperateHSL.Core.DbContext;
using OperateHSL.Data.DataModel.PLCModel;
using OperateHSL.Data.DataModel.ThirdPartModel;
using OperateHSL.Data.UsedModel.PLCInfo;
using OperateHSL.Services.Log4netService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperateHSL.Services.PlcInfoService
{

    public interface IPlcDataService
    {
        /// <summary>
        /// 需要读取的数据
        /// </summary>
        /// <param name="type">1：下料线 2：数据线</param>
        /// <returns></returns>
        List<DevicePlcDb> GetReadPlcDevice(int type = 1);
        /// <summary>
        /// 获取下料线plc数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        SlicingPlcDb GetCollectionPlcDb(string code);

        /// <summary>
        /// 获取PLC点位信息
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        SlicingPlcDb GetCollectionPlcData(string code, string dataCode);

        /// <summary>
        /// 通过code获取plc列表db块
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        List<SlicingPlcDb> GetCollectionPlcDbList(string code);
        /// <summary>
        /// 获取fid ip
        /// </summary>
        /// <returns></returns>
        List<PLC_FIDIP> GetPLCFIDIP();

        /// <summary>
        /// 获取rfid设备读取记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        string GetLastReadData(string ip);

        /// <summary>
        /// 获取符合条件的agv点位
        /// </summary>
        /// <param name="materialName"></param>
        /// <returns></returns>
        Third_AgvPosition GetAgvPositionInfo(List<string> materialName);

        Third_AgvPosition GetRemoveAgvPositionInfo(bool isAlone);

        /// <summary>
        /// 获取成品区空闲点位
        /// </summary>
        /// <returns></returns>
        Third_AgvPosition GetFinishArea();

        string GetEndAgvPostion(string name);
        /// <summary>
        /// 新增agv任务
        /// </summary>
        /// <param name="third_AgvTask"></param>
        /// <returns></returns>
        Task AddAgvTaskAsync(Third_AgvTask third_AgvTask);


        /// <summary>
        /// 获取rfid读取记录
        /// </summary>
        /// <returns></returns>
        List<PLC_FIDReadRecord> GetRfidReadData();

        /// <summary>
        /// 获取产线数据
        /// </summary>
        /// <returns></returns>
        List<PLC_FIDTransportRecord> GetTransportData();

        void UpdateTransportRecord(int modelId, short nowCount);


        /// <summary>
        /// 获取PLC点位信息
        /// </summary>
        /// <returns></returns>
        PLC_DeviceDb GetPlcDBInfo(decimal code);
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        void ExcueteSql(string sql);

        /// <summary>
        /// 判断两个点位是否有未执行/正在执行的任务存在
        /// </summary>
        /// <param name="startPostion"></param>
        /// <param name="endPostion"></param>
        /// <returns></returns>

        bool ISExesitAgvTask(string endPostion);

        /// <summary>
        /// 判断该点位是否存在AGV任务
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool PositionIsExistAgvTask(string position);

        /// <summary>
        /// 判断起始点位是否有agv任务
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool PositionStartIsExistAgvTask(string position);

        /// <summary>
        /// 判断目标点位是否有agv任务
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool PositionEndIsExistAgvTask(string position);



        Task modifyAgvPostion(Dictionary<Third_AgvPosition, string> dic);
    }
    public class PlcDataService : IPlcDataService
    {
        private readonly IDbContext _dbContext;
        private readonly ICacheManager _caheManager;

        public PlcDataService(IDbContext dbContext, ICacheManager cacheManager)
        {
            _dbContext = dbContext;
            _caheManager = cacheManager;
        }
        /// <summary>
        /// plc数据读取
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<DevicePlcDb> GetReadPlcDevice(int type = 1)
        {
            var dataResult = new List<DevicePlcDb>();
            var cacheData = _caheManager.Get<List<DevicePlcDb>>("devicedb_" + type);
            if (cacheData == null || cacheData.Count == 0)
            {
                using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
                {
                    if (_conn.State == ConnectionState.Closed)
                    {
                        _conn.Open();
                    }
                    //获取plc列表信息
                    var selectPlcSql = $"select * from PLC_BaseInfo where PlcArea='{type}'";
                    var baseResult = _conn.Query<PLC_BaseInfo>(selectPlcSql).ToList();
                    //plc db
                    var selectPlcDbSql = "select * from PLC_DbInfo";
                    var dbResult = _conn.Query<PLC_DbInfo>(selectPlcDbSql).ToList();
                    //设备
                    var selectDeviceSql = "select * from Work_device";
                    var deviceResult = _conn.Query<Work_device>(selectDeviceSql).ToList();
                    //设备对应数据
                    var selectDevDb = "select * from PLC_DeviceDb";
                    var deviceDbResult = _conn.Query<PLC_DeviceDb>(selectDevDb).ToList();


                    foreach (var item in dbResult)
                    {
                        var dataModel = new DevicePlcDb();
                        var baseIp = baseResult.First(p => p.Id == item.Ass_PLC_BaseInfo);
                        dataModel.plc_ip = baseIp.PlcIp;
                        dataModel.plc_port = baseIp.PlcPort;
                        dataModel.plc_dbadr = item.DbAdr;
                        dataModel.db_length = item.DbLen.ToString();
                        var plcDevices = new List<DeviceInfo>();
                        //获取db块绑定的设备
                        //先获取db块绑定的数据
                        var dbResultDeviceIds = deviceDbResult.Where(p => p.Ass_PLC_DbInfo == item.Id).GroupBy(p => new { p.Ass_Work_device }).ToList();
                        foreach (var itemDevice in dbResultDeviceIds)
                        {
                            var plcDevice = new DeviceInfo();
                            var deviceInfo = deviceResult.First(p => p.Id == itemDevice.Key.Ass_Work_device);
                            var deviceResultInfo = deviceDbResult.Where(p => p.Ass_PLC_DbInfo == item.Id && p.Ass_Work_device == deviceInfo.Id).ToList();
                            var devdbDatas = new List<DeviceDb>();
                            foreach (var devicedbItem in deviceResultInfo)
                            {
                                var devdbData = new DeviceDb();
                                devdbData.data_code = devicedbItem.DataCode;
                                devdbData.data_length = devicedbItem.DataLength.ToString();
                                devdbData.data_name = devicedbItem.DataName;
                                devdbData.data_offset = devicedbItem.DataOffset;
                                devdbData.data_type = devicedbItem.DataType;
                                devdbData.is_use = devicedbItem.IsUse.ToString();
                                devdbData.operate_type = devicedbItem.OperateType.ToString();
                                devdbDatas.Add(devdbData);
                            }
                            plcDevice.device_name = deviceInfo.DeviceName;
                            plcDevice.device_code = deviceInfo.DeviceCode;
                            plcDevice.device_dbs = devdbDatas;
                            plcDevices.Add(plcDevice);
                        }
                        dataModel.plc_devices = plcDevices;
                        dataResult.Add(dataModel);
                    }

                    _caheManager.Set("devicedb_" + type, dataResult, 30);
                }
            }
            else
            {
                dataResult = cacheData;
            }
            return dataResult;
        }

        /// <summary>
        /// 获取下料线plc数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public SlicingPlcDb GetCollectionPlcDb(string code)
        {
            var dataResult = new SlicingPlcDb();
            var cacheData = _caheManager.Get<SlicingPlcDb>("slingcingdb_" + code);
            if (cacheData == null)
            {
                //var _conn = _dbContext.GetLatestConn();
                using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
                {
                    if (_conn.State == ConnectionState.Closed)
                    {
                        _conn.Open();
                        //plc db
                        var selectPlcDbSql = $"select * from PLC_DbInfo where DbCode='{code}'";
                        var dbResult = _conn.QueryFirst<PLC_DbInfo>(selectPlcDbSql);


                        //获取plc列表信息
                        var selectPlcSql = $"select * from PLC_BaseInfo where Id='{dbResult.Ass_PLC_BaseInfo}'";
                        var baseResult = _conn.QueryFirst<PLC_BaseInfo>(selectPlcSql);

                        //设备对应数据
                        var selectDevDb = $"select * from PLC_DeviceDb where Ass_PLC_DbInfo='{dbResult.Id}'";
                        var deviceDbResult = _conn.Query<PLC_DeviceDb>(selectDevDb).ToList();

                        dataResult.plc_ip = baseResult.PlcIp;
                        dataResult.plc_port = baseResult.PlcPort;
                        dataResult.plc_dbadr = dbResult.DbAdr;
                        dataResult.db_length = dbResult.DbLen.ToString();

                        foreach (var itemDb in deviceDbResult)
                        {
                            var dbData = new DataDb();
                            dbData.data_code = itemDb.DataCode;
                            dbData.data_length = itemDb.DataLength.ToString();
                            dbData.data_name = itemDb.DataName;
                            dbData.data_offset = itemDb.DataOffset;
                            dbData.data_type = itemDb.DataType;
                            dbData.is_use = itemDb.IsUse.ToString();
                            dbData.operate_type = itemDb.OperateType.ToString();
                            dbData.data_model = itemDb.DataModel;
                            dataResult.plc_dbInfos.Add(dbData);
                        }
                        _caheManager.Set("slingcingdb_" + code, dataResult, 30);
                    }
                }

            }
            else
            {
                dataResult = cacheData;
            }
            return dataResult;
        }

        /// <summary>
        /// db code集合数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<SlicingPlcDb> GetCollectionPlcDbList(string code)
        {
            var dataResult = new List<SlicingPlcDb>();
            var cacheData = _caheManager.Get<List<SlicingPlcDb>>("slingcingdblist_" + code);
            if (cacheData == null)
            {
                using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
                {
                    if (_conn.State == ConnectionState.Closed)
                    {
                        _conn.Open();
                    }
                    //plc db
                    var selectPlcDbSql = $"select * from PLC_DbInfo where DbCode='{code}' and DbState='1'";
                    var dbResult = _conn.Query<PLC_DbInfo>(selectPlcDbSql);

                    foreach (var itemDbInfo in dbResult)
                    {
                        var dataResultSingle = new SlicingPlcDb();
                        //获取plc列表信息
                        var selectPlcSql = $"select * from PLC_BaseInfo where Id='{itemDbInfo.Ass_PLC_BaseInfo}'";
                        var baseResult = _conn.QueryFirst<PLC_BaseInfo>(selectPlcSql);

                        //设备对应数据
                        var selectDevDb = $"select * from PLC_DeviceDb where Ass_PLC_DbInfo='{itemDbInfo.Id}'";
                        var deviceDbResult = _conn.Query<PLC_DeviceDb>(selectDevDb).ToList();

                        dataResultSingle.plc_ip = baseResult.PlcIp;
                        dataResultSingle.plc_port = baseResult.PlcPort;
                        dataResultSingle.plc_dbadr = itemDbInfo.DbAdr;
                        dataResultSingle.db_length = itemDbInfo.DbLen.ToString();

                        foreach (var itemDb in deviceDbResult)
                        {
                            var dbData = new DataDb();
                            dbData.data_code = itemDb.DataCode;
                            dbData.data_length = itemDb.DataLength.ToString();
                            dbData.data_name = itemDb.DataName;
                            dbData.data_offset = itemDb.DataOffset;
                            dbData.data_type = itemDb.DataType;
                            dbData.is_use = itemDb.IsUse.ToString();
                            dbData.operate_type = itemDb.OperateType.ToString();
                            dbData.data_model = itemDb.DataModel;
                            dataResultSingle.plc_dbInfos.Add(dbData);
                        }

                        dataResult.Add(dataResultSingle);
                    }

                    _caheManager.Set("slingcingdblist_" + code, dataResult, 30);
                }

            }
            else
            {
                dataResult = cacheData;
            }
            return dataResult;
        }

        public List<PLC_FIDIP> GetPLCFIDIP()
        {
            var dataResult = new List<PLC_FIDIP>();
            var cacheData = _caheManager.Get<List<PLC_FIDIP>>("fidDbList");
            if (cacheData == null)
            {
                using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
                {
                    if (_conn.State == ConnectionState.Closed)
                    {
                        _conn.Open();
                    }
                    //plc db
                    var selectFidDbSql = $"select * from PLC_FIDIP where IsUse='1'";
                    dataResult = _conn.Query<PLC_FIDIP>(selectFidDbSql).ToList();
                    _caheManager.Set("fidDbList", dataResult, 30);
                }
            }
            else
            {
                dataResult = cacheData;
            }
            return dataResult;
        }

        /// <summary>
        /// 获取rfid读取记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string GetLastReadData(string ip)
        {
            var dateNowInfo = DateTime.Now;
            var dateNowAddRead = dateNowInfo.AddSeconds(3).ToString("yyyy-MM-dd HH:mm:ss");
            var dateNowSpliceRead = dateNowInfo.AddSeconds(-3).ToString("yyyy-MM-dd HH:mm:ss");
            var dateNowAddWrite = dateNowInfo.AddSeconds(10).ToString("yyyy-MM-dd HH:mm:ss");
            var dateNowSpliceWrite = dateNowInfo.AddSeconds(-10).ToString("yyyy-MM-dd HH:mm:ss");


            var selectCountSql = $"select readresult from plc_fidreadrecord where IpAddress='{ip}' " +
                $"and lastreadtime>= '{dateNowSpliceWrite}' and lastreadtime<= '{dateNowAddWrite}' " +
                $"group by readresult";
            

            var selectSql = $"select ReadResult from plc_fidreadrecord where IpAddress='{ip}' " +
                $"and LastReadTime>='{dateNowSpliceRead}' and LastReadTime<='{dateNowAddRead}' order by id desc limit 0,1";
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                var dataCountResult = _conn.Query<string>(selectCountSql);
                if (dataCountResult.Count() != 1)
                {
                    return "";
                }

                var result = _conn.QueryFirstOrDefault<string>(selectSql);
                result = result.ToUpper();
                if ((!result.Contains("NC") && !result.Contains("WC")))
                {
                    return "";
                }
                else if (result.Length<12)
                {
                    return "";
                }
                else
                {
                    //换向线不去重
                    if (ip != "10.99.109.210" && ip != "10.99.109.211")
                    {
                        
                        //去重
                        var minTime = DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");
                        var ipLastRead = $"select SendResult from plc_fidsendrecord where IpAddress='{ip}' and LastSendTime>'{minTime}' order by LastSendTime desc  limit 0,5";
                        var readData = _conn.Query<string>(ipLastRead).ToList();
                        if (readData.Contains(result))
                        {
                            return "";
                        }
                    }
                    var ipSendata = new PLC_FidSendRecord
                    {
                        IpAddress = ip,
                        SendResult = result,
                        LastSendTime = dateNowInfo
                    };
                    _conn.Insert<PLC_FidSendRecord>(ipSendata);
                    return result;
                }
            }
        }


        /// <summary>
        /// 处理倍速链上线路数据
        /// </summary>
        /// <param name="_conn"></param>
        /// <param name="ip"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task HandleLineData(string ip, string result)
        {
            using (var _taskConn = new MySqlConnection(_dbContext.ConnectionString))
            {
                //判断最近一小时send数量大于1不处理
                var sendCount = $"select count(*) from plc_fidsendrecord where IpAddress='{ip}' and SendResult='{result}' and LastSendTime>NOW() - INTERVAL 1 HOUR;";
                var dataCount = _taskConn.QueryFirst<int>(sendCount);
                if (dataCount > 0)
                {
                    await Task.Delay(10);
                    return;
                }
                var readResultData = result.Substring(0, 5);
                var lineReportSql = $"select * from plc_fidtransportrecord where SumIpAddress='{ip}' and SumModelData='{readResultData}'";
                var lineDataResults = _taskConn.Query<PLC_FIDTransportRecord>(lineReportSql).ToList();
                foreach (var itemlineDataResult in lineDataResults)
                {
                    //增数据
                    itemlineDataResult.SumCount = itemlineDataResult.SumCount + 1;
                    itemlineDataResult.UpdateDt = DateTime.Now;
                    _taskConn.Update<PLC_FIDTransportRecord>(itemlineDataResult);
                }
                if (ip == "10.99.109.237" || ip == "10.99.109.239")
                {
                    //减数据
                    if (result.Contains("12"))
                    {
                        //获取最近一次入库流转
                        var lastInTime = $"select * from plc_fidsendrecord where (IpAddress='10.99.109.210' or IpAddress='10.99.109.211') and SendResult='{result}' order by LastSendTime desc limit 0,1";
                        var lastInTimeResult = _taskConn.QueryFirstOrDefault<PLC_FidSendRecord>(lastInTime);
                        if (lastInTimeResult != null)
                        {
                            //寻找哪些经过的rfid设备
                            var recentIpAddressSql = $"select ipaddress from plc_fidsendrecord where LastSendTime>'{lastInTimeResult.LastSendTime}' and SendResult='{result}'  group by ipaddress";
                            var recentIpAddress = _taskConn.Query<string>(recentIpAddressSql).ToList();
                            if (recentIpAddress != null)
                            {
                                var lineDataSplicesSql = $"select * from plc_fidtransportrecord where sumModelData='{readResultData}' and SumIpAddress in @Values";
                                var lineDataSplices = _taskConn.Query<PLC_FIDTransportRecord>(lineDataSplicesSql, new { Values = recentIpAddress }).ToList();
                                foreach (var itemSplices in lineDataSplices)
                                {
                                    itemSplices.SumCount = itemSplices.SumCount - 1;
                                    itemSplices.UpdateDt = DateTime.Now;
                                    _taskConn.Update<PLC_FIDTransportRecord>(itemSplices);
                                }
                            }
                        }
                    }
                }
                await Task.Delay(10);
            }
        }


        public List<PLC_FIDReadRecord> GetRfidReadData()
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                var selectSql = "SELECT * FROM plc_fidreadrecord WHERE (ipaddress, LastReadTime) IN  " +
                    "(SELECT ipaddress, MAX(LastReadTime)" +
                    " FROM plc_fidreadrecord WHERE LastReadTime >= DATE_SUB(NOW(),INTERVAL 3 SECOND) GROUP BY ipaddress)";
                var result = _conn.Query<PLC_FIDReadRecord>(selectSql).ToList();
                return result;
            }
        }

        public List<PLC_FIDTransportRecord> GetTransportData()
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                var resultDataSql = "select Id,SumCount,SumIpAddress,DbWriteAddress,SumModelData,OrginalIpAddress from plc_fidtransportrecord";
                var resultData = _conn.Query<PLC_FIDTransportRecord>(resultDataSql).ToList();
                return resultData;
            }
        }

        public void UpdateTransportRecord(int modelId, short nowCount)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                var selectSql = $"select * from PLC_FIDTransportRecord where Id='{modelId}'";
                var oldData = _conn.QueryFirstOrDefault<PLC_FIDTransportRecord>(selectSql);
                if (oldData != null)
                {
                    oldData.SumCount = nowCount;
                    oldData.UpdateDt = DateTime.Now;
                    _conn.Update<PLC_FIDTransportRecord>(oldData);
                }

            }
        }



        public void ExcueteSql(string sql)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();

                }
                _conn.ExecuteAsync(sql);
            }
        }

        public SlicingPlcDb GetCollectionPlcData(string code, string dataCode)
        {
            var dataResult = new SlicingPlcDb();
            var cacheData = _caheManager.Get<SlicingPlcDb>("slingcingdb1_" + code+"_"+ dataCode);
            if (cacheData != null)
            {
                return cacheData;
            }
            else
            {
                using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
                {
                    if (_conn.State == ConnectionState.Closed)
                    {
                        _conn.Open();
                        //plc db
                        var selectPlcDbSql = $"select * from PLC_DbInfo where DbCode='{code}'";
                        var dbResult = _conn.QueryFirst<PLC_DbInfo>(selectPlcDbSql);


                        //获取plc列表信息
                        var selectPlcSql = $"select * from PLC_BaseInfo where Id='{dbResult.Ass_PLC_BaseInfo}'";
                        var baseResult = _conn.QueryFirst<PLC_BaseInfo>(selectPlcSql);

                        //设备对应数据
                        var selectDevDb = $"select * from PLC_DeviceDb where Ass_PLC_DbInfo='{dbResult.Id}' AND DataCode='{dataCode}'";
                        var deviceDbResult = _conn.Query<PLC_DeviceDb>(selectDevDb).ToList();

                        dataResult.plc_ip = baseResult.PlcIp;
                        dataResult.plc_port = baseResult.PlcPort;
                        dataResult.plc_dbadr = dbResult.DbAdr;
                        dataResult.db_length = dbResult.DbLen.ToString();

                        foreach (var itemDb in deviceDbResult)
                        {
                            var dbData = new DataDb();
                            dbData.data_code = itemDb.DataCode;
                            dbData.data_length = itemDb.DataLength.ToString();
                            dbData.data_name = itemDb.DataName;
                            dbData.data_offset = itemDb.DataOffset;
                            dbData.data_type = itemDb.DataType;
                            dbData.is_use = itemDb.IsUse.ToString();
                            dbData.operate_type = itemDb.OperateType.ToString();
                            dbData.data_model = itemDb.DataModel;
                            dataResult.plc_dbInfos.Add(dbData);
                        }
                        _caheManager.Set("slingcingdb1_" + code + "_" + dataCode, dataResult, 30);
                    }
                }
                return dataResult;
            }
           

        }

        public Third_AgvPosition GetAgvPositionInfo(List<string> materialName)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                    //设备对应数据
                    foreach (var item in materialName)
                    {
                        var currentPoistionSql = $"select * from Third_AgvPosition where  Ass_Third_AgvArea=8 AND PositionState=2 AND TrayData=2 AND PositionRemark like '%{item}%'";
                        var currentPoistion = _conn.Query<Third_AgvPosition>(currentPoistionSql).FirstOrDefault();
                        if (currentPoistion != null)
                        {
                            return currentPoistion;
                        }
                    }

                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public PLC_DeviceDb GetPlcDBInfo(decimal code)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                    //设备对应数据
                    var selectDevDb = $"select * from PLC_DeviceDb where Ass_PLC_DbInfo='{26}' AND DataOffset={code}";
                    var deviceDbResult = _conn.Query<PLC_DeviceDb>(selectDevDb).FirstOrDefault();
                    return deviceDbResult;
                }
                else
                {
                    return null;
                }
            }
        }

        public string GetEndAgvPostion(string name)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                    //设备对应数据
                    var selectDevDb = $"select * from third_agvposition where Ass_Third_AgvArea=9 AND (PositionName='{name}_1' OR PositionName='{name}_2') ";
                    var deviceDbResult = _conn.Query<Third_AgvPosition>(selectDevDb).Where(p => p.PositionInBind == "0" && p.PositionState == 1).FirstOrDefault();
                    if (deviceDbResult != null)
                    {
                        return deviceDbResult.PositionCode;

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }


        }



        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="third_AgvTask"></param>
        /// <returns></returns>
        public async Task AddAgvTaskAsync(Third_AgvTask third_AgvTask)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }

                await conn.InsertAsync<Third_AgvTask>(third_AgvTask);
            }
        }



        public Third_AgvPosition GetRemoveAgvPositionInfo(bool isAlone)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                    //设备对应数据

                    var currentPoistionSql = $"select* from Third_AgvPosition where  Ass_Third_AgvArea = 8 AND PositionState = 2 AND TrayData = 2 AND(PositionRemark not  like '%NC1%' && PositionRemark not  like '%NC3%' && PositionRemark not  like '%NC5%' && PositionRemark not  like '%1212WC1%')";

                    var currentPoistion = _conn.Query<Third_AgvPosition>(currentPoistionSql);

                    if (isAlone == true)
                    {
                        currentPoistion = currentPoistion.Where(p => p.TrayType == 60);
                    }
                    var result = currentPoistion.FirstOrDefault();
                    return result;

                }
                else
                {
                    return null;
                }
            }
            return null;


        }

        public bool ISExesitAgvTask(string endPostion)
        {

            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                    //设备对应数据

                    var currentPoistionSql = $"select* from third_agvtask where  EndStation='{endPostion}' AND  TaskStatus=1 AND (Status=0 OR Status=1 OR Status=2)";
                    var currentPoistion = _conn.Query<Third_AgvTask>(currentPoistionSql).FirstOrDefault();
                    if (currentPoistion != null)
                    {
                        return false;
                    }
                    {
                        return true;

                    }

                }

            }

            return false;

        }


        public bool PositionIsExistAgvTask(string position)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                    //设备对应数据
                    var dateNow = DateTime.Now.AddHours(-3).ToString("yyyy-MM-dd HH:mm:ss");
                    var currentPoistionSql = $"select count(*) from third_agvtask where  (EndStation='{position}' or StartStation='{position}') AND  `Status`!=3 and `Status`!=4 and UpdateDt>'{dateNow}'";
                    var currentPoistion = _conn.Query<int>(currentPoistionSql).FirstOrDefault();
                    if (currentPoistion>0)
                    {
                        return true;
                    }
                    {
                        return false;

                    }
                }
            }
            return true;
        }


        public bool PositionStartIsExistAgvTask(string position)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                    //设备对应数据

                    var currentPoistionSql = $"select count(*) from third_agvtask where StartStation='{position}'  AND  `Status`!=3 and `Status`!=4";
                    var currentPoistion = _conn.Query<int>(currentPoistionSql).FirstOrDefault();
                    if (currentPoistion > 0)
                    {
                        LogHelper.Info("截获相同任务重新发送，重复点位为："+position);
                        return true;
                    }
                    {
                        return false;

                    }
                }
            }
            return true;
        }


        public bool PositionEndIsExistAgvTask(string position)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                    //设备对应数据

                    var currentPoistionSql = $"select count(*) from third_agvtask where EndStation='{position}'  AND  `Status`!=3 and `Status`!=4";
                    var currentPoistion = _conn.Query<int>(currentPoistionSql).FirstOrDefault();
                    if (currentPoistion > 0)
                    {
                        return true;
                    }
                    {
                        return false;

                    }
                }
            }
            return true;
        }


        public async Task modifyAgvPostion(Dictionary<Third_AgvPosition, string> dic)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                    //设备对应数据

                    var key = dic.Keys.FirstOrDefault();
                    var value = dic.Values.FirstOrDefault();
                    string[] values = new[] { key.PositionCode, value };

                    foreach (var item in values)
                    {
                        var currentPoistionSql = $"select* from Third_AgvPosition where   PositionCode='{item}' ";
                        var currentPoistion = _conn.Query<Third_AgvPosition>(currentPoistionSql).FirstOrDefault();
                        currentPoistion.PositionState = 3;
                        currentPoistion.TrayType = key.TrayType;
                        currentPoistion.UpdateDt = DateTime.Now;
                        await _conn.UpdateAsync(currentPoistion);
                    }
                }
            }

        }


        public Third_AgvPosition GetFinishArea()
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                    //设备对应数据
                    var currentPoistionSql = $"select* from Third_AgvPosition where  Ass_Third_AgvArea=10 AND PositionInBind='0' AND PositionState=1  AND TrayType=0 AND TrayData=0 ";
                    var currentPoistion = _conn.Query<Third_AgvPosition>(currentPoistionSql).FirstOrDefault();
                    return currentPoistion;
                }
                else
                {
                    return null;
                }

            }


        }

      
    }
}
