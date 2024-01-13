using AutoMapper;
using HanFishApis.Controllers.Base;
using HanFishApis.Infrastructure.OpenApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Account;
using Services.Account;

namespace HanFishApis.Controllers.Account
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> AddRoleAsync(AddRoleModel roleModel)
        {
            var result = await _roleService.AddRoleAsync(roleModel);
            return JsonSuccess(result);
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> DelateRoleAsync(int id)
        {
            if (await _roleService.DeleteRoleAsync(id))
                return JsonSuccess("删除成功");
            return JsonSuccess("删除失败");
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> UpdateRoleAsync(UpdateRoleModel roleModel)
        {
            if (await _roleService.UpdateRoleAsync(roleModel))
                return JsonSuccess("修改成功");
            return JsonSuccess("修改失败");
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> GetAllRoleAsync()
        {
            var result = await _roleService.GetAllRolesAsync();
            return JsonSuccess(result);
        }
    }
}
