using Entities.Community;
using Models.Community;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Module.Community
{
    public interface IPostCommentRepository : IRepository<PostComment>
    {
        Task<IEnumerable<PostCommentModel>> GetPostCommentByPostIdAsync(int postId);
    }
}
