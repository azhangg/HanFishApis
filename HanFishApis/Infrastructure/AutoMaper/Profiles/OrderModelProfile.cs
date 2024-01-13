using AutoMapper;
using Entities.Community;
using Entities.Goods;
using Models.Community;
using Models.Goods;

namespace HanFishApis.Infrastructure.AutoMaper.Profiles
{
    public class OrderModelProfile : Profile
    {
        public OrderModelProfile()
        {
            CreateMap<Order, OrderModel>();
            CreateMap<AddOrderModel, Order>();
        }
    }
}
