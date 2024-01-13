using AutoMapper;
using Entities.Community;
using Models.Community;

namespace HanFishApis.Infrastructure.AutoMaper.Profiles
{
    public class PostCommentModelProflie : Profile
    {
        public PostCommentModelProflie()
        {
            CreateMap<PostComment, PostCommentModel>();
            CreateMap<PostComment, CommentPostModel>();
            CreateMap<PostComment, PostCommentResponseModel>();
            CreateMap<AddPostCommentModel, PostComment>();
            CreateMap<PostCommentModel, PostCommentResponseModel>();
        }
    }
}
