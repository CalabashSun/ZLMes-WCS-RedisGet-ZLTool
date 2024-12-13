using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using Operate.Services.PlcConnectService;
using OperateHSL.Core.Caching;
using OperateHSL.Core.DbContext;
using OperateHSL.Data.DataModel.ThirdPartModel;
using OperateHSL.Data.PLC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperateHSL.Services.AgvPosition
{
    public interface IAgvPositionService
    {
        /// <summary>
        /// 获取点位信息
        /// </summary>
        /// <param name="positionCode"></param>
        /// <returns></returns>
        Task<Third_AgvPosition> GetPositionInfo(string positionCode);
        /// <summary>
        /// 修改点位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdatePositionInfo(Third_AgvPosition model);

        /// <summary>
        /// 修改下件区域所有指定的点位为空托盘
        /// </summary>
        /// <param name="positons"></param>
        /// <returns></returns>
        Task UpdateChoosedPosition(List<StructuralOtherData> model);


        /// <summary>
        /// 修改下件区域所有指定的点位为空位置
        /// </summary>
        /// <param name="positons"></param>
        /// <returns></returns>
        Task UpdateChoosedPositionEmpty(List<StructuralOtherData> model);

        /// <summary>
        /// 打磨区点位是否有正在执行的任务
        /// </summary>
        /// <param name="positionName"></param>
        /// <returns></returns>
        Task<int> GetRepairePositionTaskCount(string positionName);

        /// <summary>
        /// 打磨区可用物料空位置
        /// </summary>
        /// <param name="positionName"></param>
        /// <returns></returns>
        Task<Third_AgvPosition> GetRepairePositionData(string positionName);




        /// <summary>
        /// 打磨区可用物料实物托盘
        /// </summary>
        /// <param name="positionName"></param>
        /// <returns></returns>
        Task<Third_AgvPosition> GetRepairePositionEndData(string positionName);

        Task<Third_AgvTask> GetThirdAGVTask(string positionCode);



        /// <summary>
        /// 下件区可用物料托盘
        /// </summary>
        /// <returns></returns>
        Task<Third_AgvPosition> GetRepaireEndPositionData(int trayType = -1, int workSeatId = -1);


        /// <summary>
        /// 下件区所有可用物料托盘
        /// </summary>
        /// <returns></returns>
        Task<List<Third_AgvPosition>> GetStructrualEndPositionDatas();
        /// <summary>
        /// 人工打磨缓存区所有空置点位
        /// </summary>
        /// <returns></returns>
        Task<List<Third_AgvPosition>> GetRepaireEmptyPositionDatas();



        /// <summary>
        /// 上挂区空置点位
        /// </summary>
        /// <returns></returns>
        Task<Third_AgvPosition> GetRickingEndEmtyPositionData();


        /// <summary>
        /// 上挂区空置点位
        /// </summary>
        /// <returns></returns>
        Task<Third_AgvPosition> GetRickingUpCatchPositionData();

        /// <summary>
        /// 打磨完成缓存点位
        /// </summary>
        /// <returns></returns>
        Task<Third_AgvPosition> GetDMComplishPositionData();

        /// <summary>
        /// 上挂区空托盘
        /// </summary>
        /// <returns></returns>
        Task<Third_AgvPosition> GetRickingEndEmtyBoxPositionData(int type);

        /// <summary>
        /// 获取下件plc点位对应的agv点位数据
        /// </summary>
        /// <returns></returns>
        Task<List<Third_StructuralAgvPlcPosition>> GetAgvPlcPostions();


        /// <summary>
        /// 获取下件缓存区空托盘
        /// </summary>
        /// <returns></returns>
        Task<List<Third_AgvPosition>> GetStructuralEmptyTray();

        /// <summary>
        /// 获取下件缓存区空位置
        /// </summary>
        /// <returns></returns>
        Task<Third_AgvPosition> GetStructuralEmptyPostion();

        /// <summary>
        /// 获取上挂区空置托盘
        /// </summary>
        /// <returns></returns>
        Task<List<Third_AgvPosition>> GetRickingEmptyTray(int taskCount = -1);

        /// <summary>
        /// 获取下件区空置点位
        /// </summary>
        /// <returns></returns>
        Task<List<Third_AgvPosition>> GetStructuralEmptyPosition();


        Task<Third_AgvPosition> GetDMFAnotherPostion(string name, string code);

        Task HanldeWeldingPlcData(int type, int placeHex);
    }

    public class AgvPositionService : IAgvPositionService
    {

        private readonly IDbContext _dbContext;
        private readonly ICacheManager _caheManager;


        public AgvPositionService(IDbContext dbContext
            , ICacheManager cacheManager)
        {
            _dbContext = dbContext;
            _caheManager = cacheManager;
        }

        /// <summary>
        /// 获取点位信息
        /// </summary>
        /// <param name="positionCode"></param>
        /// <returns></returns>
        public async Task<Third_AgvPosition> GetPositionInfo(string positionCode)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var selectSql = $"select * from third_agvposition where PositionCode='{positionCode}' and PositionInBind=0";
                var result = await conn.QueryFirstOrDefaultAsync<Third_AgvPosition>(selectSql);
                return result;
            }
        }

        /// <summary>
        /// 更新点位信息
        /// </summary>
        /// <param name="model"></param>
        public async Task UpdatePositionInfo(Third_AgvPosition model)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                await conn.UpdateAsync<Third_AgvPosition>(model);
            }
        }

        public async Task UpdateChoosedPosition(List<StructuralOtherData> model)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var dateNow = DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:ss");
                var updateSql = $"update Third_AgvPosition set TrayData='1',PositionState='2',UpdateDt=NOW() where PositionState!=3 and  UpdateDt<'{dateNow}' and PositionCode =@postion";
                await conn.ExecuteAsync(updateSql, model);
            }
        }

        public async Task UpdateChoosedPositionEmpty(List<StructuralOtherData> model)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var dateNow = DateTime.Now.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss");
                var updateSql = $"update Third_AgvPosition set TrayData='0',PositionState='1',UpdateDt=NOW() where PositionState!=3 and  UpdateDt<'{dateNow}'  and PositionCode =@postion";
                await conn.ExecuteAsync(updateSql, model);
            }
        }

        /// <summary>
        /// 打磨区点位是否有正在执行的任务
        /// </summary>
        /// <param name="positionName"></param>
        /// <returns></returns>
        public async Task<int> GetRepairePositionTaskCount(string positionName)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var compareDate = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                var selectSql = $"select count(*) from third_agvposition where PositionName like '%{positionName}%' and PositionState='3'";
                var datacount = await conn.QueryFirstOrDefaultAsync<int>(selectSql);
                return datacount;
            }
        }

        /// <summary>
        /// 获取打磨房可用空置点位信息
        /// </summary>
        /// <param name="positionName"></param>
        /// <returns></returns>
        public async Task<Third_AgvPosition> GetRepairePositionData(string positionName)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=9 AND  PositionName like '%{positionName}%' and TrayData='0' and PositionState='1' and  PositionInBind='0'";
                if (positionName == "人工打磨区1")
                {
                    selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=9  AND  (PositionCode='1236' OR PositionCode='1237') and TrayData='0' and PositionState='1' and  PositionInBind='0'";

                }

                var result = await conn.QueryFirstOrDefaultAsync<Third_AgvPosition>(selectSql);
                return result;
            }
        }

        /// <summary>
        /// 获取打磨房可用实物点位信息
        /// </summary>
        /// <param name="positionName"></param>
        /// <returns></returns>
        public async Task<Third_AgvPosition> GetRepairePositionEndData(string positionName)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var dateNow = DateTime.Now.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss");
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=9  AND  PositionName like '%{positionName}%' and TrayData='1' and PositionState='2' and  UpdateDt<'{dateNow}' and  PositionInBind='0'";
                if (positionName == "人工打磨区1")
                {
                    selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=9  AND  (PositionCode='1236' OR PositionCode='1237') and TrayData='1' and PositionState='2' and  UpdateDt<'{dateNow}' and  PositionInBind='0'";

                }

                var result = await conn.QueryFirstOrDefaultAsync<Third_AgvPosition>(selectSql);
                return result;
            }
        }


        public async Task<Third_AgvTask> GetThirdAGVTask(string positionCode)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var dateNow = DateTime.Now.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss");

                var selectSql = $"select * from third_agvtask where    EndStation= '{positionCode}'  and  TaskType='补焊房呼叫托盘'  and (Status='1' or  Status='2' or Status='3')and  UpdateDt>'{dateNow}' order by UpdateDt desc";
                var result = await conn.QueryFirstOrDefaultAsync<Third_AgvTask>(selectSql);
                return result;
            }
        }
        public async Task<Third_AgvPosition> GetRepaireEndPositionData(int trayType = -1, int workSeatId = -1)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=11  and TrayData='2' and PositionState='2'  and PositionInBind='0'  ";

                if (trayType != -1)
                {
                    selectSql = selectSql + $" and TrayType='{trayType}'";
                }

                if (workSeatId == 5)
                {
                    //04wc2 06WC2 06WC3 12WC2 12WC3  08WC2 08WC3
                    // selectSql = selectSql + $" and TrayType='60'";
                    //selectSql = selectSql + $" and (TrayMaterial='04NC1' or TrayMaterial='04NC3')";
                    selectSql = selectSql + $" and (PositionRemark like '%04NC1%' or PositionRemark like '%04NC3%')";
                }
                else
                {
                    selectSql = selectSql + $" and (PositionRemark  not like '%04NC1%' AND PositionRemark not like '%04NC3%')";

                }
                selectSql = selectSql + " order by UpdateDt";

                var result = await conn.QueryFirstOrDefaultAsync<Third_AgvPosition>(selectSql);
                return result;
            }
        }
        public async Task<List<Third_AgvPosition>> GetStructrualEndPositionDatas()
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                //满框后等待60s
                var updateDtFull = DateTime.Now.AddSeconds(-60);

                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=8  and TrayData='2' and PositionState='2' and  PositionInBind='0' and (AreaPartTwo='1' or  AreaPartTwo='2')";

                selectSql = selectSql + " order by UpdateDt limit 0,10";

                var result = await conn.QueryAsync<Third_AgvPosition>(selectSql);
                return result.ToList();
            }
        }
        public async Task<List<Third_AgvPosition>> GetRepaireEmptyPositionDatas()
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=11  and TrayData='0' and PositionState='1'  and PositionInBind='0'";

                selectSql = selectSql + " order by UpdateDt limit 0,10";

                var result = await conn.QueryAsync<Third_AgvPosition>(selectSql);
                return result.ToList();
            }
        }
        public async Task<Third_AgvPosition> GetRickingEndEmtyPositionData()
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=10   and TrayData='0' and PositionState='1'  and PositionInBind='0'  and PositionCode>='3001' order by PositionCode";
                var result = await conn.QueryFirstOrDefaultAsync<Third_AgvPosition>(selectSql);
                return result;
            }
        }

        public async Task<Third_AgvPosition> GetRickingUpCatchPositionData()
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=10   and TrayData='0' and PositionState='1'  and PositionInBind='0'  and PositionCode<'3001' order by PositionCode";
                var result = await conn.QueryFirstOrDefaultAsync<Third_AgvPosition>(selectSql);
                return result;
            }
        }

        public async Task<Third_AgvPosition> GetDMComplishPositionData()
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=10  and  PositionInBind=0   and TrayData='2' and PositionState='2'  and PositionInBind='0'  and PositionCode>='3001' order by PositionCode";
                var result = await conn.QueryFirstOrDefaultAsync<Third_AgvPosition>(selectSql);
                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Third_StructuralAgvPlcPosition>> GetAgvPlcPostions()
        {
            var cacheData = _caheManager.Get<List<Third_StructuralAgvPlcPosition>>("structuralagvplcposition");
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

                    }
                    var dataResult = new List<Third_StructuralAgvPlcPosition>();
                    dataResult = _conn.Query<Third_StructuralAgvPlcPosition>("select * from Third_StructuralAgvPlcPosition").ToList();
                    _caheManager.Set("structuralagvplcposition", dataResult, 30);
                    await Task.Delay(10);
                    return dataResult;
                }
            }
        }

        public async Task<List<Third_AgvPosition>> GetStructuralEmptyTray()
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }

                var dataResult = await _conn.QueryAsync<Third_AgvPosition>("select * from third_agvposition where Ass_Third_AgvArea=8 and TrayData=1 and PositionState=2 and (AreaPartTwo='3'||AreaPartTwo='4') order by UpdateDt");
                var dataResultList = dataResult.ToList();
                return dataResultList;
            }
        }


        public async Task<List<Third_AgvPosition>> GetRickingEmptyTray(int takeCount = -1)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();

                }
                var selectSql = "select* from third_agvposition where Ass_Third_AgvArea = 10 and TrayData = 1 and PositionState = 2 order by UpdateDt";

                if (takeCount > 0)
                {
                    selectSql += $" limt 0,{takeCount}";
                }
                var dataResult = await _conn.QueryAsync<Third_AgvPosition>(selectSql);
                var dataResultList = dataResult.ToList();
                return dataResultList;
            }
        }

        public async Task<List<Third_AgvPosition>> GetStructuralEmptyPosition()
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=8 and TrayData=0 and PositionState=1 and PositionInBind=0 and (AreaPartTwo='1'||AreaPartTwo='2') order by UpdateDt";
                var dataResult = await _conn.QueryAsync<Third_AgvPosition>(selectSql);
                var dataResultList = dataResult.ToList();
                return dataResultList;
            }
        }
        public async Task<Third_AgvPosition> GetStructuralEmptyPostion()
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=8 and TrayData=0 and PositionState=1 and PositionInBind=0    and (AreaPartTwo='3'||AreaPartTwo='4')  order by Rand()  LIMIT 0,1 ";
                var dataResult = await _conn.QueryAsync<Third_AgvPosition>(selectSql);
                var dataResultList = dataResult.FirstOrDefault();
                return dataResultList;
            }
        }


        public async Task<Third_AgvPosition> GetRickingEndEmtyBoxPositionData(int type)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=10  and PositionState=2  AND TrayData='1' and TrayType={type}  and PositionInBind=0  order by UpdateDt";
                var dataResult = await _conn.QueryAsync<Third_AgvPosition>(selectSql);
                var dataResultList = dataResult.FirstOrDefault();
                return dataResultList;
            }
        }

        public async Task<Third_AgvPosition> GetDMFAnotherPostion(string name, string code)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                var selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=9  AND PositionInBind=0   AND  PositionName like '%{name}%' AND  PositionCode!='{code}'";
                if (name == "人工打磨区1")
                {
                    selectSql = $"select * from third_agvposition where Ass_Third_AgvArea=9  AND PositionInBind=0   AND  PositionName like '%{name}%' AND  PositionCode!='{code}' ORDER BY PositionCode desc  LIMIT 0,1";

                }

                var dataResult = await _conn.QueryAsync<Third_AgvPosition>(selectSql);
                var dataResultList = dataResult.FirstOrDefault();
                return dataResultList;
            }
        }

        public async Task HanldeWeldingPlcData(int type, int placeHex)
        {
            var s7Net = PlcBufferRack.Instance("10.99.108.161");
            var handleType = 8;
            if (type == 6)
            {
                handleType = 12;
            }

            if (type == 5)
            {
                if (placeHex == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX0.0").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".0", true);
                        }

                    }

                }
                else if (placeHex == 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX0.1").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".1", true);
                        }
                    }

                }
                else if (placeHex == 3)
                {

                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX0.2").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".2", true);
                        }
                    }

                }
                else if (placeHex == 4)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX0.3").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".3", true);
                        }
                    }

                }
                else if (placeHex == 5)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX0.4").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".4", true);
                        }
                    }

                }
                else if (placeHex == 6)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX0.5").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".5", true);
                        }
                    }

                }
                else if (placeHex == 7)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX0.6").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".6", true);
                        }
                    }

                }
                else if (placeHex == 8)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX0.7").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".7", true);
                        }
                    }

                }
                else if (placeHex == 9)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var handleNew = handleType + 1;
                        var conte = s7Net.ReadBool("DB100.DBX1.0").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleNew + ".0", true);
                        }


                    }

                }
                else if (placeHex == 10)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var handleNew = handleType + 1;
                        var conte = s7Net.ReadBool("DB100.DBX1.1").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleNew + ".1", true);
                        }
                    }

                }
                else if (placeHex == 11)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var handleNew = handleType + 1;
                        var conte = s7Net.ReadBool("DB100.DBX1.2").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleNew + ".2", true);
                        }
                    }

                }
                else if (placeHex == 12)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var handleNew = handleType + 1;
                        var conte = s7Net.ReadBool("DB100.DBX1.3").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleNew + ".3", true);
                        }
                    }

                }




            }
            else {

                if (placeHex == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        //var conte = s7Net.ReadBool("DB100.DBX4.0").Content;
                        //if (Convert.ToBoolean(conte) == true)
                        //{
                        //    s7Net.Write("DB100.DBX" + handleType + ".0", true);
                        //}
                        var conte1 = s7Net.ReadBool("DB100.DBX5.4").Content;
                        if (Convert.ToBoolean(conte1) == true)
                        {
                            s7Net.Write("DB100.DBX13.4", true);
                        }

                    }

                }
                else if (placeHex == 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX4.1").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".1", true);
                        }
                    }

                }
                else if (placeHex == 3)
                {

                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX4.2").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".2", true);
                        }
                    }

                }
                else if (placeHex == 4)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX4.3").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".3", true);
                        }
                    }

                }
                else if (placeHex == 5)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX4.4").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".4", true);
                        }
                    }

                }
                else if (placeHex == 6)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX4.5").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".5", true);
                        }
                    }

                }
                else if (placeHex == 7)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX4.6").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".6", true);
                        }
                    }

                }
                else if (placeHex == 8)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var conte = s7Net.ReadBool("DB100.DBX4.7").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleType + ".7", true);
                        }
                    }

                }
                else if (placeHex == 9)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var handleNew = handleType + 1;
                        var conte = s7Net.ReadBool("DB100.DBX5.0").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleNew + ".0", true);
                        }


                    }

                }
                else if (placeHex == 10)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var handleNew = handleType + 1;
                        var conte = s7Net.ReadBool("DB100.DBX5.1").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleNew + ".1", true);
                        }
                    }

                }
                else if (placeHex == 11)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var handleNew = handleType + 1;
                        var conte = s7Net.ReadBool("DB100.DBX5.2").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleNew + ".2", true);
                        }
                    }

                }
                else if (placeHex == 12)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var handleNew = handleType + 1;
                        var conte = s7Net.ReadBool("DB100.DBX5.3").Content;
                        if (Convert.ToBoolean(conte) == true)
                        {
                            s7Net.Write("DB100.DBX" + handleNew + ".3", true);
                        }
                    }

                }


            }
    
            await Task.Delay(10);
        }

        
    }
}
