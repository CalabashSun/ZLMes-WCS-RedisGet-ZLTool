using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using OperateHSL.Core.Configuration;
using OperateHSL.Core.Infrastructure;
using OperateHSL.Core.Infrastructure.Dispose;

namespace OperateHSL.Core.DbContext
{
    public class DapperDbContext : DisposableObject, IDbContext
    {
        private readonly string _connections = EngineContext.Current.Resolve<mysql>().connection;
        private bool _committed = true;
        private readonly object _sync = new object();

        public IDbConnection Conn { private set; get; }

        public string ConnectionString { private set; get; }

        public DapperDbContext()
        {
            ConnectionString = _connections;
        }

        public void InitConnection()
        {
            var connection = new MySqlConnection(_connections);
            connection.Open();
            this.Conn = connection;
        }

        public IDbConnection GetLatestConn()
        {
            if (this.Conn==null||this.Conn.State != ConnectionState.Open)
            {
                var connection = new MySqlConnection(_connections);
                connection.Open();
                this.Conn = connection;
            }
            return this.Conn;
        }

        public bool Committed
        {
            set => _committed = value;
            get => _committed;
        }




        public IDbTransaction Tran { private set; get; }

        public void BeginTran()
        {
            this.Tran = this.Conn.BeginTransaction();
            this.Committed = false;
        }

        public void Commit()
        {
            if (Committed) return;
            lock (_sync)
            {
                this.Tran.Rollback();
                this._committed = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            if (this.Conn==null||this.Conn.State != ConnectionState.Open) return;
            Commit();
            this.Conn.Close();
            this.Conn.Dispose();
        }
    }
}
