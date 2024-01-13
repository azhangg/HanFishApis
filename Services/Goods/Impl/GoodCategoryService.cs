using Entities.Goods;
using Repositories.Module.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Services.Goods.Impl
{
    internal class GoodCategoryService : IGoodCategoryService
    {
        private readonly IGoodCategoryRepository _repository;

        public GoodCategoryService(IGoodCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<GoodCategory> AddGoodCategoryAscyn(string name)
        {
            GoodCategory goodCategory = new GoodCategory() { Name = name };
            await _repository.AddEntityAsync(goodCategory);
            await _repository.UnitOfWork.SaveChangeAsync();
            return goodCategory;
        }

        public async Task<bool> DeleteGoodCategoryAsync(int id)
        {
            var goodCategory = await _repository.GetEntityAsNoTrackingAsync(c => c.Id == id,"Goods");
            if (goodCategory is null) throw new CustomException("该分类不存在");
            if(goodCategory.Goods.Count() > 0) throw new CustomException("该分类存在物品，请删除该分类下的所有物品");
            _repository.DeleteEntity(goodCategory);
            return await _repository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<GoodCategory>> GetGoodCategoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<bool> UpdateGoodCategoryAsync(int id, string name)
        {
            var goodCategory = await _repository.GetEntityAsync(c => c.Id == id);
            if (goodCategory is null) throw new CustomException("该分类不存在");
            goodCategory.Name = name;
            _repository.UpdateEntity(goodCategory);
            return await _repository.UnitOfWork.SaveChangeAsync();
        }
    }
}
