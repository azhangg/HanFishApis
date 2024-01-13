using HanFishApis.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Community;
using Services.Community;

namespace HanFishApis.Controllers.Community
{
    public class BannerController : BaseController
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAppliedBannerListAsync()
        { 
            var result = await  _bannerService.GetAppliedBannerListAsync();
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBannerListAsync(bool isApplied = false)
        {
            var result = await _bannerService.GetAllBannerListAsync(isApplied);
            return JsonSuccess(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddBannerListAsync(AddBannerModel model)
        {
            var result = await _bannerService.AddBannerAsync(model);
            if (result is null) return JsonFail("添加失败");
            return JsonSuccess(result,"添加成功");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBannerListAsync(UpdateBannerModel model)
        {
            var result = await _bannerService.UpdateBannerAsync(model);
            if (!result) return JsonFail("修改失败");
            return JsonSuccess("修改成功");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBannerListAsync(int id)
        {
            var result = await _bannerService.DeleteBannerAsync(id);
            if (!result) return JsonFail("删除失败");
            return JsonSuccess("删除成功");
        }
    }
}
