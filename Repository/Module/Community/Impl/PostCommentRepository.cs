using Dapper;
using Entities.Base;
using Entities.Community;
using Entities.System;
using Microsoft.Data.SqlClient;
using Models.Community;
using Repositories.Base;
using Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Module.Community.Impl
{
    public class PostCommentRepository : BaseRepository<PostComment>, IPostCommentRepository
    {
        public PostCommentRepository(HanFishDbContext hanFishDbContext) : base(hanFishDbContext)
        {
        }

        public async Task<IEnumerable<PostCommentModel>> GetPostCommentByPostIdAsync(int postId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var sql = @"Select c.Id,c.PostId,c.Comment,c.ImgUrl,c.PId,c.CreateTime,c.UserId,u.Name as 'UserName',u.AvatarUrl FROM [PostComment] as c INNER JOIN [User] as u on c.UserId = u.Id WHERE PostId = @postId";
                var result = await conn.QueryAsync<PostCommentModel>(sql, new { postId });
                return result.ToList();
            }
        }
    }
}
