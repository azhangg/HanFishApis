using Entities.Account;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Module.Account.impl
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(HanFishDbContext hanFishDbContext) : base(hanFishDbContext)
        {
        }

        public async Task<User> GetUserById(int id)
        {
            return await CurrentDbContext.Set<User>().Include(u => u.Role).FirstAsync(u => u.Id ==id);
        }
    }
}
