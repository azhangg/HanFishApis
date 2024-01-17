using AutoMapper;
using HanFishApis.Controllers.Base;
using HanFishApis.Infrastructure.OpenApi;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models.Account;
using Services.Account;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Utils;
using Utils.Crypto;

namespace HanFishApis.Controllers.Account
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetUsersAsync();
            return JsonSuccess(users);
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> GetUsersToPaginationAsync(string? searchText,int page,int count)
        {
            if (searchText.IsNullOrEmpty()) searchText = "";
            var users = await _userService.GetUsersToPagination(searchText,page,count);
            return JsonSuccess(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var userId = User.FindFirstValue("id");
            if (userId is null)
                return Unauthorized();
            var userInfo = await _userService.GetUserByIdAsync(int.Parse(userId));
            return JsonSuccess(userInfo);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfoByUserIdAsync(int id)
        {
            var userInfo = await _userService.GetUserByIdAsync(id);
            return JsonSuccess(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserModel user)
        {
            if (await _userService.UpdateUserAsync(_mapper.Map<UserModel>(user)))
                return JsonSuccess("修改成功");
            return JsonFail("修改失败");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserPasswordAsync(UpdatePasswordModel model)
        {
            model.Password = MD5Encrypt.EncryptTo32(model.Password);
            if (await _userService.UpdateUserPasswordAsync(model))
                return JsonSuccess("修改成功");
            return JsonFail("修改失败");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClientUserAsync(UpdateClientUserModel user)
        {
            if (await _userService.UpdateUserAsync(user))
                return JsonSuccess("修改成功");
            return JsonFail("修改失败");
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            if (await _userService.DeleteUserAsync(id))
                return JsonSuccess("删除成功");
            return JsonFail("删除失败");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UploadUserAvatarAsync(IFormFile  formFile)
        {
            var result = await FileHelper.UploadFilesAsync(FileHelper.FileType.UserProfile, new List<IFormFile>() { formFile } );
            return JsonSuccess(result,"上传成功");
        }
    }
}
