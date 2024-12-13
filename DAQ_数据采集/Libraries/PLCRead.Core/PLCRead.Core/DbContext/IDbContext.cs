using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Core.DbContext
{
    public interface IDbContext : IDisposable
    {
        IDbConnection Conn { get; }

        string ConnectionString { get; }

        void InitConnection();

        IDbConnection GetLatestConn();

    }
}
