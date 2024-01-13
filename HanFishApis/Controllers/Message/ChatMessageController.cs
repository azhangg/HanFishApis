using HanFishApis.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using Models.Message;
using Services.Message;
using System.Security.Claims;
using Utils.Hubs;

namespace HanFishApis.Controllers.Message
{
    public class ChatMessageController : BaseController
    {
        private readonly IChatMessageService _chatMessageService;

        public ChatMessageController(IChatMessageService chatMessageService, IHubContext<MessageHub> hubContext)
        {
            _chatMessageService = chatMessageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetChatMessagesAsync(int targetId, string time = "")
        {
            var userId = User.FindFirstValue("id");
            if (userId is null)
                return Unauthorized();
            var result = await _chatMessageService.GetChatMessagesAsync(int.Parse(userId), targetId, time);
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetChatMessagesUserUnReadAsync()
        {
            var userId = User.FindFirstValue("id");
            if (userId is null)
                return Unauthorized();
            var result = await _chatMessageService.GetChatMessageUserUnReadAsync(int.Parse(userId));
            return JsonSuccess(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddChatMessagesAsync(AddChatMessageModel model)
        {
            var result = await _chatMessageService.AddChatMessageAsync(model);
            if (result is null) return JsonFail("添加失败");
            return JsonSuccess(result,"添加成功");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChatMessageAsync(DeleteModel model)
        {
            var result = await _chatMessageService.DeleteChatMessageAsync(model.Id);
            if (!result) return JsonFail("删除失败");
            return JsonSuccess("删除成功");
        }

        [HttpPost]
        public async Task<IActionResult> RefuseMessageAsync(DeleteModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null)
                return Unauthorized();
            var result = await _chatMessageService.RefuseMessageAsync(model.Id, int.Parse(userId));
            if (!result) return JsonFail("拒绝失败");
            return JsonSuccess("拒绝成功");
        }

        [HttpPost]
        public async Task<IActionResult> WithDrawMessageAsync(DeleteModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null)
                return Unauthorized();
            var result = await _chatMessageService.WithDrawMessageAsync(model.Id, int.Parse(userId));
            if (!result) return JsonFail("撤回失败");
            return JsonSuccess("撤回成功");
        }

        [HttpPost]
        public async Task<IActionResult> ReadMessageAsync(ReadMessageModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null)
                return Unauthorized();
            var result = await _chatMessageService.ReadMessageAsync(int.Parse(userId), model.MessageIds);
            if (result) return JsonSuccess("已读消息"); 
            return JsonFail("已读失败");
        }
    }
}
