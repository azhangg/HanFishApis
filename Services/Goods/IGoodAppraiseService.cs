using Models.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Goods
{
    public interface IGoodAppraiseService
    {
        Task<IEnumerable<GoodAppraiseResponseModel>> GetGoodAppraisesAsync(int userId);

        Task<GoodAppraiseResponseModel> AddGoodAppraiseAsync(AddGoodAppraiseModel model);
    }
}
