using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using ZLOperateTool.Models;

namespace ZLOperateTool.Services
{
    public class BankService
    {
        public List<View_InterContainer_WCS> GetBankData()
        {
            var connection = ConfigurationManager.AppSettings["bankdbconnection"];
            using (var conn = new SqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                var selectSql = "select * from View_InterContainer_WCS where OutStatus is null order by LastActionTime";
                var result = conn.Query<View_InterContainer_WCS>(selectSql).ToList();
                return result;
            }
        }


        public string GetLastestOutData()
        {
            var connection = ConfigurationManager.AppSettings["bankdbconnection"];
            using (var conn = new SqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                var orderCountSql = "SELECT Remark FROM InterOutCommand_WCS ORDER BY CreateTime DESC";
                var resultOrder = conn.QueryFirstOrDefault<string>(orderCountSql);
                return resultOrder;
            }
        }


        public void AddBankOutData(string stockNum,string quantity,string lastOrderData,string productName,string dataId)
        {
            var connection = ConfigurationManager.AppSettings["bankdbconnection"];
            using (var conn = new SqlConnection(connection))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                string inserSql = "INSERT INTO InterOutCommand_WCS";
                inserSql += "(StockNum,Quantity,[Order],Status,CreateTime";
                inserSql += ",Remark,ProductName,MaterialCode)";
                inserSql += " VALUES(";
                inserSql += $"'{stockNum}','{quantity}','1','-1',getdate(),";
                inserSql += $"'{lastOrderData}','{productName}','{dataId}'";
                inserSql += " )";
                conn.Execute(inserSql);
            }
        }


    }
}
