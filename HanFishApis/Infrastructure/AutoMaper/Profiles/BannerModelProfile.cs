using AutoMapper;
using Entities.Community;
using Models.Community;

namespace HanFishApis.Infrastructure.AutoMaper.Profiles
{
    public class BannerModelProfile : Profile
    {
        public BannerModelProfile()
        {
            CreateMap<Banner, BannerModel>();
            CreateMap<AddBannerModel, Banner>();
        }
    }
}
