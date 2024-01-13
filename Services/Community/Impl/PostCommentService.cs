using AutoMapper;
using Entities.Community;
using Entities.System;
using Models.Community;
using Models.System;
using Repositories.Module.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Services.Community.Impl
{
    internal class PostCommentService : IPostCommentService
    {
        private readonly IPostCommentRepository _postCommentRepository;
        private readonly IMapper _mapper;
        public PostCommentService(IPostCommentRepository postCommentRepository, IMapper mapper)
        {
            _postCommentRepository = postCommentRepository;
            _mapper = mapper;
        }

        public async Task<PostCommentModel> AddPostCommentAsync(AddPostCommentModel model)
        {
            var postComment = _mapper.Map<PostComment>(model);
            postComment.CreateTime = DateTime.Now;
            await _postCommentRepository.AddEntityAsync(postComment);
            await _postCommentRepository.UnitOfWork.SaveChangeAsync();
            return _mapper.Map<PostCommentModel>(postComment);
        }

        public async Task<bool> DeletePostCommentAsync(int id)
        {
            var postComment = await _postCommentRepository.GetEntityAsync(c => c.Id == id);
            if (postComment is null) throw new CustomException("该评论不存在");
            _postCommentRepository.DeleteEntity(postComment);
            return await _postCommentRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<int> GetCommentCountAsync(int postId)
        {
            var result = await _postCommentRepository.GetListAsNoTrackingAsync(c => c.PostId == postId);
            return result.Count();
        }

        public async Task<IEnumerable<PostCommentResponseModel>> GetCommentListByPostIdAsync(int postId)
        {
            var comments = await _postCommentRepository.GetPostCommentByPostIdAsync(postId);
            List<PostCommentResponseModel> result = comments.Select(_mapper.Map<PostCommentResponseModel>).ToList();
            return GetCommentTree(result);
        }

        #region 评论树遍历
        public List<PostCommentResponseModel> GetCommentTree(List<PostCommentResponseModel> comments, int pId = 0)
        {
            var current = comments.Where(c => c.PId == pId).OrderByDescending(c => c.CreateTime).ToList();
            if (current.Count == 0) return new List<PostCommentResponseModel>();
            foreach (var comment in current)
            {
                comment.Children = GetCommentTree(comments, comment.Id);
            }
            return current;
        }
        #endregion
    }
}
