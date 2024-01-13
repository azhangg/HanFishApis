using HanFishApis.Controllers.Base;
using HanFishApis.Infrastructure.OpenApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Community;
using Services.Community;
using System.Security.Claims;

namespace HanFishApis.Controllers.Community
{
    public class PostLikeController : BaseController
    {
        private readonly IPostLikeService _postLikeService;

        public PostLikeController(IPostLikeService postLikeService)
        {
            _postLikeService = postLikeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPostLikeAsync(PostIdModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            if (await _postLikeService.AddPostLikeAsync(model.PostId, int.Parse(userId))) 
                return JsonSuccess("点赞成功");
            return JsonFail("点赞失败");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePostLikeAsync(PostIdModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            if (await _postLikeService.DeletePostLikeAsync(model.PostId, int.Parse(userId)))
                return JsonSuccess("取消点赞");
            return JsonFail("取消失败");
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Client))]
        public async Task<IActionResult> GetUserLikePostIdsAsync(int? userid)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            var id = userid is null ? int.Parse(userId) : (int)userid;
            var result = await _postLikeService.GetUserLikePostIdsAsync(id);
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersLikeCountAsync(int userId)
        {
            var result = await _postLikeService.GetUsersPostLikesAsync(userId);
            return JsonSuccess(result);
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Client))]
        public async Task<IActionResult> GetUserLikePostListAsync()
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            var result = await _postLikeService.GetUserLikePostListAsync(int.Parse(userId));
            return JsonSuccess(result);
        }
    }
}
