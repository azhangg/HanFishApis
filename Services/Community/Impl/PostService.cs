using AutoMapper;
using Entities.Community;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Account;
using Models.Community;
using Repositories.Module.Account;
using Repositories.Module.Community;
using Repositories.Module.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;
using Utils.Enums;

namespace Services.Community.Impl
{
    internal class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostCollectRepository _postCollectRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IPostCommentRepository _postCommentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGoodAppraiseRepository _goodAppraiseRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IMapper mapper, IPostCollectRepository postCollectRepository, IPostLikeRepository postLikeRepository, IPostCommentRepository postCommentRepository, IUserRepository userRepository, IGoodAppraiseRepository goodAppraiseRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _postCollectRepository = postCollectRepository;
            _postLikeRepository = postLikeRepository;
            _postCommentRepository = postCommentRepository;
            _userRepository = userRepository;
            _goodAppraiseRepository = goodAppraiseRepository;
        }

        public async Task<PostModel> AddPostAsync(AddPostModel model)
        {
            Post post = _mapper.Map<Post>(model);
            post.Status = PostStatus.已发布;
            post.CreateTime = DateTime.Now;
            await _postRepository.AddEntityAsync(post);
            await _postRepository.UnitOfWork.SaveChangeAsync();
            return _mapper.Map<PostModel>(post);
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _postRepository.GetEntityAsNoTrackingAsync(p => p.Id == id);
            if (post is null) throw new CustomException("该帖子不存在");
            _postRepository.DeleteEntity(post);
            return await _postRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<PostModel> GetPostByIdAsync(int id)
        {
            var post = await _postRepository.GetEntityAsNoTrackingAsync(_ => _.Id == id, "Publisher");

            if (post is null) return null;

            var result = _mapper.Map<PostModel>(post);
            var collects = await _postCollectRepository.GetListAsNoTrackingAsync(pc => pc.PostId == result.Id);
            var likes = await _postLikeRepository.GetListAsNoTrackingAsync(pl => pl.PostId == result.Id);
            var comments = await _postCommentRepository.GetListAsNoTrackingAsync(pm => pm.PostId == result.Id);
            result.Collects = collects.Count();
            result.Likes = likes.Count();
            result.Comments = comments.Count();

            return result;
        }

        public async Task<PaginationModel<PostModel>> GetPostListToPaginationAsync(int page, int count, string search="", PostStatus status = PostStatus.已发布)
        {
            var (posts,total) = search.IsNullOrEmpty()? await _postRepository.GetListToPaginationAsync(p => p.CreateTime, true,
                p =>  p.Status == status, page, count, "Publisher") :
                await _postRepository.GetListToPaginationAsync(p=>p.CreateTime,true,p=>p.Text.Contains(search)&&
                p.Status == status, page,count, "Publisher");

            var result = new List<PostModel>();
            foreach (var post in posts)
            {
                var postmodel = _mapper.Map<PostModel>(post);
                var collects = await _postCollectRepository.GetListAsNoTrackingAsync(pc => pc.PostId == postmodel.Id);
                var likes = await _postLikeRepository.GetListAsNoTrackingAsync(pl => pl.PostId == postmodel.Id);
                var comments = await _postCommentRepository.GetListAsNoTrackingAsync(pm => pm.PostId == postmodel.Id);
                postmodel.Collects = collects.Count();
                postmodel.Likes = likes.Count();
                postmodel.Comments = comments.Count();
                result.Add(postmodel);
            }
            return new PaginationModel<PostModel> 
            {
                Total = total,
                Page = page,
                PageCount = count,
                Data = result
            };
        }

        public async Task<IEnumerable<PostModel>> GetPostsByUserIdAsync(int userId)
        {
            var posts = await _postRepository.GetListAsNoTrackingAsync(p => p.PublisherId == userId && p.Status == PostStatus.已发布, "Publisher");
            return posts.Select(_mapper.Map<PostModel>).OrderByDescending(p => p.CreateTime).ToList();
        }

        public async Task<IEnumerable<PostModel>> GetPostsUserCollectedAsync(int userId)
        {
            var collects = await _postCollectRepository.GetListAsNoTrackingAsync(pc => pc.UserId == userId);
            var collectIds = collects.OrderByDescending(pc => pc.Id).Select(pc => pc.PostId).ToList();
            var result = new List<PostModel>();
            foreach (var postId in collectIds)
            {
                var post = await _postRepository.GetEntityAsNoTrackingAsync(p => p.Id == postId && p.Status == PostStatus.已发布, "Publisher");
                if (post is not null) result.Add(_mapper.Map<PostModel>(post));
            }
            return result;
        }

        public async Task<IEnumerable<CommentPostModel>> GetPostsUserCommentedAsync(int userId)
        {
            var comments = await _postCommentRepository.GetListAsNoTrackingAsync(pt => pt.UserId == userId);
            var result = comments.Select(_mapper.Map<CommentPostModel>).OrderByDescending(pt => pt.CreateTime).ToList();
            foreach (var item in result)
            {
                item.Post = _mapper.Map<PostModel>(await _postRepository.GetEntityAsNoTrackingAsync(p => p.Id == item.PostId, "Publisher"));
                if(item.PId != 0)
                {
                    item.Puser = _mapper.Map<UserModel>(await _userRepository.GetEntityAsNoTrackingAsync(u => u.Id == item.UserId));
                }
            }
            return result.Where(r => r.Post is not null).Where(r => r.Post.Status == PostStatus.已发布.ToString());
        }

        public async Task<IEnumerable<PostModel>> GetPostsUserLikedAsync(int userId)
        {
            var likes = await _postLikeRepository.GetListAsNoTrackingAsync(pc => pc.UserId == userId);
            var likeIds = likes.OrderByDescending(pl => pl.Id).Select(pc => pc.PostId).ToList();
            var result = new List<PostModel>();
            foreach (var postId in likeIds)
            {
                var post = await _postRepository.GetEntityAsNoTrackingAsync(p => p.Id == postId && p.Status == PostStatus.已发布, "Publisher");
                if (post is not null) result.Add(_mapper.Map<PostModel>(post));
            }
            return result;
        }

        public async Task<UserCommunityDataModel> GetUserCommunityDataAsync(int userId)
        {
            var appraises = await _goodAppraiseRepository.GetListAsNoTrackingAsync(a => a.SellerId == userId);
            var posts = await _postRepository.GetListAsNoTrackingAsync(p => p.PublisherId == userId && p.Status == PostStatus.已发布);
            var allPost = await _postRepository.GetListAsNoTrackingAsync(p => p.Status == PostStatus.已发布);
            var allPostIds = allPost.Select(p => p.Id);
            var postIds = posts.Select(p => p.Id);
            var bePraises = await _postLikeRepository.GetListAsNoTrackingAsync(l => postIds.Contains(l.PostId));
            var collects = await _postCollectRepository.GetListAsNoTrackingAsync(c => c.UserId == userId && allPostIds.Contains(c.PostId));
            return new UserCommunityDataModel
            {
                AppraiseNum = appraises.Count(),
                PostNum = posts.Count(),
                BePraiseNum = bePraises.Count(),
                CollectNum = collects.Count(),
            };
        }

        public async Task<bool> UpdatePostStatusAsync(int id, int status)
        {
            var post = await _postRepository.GetEntityAsNoTrackingAsync(p => p.Id == id);
            if (post is null) throw new CustomException("该帖子不存在");
            if (!Enum.IsDefined(typeof(PostStatus),status)) throw new CustomException("无效状态");
            post.Status = (PostStatus)status;
            _postRepository.UpdateEntity(post);
            return await _postRepository.UnitOfWork.SaveChangeAsync();
        }
    }
}
