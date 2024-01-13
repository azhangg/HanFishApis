using AutoMapper;
using Entities.Goods;
using Models.Account;
using Models.Goods;
using Repositories.Module.Account;
using Repositories.Module.Account.impl;
using Repositories.Module.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Services.Goods.Impl
{
    internal class GoodAppraiseService : IGoodAppraiseService
    {
        private readonly IGoodAppraiseRepository _goodAppraiseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGoodRepository _goodRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GoodAppraiseService(IGoodAppraiseRepository goodAppraiseRepository, IUserRepository userRepository, IGoodRepository goodRepository, IMapper mapper, IOrderRepository orderRepository)
        {
            _goodAppraiseRepository = goodAppraiseRepository;
            _userRepository = userRepository;
            _goodRepository = goodRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<GoodAppraiseResponseModel> AddGoodAppraiseAsync(AddGoodAppraiseModel model)
        {
            var user = await _userRepository.GetEntityAsNoTrackingAsync(u => u.Id == model.UserId);
            if (user is null) throw new CustomException("该用户不存在");
            var seller = await _userRepository.GetEntityAsNoTrackingAsync(u => u.Id == model.SellerId);
            if (seller is null) throw new CustomException("该卖家不存在");
            var good = await _goodRepository.GetEntityAsNoTrackingAsync(g => g.Id == model.GoodId);
            if (good is null) throw new CustomException("该物品不存在");
            var order = await _orderRepository.GetEntityAsync(o => o.GoodId == model.GoodId && o.Status != Utils.Enums.OrderStatus.已取消);
            if(order is not null)
            {
                order.Status = Utils.Enums.OrderStatus.已完成;
                _orderRepository.UpdateEntity(order);
            }
            var goodAppraise = _mapper.Map<GoodAppraise>(model);
            goodAppraise.CreateTime = DateTime.Now;
            await _goodAppraiseRepository.AddEntityAsync(goodAppraise);
            await _goodAppraiseRepository.UnitOfWork.SaveChangeAsync();
            var result = _mapper.Map<GoodAppraiseResponseModel>(goodAppraise);
            result.User = _mapper.Map<UserModel>(await _userRepository.GetEntityAsNoTrackingAsync(u => u.Id == result.UserId));
            result.Good = _mapper.Map<GoodModel>(await _goodRepository.GetEntityAsNoTrackingAsync(g => g.Id == result.GoodId, "User"));
            return result;
        }

        public async Task<IEnumerable<GoodAppraiseResponseModel>> GetGoodAppraisesAsync(int userId)
        {
            var goodAppraises = await _goodAppraiseRepository.GetEntitiesAsync(ga => ga.SellerId == userId);
            var result = goodAppraises.OrderByDescending(ga => ga.CreateTime).Select(_mapper.Map<GoodAppraiseResponseModel>).ToList();
            foreach (var item in result)
            {
                item.User = _mapper.Map<UserModel>(await _userRepository.GetEntityAsNoTrackingAsync(u => u.Id == item.UserId));
                item.Good = _mapper.Map<GoodModel>(await _goodRepository.GetEntityAsNoTrackingAsync(g => g.Id == item.GoodId,"User"));
            }
            return result;
        }
    }
}
