using AutoMapper;
using Entities.Community;
using Models.Community;

namespace HanFishApis.Infrastructure.AutoMaper.Profiles
{
    public class PostModelProfile : Profile
    {
        public PostModelProfile()
        {
            CreateMap<Post, PostModel>().ForMember(p => p.Status, opt => opt.MapFrom(p => p.Status.ToString()));
            CreateMap<AddPostModel, Post>();
        }
    }
}
