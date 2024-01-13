using Models.System;
using Repositories.Module.Account;
using Repositories.Module.Community;
using Repositories.Module.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;

namespace Services.Sysyem.Impl
{
    internal class DataAnalysisService : IDataAnalysisService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IGoodRepository _goodRepository;
        private readonly IPostRepository _postRepository;

        public DataAnalysisService(IUserRepository userRepository, IOrderRepository orderRepository, IGoodRepository goodRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _goodRepository = goodRepository;
            _postRepository = postRepository;
        }

        public async Task<DataAnalysisModel> GetDataAnalysisAsync()
        {
            DateTime today = DateTime.Now.Date;
            var todayDeals = await _orderRepository.GetListAsNoTrackingAsync(o => o.Status == OrderStatus.已完成 && o.CreateTime >= today);
            var todayDealGoodIds = todayDeals.Select(o => o.GoodId);
            var todayDealGoods = await _goodRepository.GetListAsNoTrackingAsync(g => g.Status == GoodStatus.已交易 && todayDealGoodIds.Contains(g.Id));
            var todayUsers = await _userRepository.GetListAsNoTrackingAsync(u => u.CreateTime >= today);
            var todayPosts = await _postRepository.GetListAsNoTrackingAsync(p => p.CreateTime >= today);
            var todayGoods = await _goodRepository.GetListAsNoTrackingAsync(g => g.CreateTime >= today);

            var totalDeals = await _orderRepository.GetListAsNoTrackingAsync(o => o.Status == OrderStatus.已完成);
            var totalDealGoodIds = totalDeals.Select(o => o.GoodId);
            var totalDealGoods = await _goodRepository.GetListAsNoTrackingAsync(g => g.Status == GoodStatus.已交易 && totalDealGoodIds.Contains(g.Id));
            var totalUsers = await _userRepository.GetListAsNoTrackingAsync(u => true);
            var totalPosts = await _postRepository.GetListAsNoTrackingAsync(p => true);
            var totalGoods = await _goodRepository.GetListAsNoTrackingAsync(g => true);
            return new DataAnalysisModel
            {
                TodayDeals = todayDeals.Count(),
                TodayTransactionAmount = todayDealGoods.Sum(dg => dg.Price),
                TodayRegisterNum = todayUsers.Count(),
                TodayPostPublishNum = todayPosts.Count(),
                TodayGoodPublishNum = todayGoods.Count(),
                TotalDeals = totalDeals.Count(),
                TotalTransactions = totalDealGoods.Sum(dg => dg.Price),
                TotalRegisterNum = totalUsers.Count(),
                TotalPostPublishNum = totalPosts.Count(),
                TotalGoodPublishNum = totalGoods.Count()
            };
        }
    }
}
