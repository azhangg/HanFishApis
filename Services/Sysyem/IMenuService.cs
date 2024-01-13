using Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Sysyem
{
    public interface IMenuService
    {
        Task<List<MenuModel>> GetAllMenuAsync();

        Task<MenuModel> AddMenuAsync(AddMenuModel model);

        Task<bool> DeleteMenuAsync(int id);

        Task<bool> UpdateMenuAsync(UpdateMenuModel model);

        Task<List<MenuModel>> GetMenusByUserId(int id);
    }
}
