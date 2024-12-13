using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using OperateHSL.Core.DbContext;
using OperateHSL.Data.DataModel.ThirdPartModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OperateHSL.Services.LogService
{
    public class ApiLogService : IApiLogService
    {
        private readonly IDbContext _dbContext;

        public ApiLogService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async void AddApiLogRecord(Third_ApiRecord model)
        {
            try
            {
                using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
                {
                    if (_conn.State == System.Data.ConnectionState.Closed)
                    {
                        await _conn.OpenAsync();
                    }
                    _conn.Insert<Third_ApiRecord>(model);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void UpdateApiLogRecord(Third_ApiRecord model)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                _conn.Update<Third_ApiRecord>(model);
            }
        }

        public void AddSlicingCallRecord(Third_SlicingCall model)
        {
            try
            {
                var _conn = _dbContext.GetLatestConn();
                _conn.InsertAsync<Third_SlicingCall>(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Third_ApiRecord GetLogInfo(int logId)
        {
            using (var _conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                var selectSql = $"select * from Third_ApiRecord where Id='{logId}'";
                var result = _conn.QueryFirstOrDefault<Third_ApiRecord>(selectSql);
                return result;
            }
        }
    }
}
