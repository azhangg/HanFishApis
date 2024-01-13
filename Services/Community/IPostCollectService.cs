using Models.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Community
{
    public interface IPostCollectService
    {
        Task<int> GetPostCollectCountAsync(int postId);

        Task<bool> AddPostCollectAsync(int postId,int userId);

        Task<bool> DeletePostCollectAsync(int postId, int userId);

        Task<IEnumerable<int>> GetUserCollectPostIdsAsync(int userId);

        Task<IEnumerable<PostModel>> GetUserCollectPostListAsync(int userId);
    }
}
