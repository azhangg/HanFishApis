using AutoMapper;
using Entities.Goods;
using Models.Goods;

namespace HanFishApis.Infrastructure.AutoMaper.Profiles
{
    public class AddressModelProfile : Profile
    {
        public AddressModelProfile()
        {
            CreateMap<Address,AddressModel>();
        }
    }
}
