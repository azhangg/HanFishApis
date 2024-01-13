using AutoMapper;
using Entities.Goods;
using Models.Goods;

namespace HanFishApis.Infrastructure.AutoMaper.Profiles
{
    public class GoodAppraiseModelProfile : Profile
    {
        public GoodAppraiseModelProfile()
        {
            CreateMap<GoodAppraise, GoodAppraiseResponseModel>();
            CreateMap<AddGoodAppraiseModel, GoodAppraise>();
        }
    }
}
