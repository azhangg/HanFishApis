using HanFishApis.Controllers.Base;
using HanFishApis.Infrastructure.OpenApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.System;
using Services.Sysyem;
using System.Security.Claims;
using Utils.CustomExceptions;

namespace HanFishApis.Controllers.System
{
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> GetAllMenusAsync()
        {
            var data = await _menuService.GetAllMenuAsync();
            return JsonSuccess(data);
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> AddMenuAsync(AddMenuModel model)
        {
            var result = await _menuService.AddMenuAsync(model);
            if (result is null)
                return JsonFail("添加失败");
            return JsonSuccess(result, "添加成功");
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> UpdateMenuAsync(UpdateMenuModel model)
        {
            if (await _menuService.UpdateMenuAsync(model))
                return JsonSuccess("修改成功");
            return JsonFail("修改失败");
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> DeleteMenuAsync(int id)
        {
            if (await _menuService.DeleteMenuAsync(id))
                return JsonSuccess("删除成功");
            return JsonFail("删除失败");
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> GetUserMenus()
        {
            var userId = User.FindFirstValue("id");
            if (userId is null)
                return Unauthorized();
            var result = await _menuService.GetMenusByUserId(int.Parse(userId));
            return JsonSuccess(result);
        }
    }
}
