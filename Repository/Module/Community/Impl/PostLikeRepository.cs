using Entities.Community;
using Repositories.Base;
using Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Module.Community.Impl
{
    public class PostLikeRepository : BaseRepository<PostLike>, IPostLikeRepository
    {
        public PostLikeRepository(HanFishDbContext hanFishDbContext) : base(hanFishDbContext)
        {
        }
    }
}
