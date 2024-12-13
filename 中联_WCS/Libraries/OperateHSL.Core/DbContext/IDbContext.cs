using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OperateHSL.Core.DbContext
{
    public interface IDbContext : IDisposable
    {
        IDbConnection Conn { get; }
        
        string ConnectionString { get; }

        void InitConnection();

        IDbConnection GetLatestConn();

    }
}
