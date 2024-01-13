using Entities.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Goods
{
    public interface IGoodCategoryService
    {
        Task<GoodCategory> AddGoodCategoryAscyn(string name);

        Task<bool> DeleteGoodCategoryAsync(int id);

        Task<bool> UpdateGoodCategoryAsync(int id,string name);

        Task<IEnumerable<GoodCategory>> GetGoodCategoriesAsync();
    }
}
