using AutoMapper;
using Entities.Account;
using Entities.Goods;
using Entities.Message;
using Microsoft.AspNetCore.SignalR;
using Models;
using Models.Account;
using Models.Goods;
using Models.Message;
using Repositories.Module.Account;
using Repositories.Module.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;
using Utils.Enums;
using Utils.Hubs;

namespace Services.Goods.Impl
{
    internal class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGoodRepository _goodRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IMapper mapper, IGoodRepository goodRepository, IAddressRepository addressRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _goodRepository = goodRepository;
            _addressRepository = addressRepository;
        }

        public async Task<OrderModel> AddOrderAsync(AddOrderModel model)
        {
            var good = await _goodRepository.GetEntityAsync(g => g.Id == model.GoodId);
            if (good is null || good.Status != GoodStatus.未交易) throw new CustomException("该物品不存在");
            var user = await _userRepository.GetEntityAsync(u => u.Id == model.UserId);
            if (user is null) throw new CustomException("该用户不存在");
            var address = await _addressRepository.GetEntityAsync(a => a.Id == model.AddressId);
            if (address is null) throw new CustomException("该地址不存在");
            var order = _mapper.Map<Order>(model);
            order.Code = $"HXY{DateTime.Now.ToString("yyyyMMddHHmmss")}{new Random().Next(999)}";
            order.Status = OrderStatus.待付款;
            order.CreateTime = DateTime.Now;
            good.Status = GoodStatus.已交易;
            _goodRepository.UpdateEntity(good);
            await _orderRepository.AddEntityAsync(order);
            await _orderRepository.UnitOfWork.SaveChangeAsync();
            return _mapper.Map<OrderModel>(order); ;
        }

        public async Task<bool> CancelOrderAsync(int id)
        {
            var order = await _orderRepository.GetEntityAsync(o => o.Id == id);
            if (order is null) throw new CustomException("该订单不存在");
            var good = await _goodRepository.GetEntityAsync(g => g.Id == order.GoodId);
            if (good is null) throw new CustomException("该物品不存在");
            good.Status = GoodStatus.未交易;
            order.Status = OrderStatus.已取消;
            _orderRepository.UpdateEntity(order);
            _goodRepository.UpdateEntity(good);
            return await _orderRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetEntityAsync(o => o.Id == id);
            if (order is null) throw new CustomException("该订单不存在");
            var good = await _goodRepository.GetEntityAsync(g => g.Id == order.GoodId);
            if (good is not null)
            {
                if(order.Status != OrderStatus.已完成)
                {
                    good.Status = GoodStatus.未交易;
                    _goodRepository.UpdateEntity(good);
                }
            }
            _orderRepository.DeleteEntity(order);
            return await _orderRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<OrderModel> getOrderByOrderIdAsync(int id)
        {
            var order = await _orderRepository.GetEntityAsNoTrackingAsync(o => o.Id == id, "Good", "User", "Address");
            if (order is null) throw new CustomException("该订单不存在");
            return _mapper.Map<OrderModel>(order);
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetListAsNoTrackingAsync(o => o.UserId == userId || o.Good.UserId == userId, "Good", "User", "Address");
            return orders.Select(_mapper.Map<OrderModel>).OrderByDescending(o => o.CreateTime).ToList();
        }

        public async Task<PaginationModel<OrderModel>> GetOrderToPaginationAsync(int page, int count, int userId = 0, int status = 0, string search = "")
        {
            var (orders,total) = await _orderRepository.GetListToPaginationAsync(o => o.CreateTime, true, o => (userId != 0 ? o.UserId == userId : true) &&
            (status != 0 ? o.Status == (OrderStatus)status : true) && o.Code.Contains(search),
                page, count, "Good", "User", "Address");
            return new PaginationModel<OrderModel>
            {
                Total = total,
                Data = orders.Select(_mapper.Map<OrderModel>).ToList()
        };
        }

        public async Task<bool> ModifyOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await _orderRepository.GetEntityAsync(o => o.Id == id);
            if (order is null) throw new CustomException("该订单不存在");
            order.Status = status;
            _orderRepository.UpdateEntity(order);
            return await _orderRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<bool> OrderDeliveryAsync(int id)
        {
            var order = await _orderRepository.GetEntityAsync(o => o.Id == id);
            if (order is null) throw new CustomException("该订单不存在");
            if(order.Status != OrderStatus.待发货) throw new CustomException("订单状态必须为待发货");
            order.Status = OrderStatus.待收货;
            _orderRepository.UpdateEntity(order);
            return await _orderRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<bool> PayOrderAsync(int id)
        {
            var order = await _orderRepository.GetEntityAsync(o => o.Id == id);
            if (order is null) throw new CustomException("该订单不存在");
            order.Status = OrderStatus.待发货;
            _orderRepository.UpdateEntity(order);
            return await _orderRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<bool> ReceiveOrderAsync(int id)
        {
            var order = await _orderRepository.GetEntityAsync(o => o.Id == id);
            if (order is null) throw new CustomException("该订单不存在");
            if (order.Status != OrderStatus.待收货) throw new CustomException("订单状态必须为待收货");
            order.Status = OrderStatus.待评价;
            _orderRepository.UpdateEntity(order);
            return await _orderRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateOrderAsync(UpdateOrderModel model)
        {
            var order = await _orderRepository.GetEntityAsync(o => o.Id == model.Id);
            if (order is null) throw new CustomException("该订单不存在");
            order.AddressId = model.AddressId;
            _orderRepository.UpdateEntity(order);
            return await _orderRepository.UnitOfWork.SaveChangeAsync();
        }
    }
}
