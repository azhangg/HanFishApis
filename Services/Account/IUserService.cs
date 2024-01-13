using Models;
using Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;

namespace Services.Account
{
    public interface IUserService
    {
        Task<List<UserModel>> GetUsersAsync();

        Task<UserModel> GetUserByIdAsync(int id);

        Task<UserModel> AddUserAsync(UserModel user);

        Task<bool> UpdateUserAsync(UserModel user);

        Task<bool> UpdateUserAsync(UpdateClientUserModel user);

        Task<bool> DeleteUserAsync(int id);

        Task<bool> IsUserExistAsync(string loginName);

        Task<PaginationModel<UserModel>> GetUsersToPagination(string searchText, int page, int count);
    }
}
