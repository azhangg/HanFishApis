using Models.Community;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;

namespace Services.Community
{
    public interface IPostService
    {
        Task<PaginationModel<PostModel>> GetPostListToPaginationAsync(int page,int count,string search = "",PostStatus status = PostStatus.已发布);

        Task<PostModel> AddPostAsync(AddPostModel model);

        Task<IEnumerable<PostModel>> GetPostsByUserIdAsync(int userId);

        Task<IEnumerable<PostModel>> GetPostsUserLikedAsync(int userId);

        Task<IEnumerable<PostModel>> GetPostsUserCollectedAsync(int userId);

        Task<IEnumerable<CommentPostModel>> GetPostsUserCommentedAsync(int userId);

        Task<bool> UpdatePostStatusAsync(int id,int status);

        Task<bool> DeletePostAsync(int id);

        Task<PostModel> GetPostByIdAsync(int id);

        Task<UserCommunityDataModel> GetUserCommunityDataAsync(int userId);
    }
}
