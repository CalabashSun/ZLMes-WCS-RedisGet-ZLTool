using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLOperateTool.Models;

namespace ZLOperateTool.Services
{
    public class AgvTaskService
    {
        public int TaskOn(string positionCode)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                conn.Open();
                var selectSql = $"select count(*) from third_agvtask where (StartStation='{positionCode}') and IsDeleted=0 and (`Status`=1 or `Status`=2)";
                var result = conn.QueryFirst<int>(selectSql);
                return result;
            }
        }


        public void CreatAgvTask(third_agvtask model)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                conn.Open();
                conn.Insert<third_agvtask>(model);
            }
        }

        public int AgvTaskStatus(string taskCode)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                var agvTask1 = $"select Status from third_agvtask where TaskNo='{taskCode}'";
                var resultTask1 = conn.QueryFirstOrDefault<int>(agvTask1);
                return resultTask1;
            }
        }
    }
}
