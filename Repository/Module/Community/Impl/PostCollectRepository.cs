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
    public class PostCollectRepository : BaseRepository<PostCollect>, IPostCollectRepository
    {
        public PostCollectRepository(HanFishDbContext hanFishDbContext) : base(hanFishDbContext)
        {
        }
    }
}
