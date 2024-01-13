using HanFishApis.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Community;
using Services.Community;
using System.Security.Claims;

namespace HanFishApis.Controllers.Community
{
    public class PostCommentController : BaseController
    {
        private readonly IPostCommentService _postCommentService;

        public PostCommentController(IPostCommentService postCommentService)
        {
            _postCommentService = postCommentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPostCommentAsync(AddPostCommentModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            model.UserId = int.Parse(userId);
            var result = await _postCommentService.AddPostCommentAsync(model);
            return JsonSuccess(result,"添加成功");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePostCommentAsync(DeleteModel model)
        {
            if (await _postCommentService.DeletePostCommentAsync(model.Id)) return JsonSuccess("删除成功");
            return JsonFail("删除失败");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostCommentListByIdAsync(int id)
        {
            var result = await _postCommentService.GetCommentListByPostIdAsync(id);
            return JsonSuccess(result);
        }
    }
}
