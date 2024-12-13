using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLOperateTool.Models;

namespace ZLOperateTool.Services
{
    public class AgvPositionService
    {
        /// <summary>
        /// 获取agv点位
        /// </summary>
        /// <param name="type">1：下料线叫料位 2：下料线缓存区 3：拣选区</param>
        /// <returns></returns>
        public List<third_agvposition> GetAgvPosition(int type)
        {
            var connection= ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                conn.Open();
                var selectSql = $"select PositionCode,PositionName from third_agvposition where Ass_Third_AgvArea='{type}' and PositionInBind=0";
                var result = conn.Query<third_agvposition>(selectSql).ToList();
                return result;
            }
        }

        /// <summary>
        /// 获取贴板对应工控机agv点位
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<third_agvposition> GetFlitchAgvPosition(int type,int area2)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                conn.Open();
                var selectSql = $"select PositionCode,PositionName from third_agvposition where Ass_Third_AgvArea='{type}' and AreaPartTwo='{area2}'  and PositionInBind=0";
                var result = conn.Query<third_agvposition>(selectSql).ToList();
                return result;
            }
        }







        /// <summary>
        /// 获取agv点位 下料线打磨完工的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<third_agvposition> GetAgvPositionWherePolishCompleted(int type=2)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                conn.Open();
                var selectSql = $"select PositionCode,PositionName,PositionRemark from third_agvposition where Ass_Third_AgvArea='{type}' and PositionInBind=0 and TrayData=6 and PositionState=2";
                var result = conn.Query<third_agvposition>(selectSql).ToList();
                return result;
            }
        }


        /// <summary>
        /// 获取下料线空托盘
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public third_agvposition CallCollectionEmptyTray()
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                
               
                var selectSql = $"select PositionCode,PositionName from third_agvposition where Ass_Third_AgvArea=2 and PositionState=2 and TrayData=1 and TrayType=1 and PositionInBind=0  limit 0,1";
                var result = conn.Query<third_agvposition>(selectSql).FirstOrDefault();
                if (result != null)
                {
                    return result;
                }
                else 
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 呼叫拣选位空托盘
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public third_agvposition CallPickingEmptyTray(string trayType)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }


                var selectSql = $"select PositionCode,PositionName from third_agvposition where Ass_Third_AgvArea=5 and PositionState=2 and TrayData=1 and TrayType='{trayType}' and PositionInBind=0 limit 0,1";
                var result = conn.Query<third_agvposition>(selectSql).FirstOrDefault();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 呼叫贴板焊接空托盘
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public third_agvposition CallFlitchEmptyTray()
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }


                var selectSql = $"select PositionCode,PositionName from third_agvposition where Ass_Third_AgvArea=5 and PositionState=2 and TrayData=1 and TrayType=3 and PositionInBind=0 limit 0,1";
                var result = conn.Query<third_agvposition>(selectSql).FirstOrDefault();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 寻找空点位
        /// </summary>
        /// <returns></returns>
        public third_agvposition CallCollectionEmptyPosition(int area)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }


                var selectSql = $"select PositionCode,PositionName from third_agvposition where Ass_Third_AgvArea={area} and PositionState=1 and TrayData=0 and PositionInBind=0  limit 0,1";
                var result = conn.QueryFirstOrDefault<third_agvposition>(selectSql);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 下料线指定类型的点位
        /// </summary>
        /// <returns></returns>
        public third_agvposition CallMaterialEmptyPosition(string typeInfo)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }


                var selectSql = $"select PositionCode,PositionName from third_agvposition where Ass_Third_AgvArea=2 and PositionState=1 and TrayData=0 and PositionInBind=0";
                if (typeInfo == "04")
                {
                    selectSql = selectSql + " and (PositionCode>='2026' and PositionCode<='2029')";
                }
                else if (typeInfo == "06")
                {
                    selectSql = selectSql + " and (PositionCode>='2016' and PositionCode<='2020')";
                }
                else if (typeInfo == "12")
                {
                    selectSql = selectSql + " and ((PositionCode>='2006' and PositionCode<='2010') or (PositionCode>='2021' and PositionCode<='2025'))";
                }
                selectSql = selectSql + " limit 0,1";

                var result = conn.QueryFirstOrDefault<third_agvposition>(selectSql);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取指定点位
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public third_agvposition GetPosition(string code)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }


                var selectSql = $"select * from third_agvposition where PositionCode='{code}'";
                var result = conn.QueryFirstOrDefault<third_agvposition>(selectSql);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public void UpdatePosition(third_agvposition model)
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                conn.Update<third_agvposition>(model);
            }
        }


        public List<third_agvposition> GetPickingCompletedTray()
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                var selectSql = "select * from third_agvposition where PositionState=2 and TrayData=4";
                var result = conn.Query<third_agvposition>(selectSql).ToList();
                return result;
            }
        }
        public List<third_agvposition> GetFlitchCompletedTray()
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                var selectSql = "select * from third_agvposition where PositionState=2 and TrayData=2 and Ass_Third_AgvArea=5 and TrayType=3";
                var result = conn.Query<third_agvposition>(selectSql).ToList();
                return result;
            }
        }

        public List<third_agvposition> GetPipeCompletedTray()
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                var selectSql = "select * from third_agvposition where PositionState=2 and TrayData=2 and Ass_Third_AgvArea=2";
                var result = conn.Query<third_agvposition>(selectSql).ToList();
                return result;
            }
        }


        public List<third_traytype> GetPickingTrayType()
        {
            var connection = ConfigurationManager.AppSettings["dbconnection"];
            using (var conn = new MySqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                var selectSql = "select * from third_traytype where TypeArea=2";
                var result = conn.Query<third_traytype>(selectSql).ToList();
                return result;
            }
        }
    }
}
