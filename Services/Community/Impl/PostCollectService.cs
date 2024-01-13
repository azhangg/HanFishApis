using AutoMapper;
using Entities.Community;
using Models.Community;
using Repositories.Module.Community;
using Repositories.Module.Community.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Services.Community.Impl
{
    internal class PostCollectService : IPostCollectService
    {
        private readonly IPostCollectRepository _postCollectRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostCollectService(IPostCollectRepository postCollectRepository, IPostRepository postRepository, IMapper mapper)
        {
            _postCollectRepository = postCollectRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddPostCollectAsync(int postId, int userId)
        {
            var isExist = await _postCollectRepository.GetEntityAsync(l => l.PostId == postId && l.UserId == userId) is not null;
            if (isExist) throw new CustomException("重复操作无效");
            await _postCollectRepository.AddEntityAsync(new PostCollect { PostId = postId, UserId = userId });
            return await _postCollectRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<bool> DeletePostCollectAsync(int postId, int userId)
        {
            var postLike = await _postCollectRepository.GetEntityAsync(l => l.PostId == postId && l.UserId == userId);
            if (postLike is null) throw new CustomException("该收藏不存在");
            _postCollectRepository.DeleteEntity(postLike);
            return await _postCollectRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<int>> GetUserCollectPostIdsAsync(int userId)
        {
            var result = await _postCollectRepository.GetListAsNoTrackingAsync(l => l.UserId == userId);
            return result.Select(c=>c.PostId).ToList();
        }

        public async Task<int> GetPostCollectCountAsync(int postId)
        {
            var result = await _postCollectRepository.GetListAsNoTrackingAsync(l => l.PostId == postId);
            return result.Count();
        }

        public async Task<IEnumerable<PostModel>> GetUserCollectPostListAsync(int userId)
        {
            var postIds = await GetUserCollectPostIdsAsync(userId);
            List<PostModel> result = new List<PostModel>();
            foreach (var postId in postIds)
            {
                var post = await _postRepository.GetEntityAsNoTrackingAsync(p => p.Id == postId);
                if (post is not null) result.Add(_mapper.Map<PostModel>(post));
            }
            return result;
        }
    }
}
