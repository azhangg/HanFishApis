using AutoMapper;
using Entities.Account;
using Entities.Goods;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Account;
using Models.Goods;
using Repositories.Module.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;
using Utils.Enums;

namespace Services.Goods.Impl
{
    internal class GoodService : IGoodService
    {
        private readonly IGoodRepository _goodsRepository;
        private readonly IMapper _mapper;

        public GoodService(IGoodRepository goodsRepository, IMapper mapper)
        {
            _goodsRepository = goodsRepository;
            _mapper = mapper;
        }

        public async Task<Good> AddGoodAsync(AddGoodModel model)
        {
            Good good = _mapper.Map<Good>(model);
            good.Status = GoodStatus.未交易;
            good.CreateTime = DateTime.Now;
            await _goodsRepository.AddEntityAsync(good);
            await _goodsRepository.UnitOfWork.SaveChangeAsync();
            return good;
        }

        public async Task<bool> DeleteGoodAsync(int id)
        {
            Good good = await _goodsRepository.GetEntityAsync(g => g.Id == id);
            if (good is null) throw new CustomException("该物品不存在");
            _goodsRepository.DeleteEntity(good);
            return await _goodsRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<GoodModel> GetGoodByIdAsync(int id)
        {
            var result = await _goodsRepository.GetEntityAsNoTrackingAsync(g => g.Id == id,"User");
            return _mapper.Map<GoodModel>(result);
        }

        public async Task<PaginationModel<GoodModel>> GetGoodsToPaginationAsync(int page, int count, string searchText = "", int categoryId = 0, 
            int status = 0)
        {
            (var goods, var total) = await _goodsRepository.GetListToPaginationAsync(g => g.CreateTime, true,
                g => (searchText.IsNullOrEmpty() ? true: g.Description.Contains(searchText)) && 
                (categoryId != default ? g.CategoryId == categoryId : true) && 
                (status != default ?  g.Status == (GoodStatus)status : true),
                page, count, "User", "Category");
            return new PaginationModel<GoodModel>()
            {
                Page = page,
                PageCount = count,
                Total = total,
                Data = goods.Select(_mapper.Map<GoodModel>).ToList()
            };
        }

        public async Task<IEnumerable<GoodModel>> GetUsersGoodByUserIdAsync(int userId)
        {
            var goods = await _goodsRepository.GetListAsNoTrackingAsync(g => g.UserId == userId, "User", "Category");
            return goods.Select(_mapper.Map<GoodModel>).OrderByDescending(g => g.CreateTime).ToList();
        }

        public async Task<bool> UpdateGoodAsync(UpdateGoodModel model)
        {
            Good good = await _goodsRepository.GetEntityAsync(g => g.Id == model.Id);
            if (good is null) throw new CustomException("该物品不存在");
            if(model.Price != default(decimal) && model.Price is not null)
                good.Price = (decimal)model.Price;
            if (model.Status != default(int) && model.Status is not null)
                good.Status = (GoodStatus)(int)model.Status;
            _goodsRepository.UpdateEntity(good);
            return await _goodsRepository.UnitOfWork.SaveChangeAsync();
        }
    }
}
