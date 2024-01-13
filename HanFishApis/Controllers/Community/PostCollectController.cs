using HanFishApis.Controllers.Base;
using HanFishApis.Infrastructure.OpenApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Community;
using Services.Community;
using System.Security.Claims;

namespace HanFishApis.Controllers.Community
{
   
    public class PostCollectController : BaseController
    {
        private readonly IPostCollectService _postCollectService;

        public PostCollectController(IPostCollectService postCollectService)
        {
            _postCollectService = postCollectService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPostCollectAsync(PostIdModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            if (await _postCollectService.AddPostCollectAsync(model.PostId, int.Parse(userId)))
                return JsonSuccess("收藏成功");
            return JsonFail("收藏失败");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePostCollectAsync(PostIdModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            if (await _postCollectService.DeletePostCollectAsync(model.PostId, int.Parse(userId)))
                return JsonSuccess("取消收藏");
            return JsonFail("取消失败");
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Client))]
        public async Task<IActionResult> GetUserCollectPostIdsAsync(int? userid)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            var id = userid is null ? int.Parse(userId) : (int)userid;
            var result = await _postCollectService.GetUserCollectPostIdsAsync(id);
            return JsonSuccess(result);
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Client))]
        public async Task<IActionResult> GetUserCollectPostListAsync()
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            var result = await _postCollectService.GetUserCollectPostListAsync(int.Parse(userId));
            return JsonSuccess(result);
        }
    }
}
