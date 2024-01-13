using Models;
using Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Account
{
    public interface IRoleService
    {
        public Task<List<RoleModel>> GetAllRolesAsync();

        public Task<PaginationModel<RoleModel>> GetRolesToPaginationAsync(int pageIndex, int pageCount);

        public Task<RoleModel> AddRoleAsync(AddRoleModel roleModel);

        public Task<bool> DeleteRoleAsync(int id);

        public Task<bool> UpdateRoleAsync(UpdateRoleModel role);
    }
}
