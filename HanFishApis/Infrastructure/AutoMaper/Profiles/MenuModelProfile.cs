using AutoMapper;
using Entities.System;
using Models.System;
using Utils;

namespace HanFishApis.Infrastructure.AutoMaper.Profiles
{
    public class MenuModelProfile : Profile
    {
        public MenuModelProfile()
        {
            CreateMap<Menu, MenuModel>()
                .ForMember(m => m.CreateTime,opt => opt.MapFrom(m => DateTimeHelper.TimeString(m.CreateTime)))
                .ReverseMap();
            CreateMap<AddMenuModel, Menu>();
            CreateMap<UpdateMenuModel, Menu>();
        }
    }
}
