using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using PLCRead.Core.Caching;
using PLCRead.Core.DbContext;
using PLCRead.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Service.ElecService
{
    public interface IPlcElecService
    {
        Task AddElecData(PLC_ElecData model);
    }


    public class PlcElecService : IPlcElecService
    {
        private readonly IDbContext _dbContext;
        private readonly IStaticCacheManager _caheManager;

        public PlcElecService(IDbContext dnContext, IStaticCacheManager cacheManager)
        {
            _dbContext= dnContext;
            _caheManager= cacheManager;
        }

        public async Task AddElecData(PLC_ElecData model)
        {
            using (var conn = new MySqlConnection(_dbContext.ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                await conn.InsertAsync<PLC_ElecData>(model);
            }
        }
    }
}
