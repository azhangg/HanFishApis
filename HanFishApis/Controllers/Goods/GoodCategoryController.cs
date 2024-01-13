using HanFishApis.Controllers.Base;
using HanFishApis.Infrastructure.OpenApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Goods;
using System.Xml.Linq;

namespace HanFishApis.Controllers.Goods
{
    public class GoodCategoryController : BaseController
    {
        private readonly IGoodCategoryService _goodCategoryService;

        public GoodCategoryController(IGoodCategoryService goodCategoryService)
        {
            _goodCategoryService = goodCategoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetGoodCategoriesAsync()
        {
            var result = await _goodCategoryService.GetGoodCategoriesAsync();
            return JsonSuccess(result);
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> AddGoodCategoryAsync(string name)
        {
            var result = await _goodCategoryService.AddGoodCategoryAscyn(name);
            if (result is null) return JsonFail("添加失败");
            return JsonSuccess(result, "添加成功");
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> UpdateGoodCategoryAsync(int id, string name)
        {
            var result = await _goodCategoryService.UpdateGoodCategoryAsync(id,name);
            if (!result) return JsonFail("修改失败");
            return JsonSuccess(result, "修改成功");
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> DeleteGoodCategoryAsync(int id)
        {
            var result = await _goodCategoryService.DeleteGoodCategoryAsync(id);
            if (!result) return JsonFail("删除失败");
            return JsonSuccess(result, "删除成功");
        }
    }
}
