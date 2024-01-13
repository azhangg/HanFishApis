using Models.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Community
{
    public interface IBannerService
    {
        Task<IEnumerable<BannerModel>> GetAppliedBannerListAsync();

        Task<IEnumerable<BannerModel>> GetAllBannerListAsync(bool isApplied = false);

        Task<BannerModel> AddBannerAsync(AddBannerModel model);

        Task<bool> UpdateBannerAsync(UpdateBannerModel model);

        Task<bool> DeleteBannerAsync(int id);
    }
}
