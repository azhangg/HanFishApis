using AutoMapper;
using Entities.Community;
using Models.Community;
using Repositories.Module.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Services.Community.Impl
{
    internal class PostLikeService : IPostLikeService
    {
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostLikeService(IPostLikeRepository postLikeRepository, IPostRepository postRepository, IMapper mapper)
        {
            _postLikeRepository = postLikeRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddPostLikeAsync(int postId, int userId)
        {
            var isExist = await _postLikeRepository.GetEntityAsync(l => l.PostId == postId && l.UserId == userId) is not null;
            if (isExist) throw new CustomException("重复操作无效");
            await _postLikeRepository.AddEntityAsync(new PostLike { PostId = postId ,UserId = userId });
            return await _postLikeRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<bool> DeletePostLikeAsync(int postId, int userId)
        {
            var postLike = await _postLikeRepository.GetEntityAsync(l => l.PostId == postId && l.UserId == userId);
            if (postLike is null) throw new CustomException("该点赞不存在");
            _postLikeRepository.DeleteEntity(postLike);
            return await _postLikeRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<int>> GetUserLikePostIdsAsync(int userId)
        {
            var result = await _postLikeRepository.GetListAsNoTrackingAsync(l =>  l.UserId == userId);
            return result.Select(l => l.PostId).ToList();
        }

        public async Task<int> GetPostLikeCountAsync(int postId)
        {
            var result = await _postLikeRepository.GetListAsNoTrackingAsync(l => l.PostId == postId);
            return result.Count();
        }

        public async Task<IEnumerable<PostModel>> GetUserLikePostListAsync(int userId)
        {
            var postIds = await GetUserLikePostIdsAsync(userId);
            List<PostModel> result = new List<PostModel>();
            foreach (var postId in postIds)
            {
                var post = await _postRepository.GetEntityAsNoTrackingAsync(p => p.Id == postId);
                if (post is not null) result.Add(_mapper.Map<PostModel>(post));
            }
            return result;
        }

        public async Task<int> GetUsersPostLikesAsync(int userId)
        {
            var userPosts = await _postRepository.GetListAsNoTrackingAsync(p => p.PublisherId == userId);
            var postIds = userPosts.Select(up => up.Id);
            var likes = await _postLikeRepository.GetListAsNoTrackingAsync(pl => postIds.Contains(pl.PostId));
            return likes.Count();
        }
    }
}
