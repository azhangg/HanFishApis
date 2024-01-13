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
    internal class BannerRepository : BaseRepository<Banner>, IBannerRepository
    {
        public BannerRepository(HanFishDbContext hanFishDbContext) : base(hanFishDbContext)
        {
        }
    }
}
