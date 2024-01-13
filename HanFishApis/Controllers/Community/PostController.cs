using HanFishApis.Controllers.Base;
using HanFishApis.Infrastructure.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using Models.Community;
using Services.Community;
using System.Security.Claims;
using Utils.Enums;

namespace HanFishApis.Controllers.Community
{
    public class PostController : BaseController
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostListToPaginationAsync(int page,int count, string search="", PostStatus status = PostStatus.已发布)
        {
            var result = await _postService.GetPostListToPaginationAsync(page, count, search, status);
            return JsonSuccess(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostByIdAsync(int postId)
        {
            var result = await _postService.GetPostByIdAsync(postId);
            if (result is null) return JsonFail("该帖子不存在");
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostListByUserIdAsync(int userId)
        {
            var result = await _postService.GetPostsByUserIdAsync(userId);
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCollectedPostsByUserIdAsync(int userId)
        {
            var result = await _postService.GetPostsUserCollectedAsync(userId);
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCommunityDataAsync(int userId)
        {
            var result = await _postService.GetUserCommunityDataAsync(userId);
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLikedPostsByUserIdAsync(int userId)
        {
            var result = await _postService.GetPostsUserLikedAsync(userId);
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentedPostsByUserIdAsync(int userId)
        {
            var result = await _postService.GetPostsUserCommentedAsync(userId);
            return JsonSuccess(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPostAsync(AddPostModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null)
                return Unauthorized();
            model.PublisherId = int.Parse(userId);
            var result = await _postService.AddPostAsync(model);
            if (result is null) return JsonFail("添加失败");
            return JsonSuccess(result, "添加成功");
        }

        [HttpPost]
        public async Task<IActionResult> SetPostStatusAsync(SetPostStatusModel model)
        {
            var result = await _postService.UpdatePostStatusAsync(model.Id,model.Status);
            if (result) return JsonSuccess("设置成功"); 
            return JsonFail("设置失败");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePostAsync(DeleteModel model)
        {
            var result = await _postService.DeletePostAsync(model.Id);
            if (result) return JsonSuccess("删除成功");
            return JsonFail("删除失败");
        }
    }
}
