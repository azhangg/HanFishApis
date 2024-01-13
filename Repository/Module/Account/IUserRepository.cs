using Entities.Account;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Module.Account
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserById(int id);
    }
}
