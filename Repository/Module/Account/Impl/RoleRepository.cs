using Entities.Account;
using Repositories.Base;
using Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Module.Account.Impl
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(HanFishDbContext hanFishDbContext) : base(hanFishDbContext)
        {
        }
    }
}
