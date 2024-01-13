using HanFishApis.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Goods;
using Services.Goods;
using System.Security.Claims;

namespace HanFishApis.Controllers.Goods
{
    public class GoodAppraiseController : BaseController
    {
        private readonly IGoodAppraiseService _goodAppraiseService;

        public GoodAppraiseController(IGoodAppraiseService goodAppraiseService)
        {
            _goodAppraiseService = goodAppraiseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGoodAppraisesAsync(int userId)
        {
            var result = await _goodAppraiseService.GetGoodAppraisesAsync(userId);
            return JsonSuccess(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddGoodAppraiseAsync(AddGoodAppraiseModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            model.UserId = int.Parse(userId);
            var result = await _goodAppraiseService.AddGoodAppraiseAsync(model);
            if (result is null) return JsonFail("评论失败");
            return JsonSuccess(result,"评论成功");
        }
    }
}
