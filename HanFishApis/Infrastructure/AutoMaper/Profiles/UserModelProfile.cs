using AutoMapper;
using Entities.Account;
using Models.Account;

namespace HanFishApis.Infrastructure.AutoMaper.Profiles
{
    public class UserModelProfile : Profile
    {
        public UserModelProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap<RegisterUserModel, UserModel>();
            CreateMap<UpdateUserModel, UserModel>();
        }
    }
}
