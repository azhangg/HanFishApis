using Entities.Goods;
using Repositories.Base;
using Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Module.Goods.Impl
{
    internal class GoodAppraiseRepository : BaseRepository<GoodAppraise>, IGoodAppraiseRepository
    {
        public GoodAppraiseRepository(HanFishDbContext hanFishDbContext) : base(hanFishDbContext)
        {
        }
    }
}
