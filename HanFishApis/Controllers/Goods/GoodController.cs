using HanFishApis.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Goods;
using Services.Goods;
using System.Security.Claims;

namespace HanFishApis.Controllers.Goods
{
    public class GoodController : BaseController
    {
        private readonly IGoodService _goodService;

        public GoodController(IGoodService goodService)
        {
            _goodService = goodService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetGoodToPaginationAsync(int page,int count, string search = "", int categoryId = 0,int status = 0) 
        {
            var result = await _goodService.GetGoodsToPaginationAsync(page, count, search, categoryId, status);
            return JsonSuccess(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetGoodByIdAsync(int id)
        {
            var result = await _goodService.GetGoodByIdAsync(id);
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersGoodAsync(int userId)
        {
            var result = await _goodService.GetUsersGoodByUserIdAsync(userId);
            return JsonSuccess(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddGoodAsync(AddGoodModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null)
                return Unauthorized();
            model.UserId = int.Parse(userId);
            var result = await _goodService.AddGoodAsync(model);
            if (result is null) return JsonFail("添加失败");
            return JsonSuccess(result,"添加成功");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGoodAsync(UpdateGoodModel model)
        {
            var result = await _goodService.UpdateGoodAsync(model);
            if (!result) return JsonFail("修改失败");
            return JsonSuccess(result, "修改成功");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGoodAsync(DeleteModel model)
        {
            var result = await _goodService.DeleteGoodAsync(model.Id);
            if (!result) return JsonFail("删除失败");
            return JsonSuccess(result, "删除成功");
        }
    }
}
