using Models.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Community
{
    public interface IPostCommentService
    {
        Task<IEnumerable<PostCommentResponseModel>> GetCommentListByPostIdAsync(int postId);

        Task<int> GetCommentCountAsync(int postId);

        Task<PostCommentModel> AddPostCommentAsync(AddPostCommentModel model);

        Task<bool> DeletePostCommentAsync(int id);
    }
}
