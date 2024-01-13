using AutoMapper;
using Entities.Community;
using Models.Community;
using Repositories.Module.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Services.Community.Impl
{
    internal class BannerService : IBannerService
    {
        private readonly IBannerRepository  _bannerRepository;
        private readonly IMapper _mapper;

        public BannerService(IBannerRepository bannerRepository, IMapper mapper)
        {
            _bannerRepository = bannerRepository;
            _mapper = mapper;
        }

        public async Task<BannerModel> AddBannerAsync(AddBannerModel model)
        {
            var banner = _mapper.Map<Banner>(model);
            banner.CreateTime = DateTime.Now;
            await _bannerRepository.AddEntityAsync(banner);
            await _bannerRepository.UnitOfWork.SaveChangeAsync();
            return _mapper.Map<BannerModel>(banner);
        }

        public async Task<bool> DeleteBannerAsync(int id)
        {
            var banner = await _bannerRepository.GetEntityAsync(b => b.Id == id);
            if (banner is null) throw new CustomException("该横幅不存在");
            _bannerRepository.DeleteEntity(banner);
            return await _bannerRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<BannerModel>> GetAllBannerListAsync(bool isApplied = false)
        {
            var bannerList = await _bannerRepository.GetListAsNoTrackingAsync(b => isApplied ? b.Apply == true : true);
            return bannerList.Select(_mapper.Map<BannerModel>).ToList();
        }

        public async Task<IEnumerable<BannerModel>> GetAppliedBannerListAsync()
        {
            var bannerList = await _bannerRepository.GetListAsNoTrackingAsync(b => b.Apply == true );
            return bannerList.Select(_mapper.Map<BannerModel>).OrderByDescending(b => b.Order).ToList();
        }

        public async Task<bool> UpdateBannerAsync(UpdateBannerModel model)
        {
            var banner = await _bannerRepository.GetEntityAsync(b => b.Id == model.Id);
            if (banner is null) throw new CustomException("该横幅不存在");
            banner.ImgUrl = model.ImgUrl;
            banner.Apply = model.Apply;
            banner.Order = model.Order;
            _bannerRepository.UpdateEntity(banner);
            return await _bannerRepository.UnitOfWork.SaveChangeAsync();
        }
    }
}
