using Dapper;
using Entities.System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Repositories.Base;
using Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Module.System.Impl
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(HanFishDbContext hanFishDbContext) : base(hanFishDbContext)
        {
        }

        public async Task<List<Menu>> GetMenusByUserIds(List<int> ids)
        {
            using (var conn = new SqlConnection(ConnectionString)) 
            {
                var sql = @"SELECT * FROM Menu WHERE Id in @ids";
                var result = await conn.QueryAsync<Menu>(sql,new { ids });
                return result.ToList();
            }
        }
    }
}
