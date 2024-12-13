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
    public class ApiRecordService
    {
        public void AddApiRecord(third_apirecord model)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                conn.Insert<third_apirecord>(model);
            }
        }

        

    }
}
