using Models.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Community
{
    public interface IPostLikeService
    {
        Task<int> GetPostLikeCountAsync(int postId);

        Task<bool> AddPostLikeAsync(int postId, int userId);

        Task<bool> DeletePostLikeAsync(int postId, int userId);

        Task<IEnumerable<int>> GetUserLikePostIdsAsync(int userId);

        Task<IEnumerable<PostModel>> GetUserLikePostListAsync(int userId);

        Task<int> GetUsersPostLikesAsync(int userId);
    }
}
