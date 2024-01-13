using AutoMapper;
using Entities.Goods;
using Entities.System;
using Models.Goods;
using Models.System;

namespace HanFishApis.Infrastructure.AutoMaper.Profiles
{
    public class GoodModelProfile : Profile
    {
        public GoodModelProfile()
        {
            CreateMap<Good, GoodModel>().ForMember(g => g.Status, opt => opt.MapFrom(g => g.Status.ToString()));
            CreateMap<AddGoodModel, Good>();
            CreateMap<GoodCategory, GoodCategoryModel>();
        }
    }
}
    