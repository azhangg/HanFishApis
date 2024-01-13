using Entities.Goods;
using Models;
using Models.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Goods
{
    public interface IGoodService
    {
        Task<Good> AddGoodAsync(AddGoodModel model);

        Task<bool> DeleteGoodAsync(int id);

        Task<bool> UpdateGoodAsync(UpdateGoodModel model);

        Task<PaginationModel<GoodModel>> GetGoodsToPaginationAsync(int page, int count, string searchText = "", int categoryId = 0,int status = 0);

        Task<GoodModel> GetGoodByIdAsync(int id);

        Task<IEnumerable<GoodModel>> GetUsersGoodByUserIdAsync(int userId);
    }
}
