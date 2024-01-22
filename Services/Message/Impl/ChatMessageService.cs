using AutoMapper;
using Entities.Message;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Models.Account;
using Models.Message;
using Repositories.Module.Account;
using Repositories.Module.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;
using Utils.Enums;
using Utils.Hubs;

namespace Services.Message.Impl
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<MessageHub> _hubContext;

        public ChatMessageService(IChatMessageRepository chatMessageRepository, IMapper mapper, IUserRepository userRepository, IHubContext<MessageHub> hubContext)
        {
            _chatMessageRepository = chatMessageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _hubContext = hubContext;
        }

        public async Task<ChatMessageModel> AddChatMessageAsync(AddChatMessageModel model)
        {
            ChatMessage chatMessage = _mapper.Map<ChatMessage>(model);
            chatMessage.CreateTime = DateTime.Now;
            await _chatMessageRepository.AddEntityAsync(chatMessage);
            await _chatMessageRepository.UnitOfWork.SaveChangeAsync();
            await _hubContext.Clients.User($"{model.ReceiverId}").SendAsync("SendMessage", new ChatMessageResponseModel
            {
                TargetInfo = _mapper.Map<UserModel>(await _userRepository.GetEntityAsync(u=>u.Id==model.SenderId) ?? new Entities.Account.User
                {
                    Id = 0,
                    Name = "社区消息",
                    LoginName = "",
                    Password = "",
                    AvatarUrl = "Files/SystemResource/notification.png"
                }),
                ChatMessages = new List<ChatMessageDetailModel>() {
                    _mapper.Map<ChatMessage,ChatMessageDetailModel>(chatMessage)
                }
            });
            if(chatMessage.Type == ChatMessageTypes.订单连接)
            {
                await _hubContext.Clients.User($"{model.SenderId}").SendAsync("SendMessage", new ChatMessageResponseModel
                {
                    TargetInfo = _mapper.Map<UserModel>(await _userRepository.GetEntityAsync(u => u.Id == model.ReceiverId)),
                    ChatMessages = new List<ChatMessageDetailModel>() {
                    _mapper.Map<ChatMessage,ChatMessageDetailModel>(chatMessage)
                }
                });
            }
            return _mapper.Map<ChatMessageModel>(chatMessage);
        }

        public async Task<bool> DeleteChatMessageAsync(int id)
        {
            var chatMessage = await _chatMessageRepository.GetEntityAsNoTrackingAsync(c => c.Id == id);
            if (chatMessage is null) throw new CustomException("该消息不存在");
            _chatMessageRepository.DeleteEntity(chatMessage);
            return await _chatMessageRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<ChatMessageResponseModel> GetChatMessagesAsync(int userId, int targetId, string datetime = "")
        {
            var userInfo = await _userRepository.GetEntityAsNoTrackingAsync(u => u.Id == targetId);
            if(userInfo is null) userInfo = new Entities.Account.User {
                Id = 0, 
                Name = "社区消息",
                LoginName="",
                Password="",
                AvatarUrl= "Files/SystemResource/notification.png"
            };
            var startTime = DateTime.TryParse(datetime,out var parsedTime) ? parsedTime: DateTime.Now;
            var (chatMessages,_) = await _chatMessageRepository.GetListToPaginationAsync(c => c.CreateTime, true,
                c => ((c.SenderId == userId && c.ReceiverId == targetId)
                || (c.SenderId == targetId && c.ReceiverId == userId))
                && c.CreateTime < startTime
                , 1, 10);
            return new ChatMessageResponseModel
            {
                TargetInfo = _mapper.Map<UserModel>(userInfo),
                ChatMessages = chatMessages.Select(_mapper.Map<ChatMessageDetailModel>).ToList()
            };
        }

        public async Task<IEnumerable<ChatMessageResponseModel>> GetChatMessageUserUnReadAsync(int userId)
        {
            var chatMessages = await _chatMessageRepository.GetEntitiesAsync(c => c.ReceiverId == userId && !c.IsRead);
            var result = new List<ChatMessageResponseModel>(); 
            foreach (var item in chatMessages.OrderBy(c => c.CreateTime).GroupBy(c => c.SenderId))
            {
                var userInfo = await _userRepository.GetEntityAsNoTrackingAsync(u => u.Id == item.Key);
                if (userInfo is null) userInfo = new Entities.Account.User
                {
                    Id = 0,
                    Name = "社区消息",
                    LoginName = "",
                    Password = "",
                    AvatarUrl = "Files/SystemResource/notification.png"
                };
                result.Add(new ChatMessageResponseModel
                {
                    TargetInfo = _mapper.Map<UserModel>(userInfo),
                    ChatMessages = item.Select(_mapper.Map<ChatMessageDetailModel>).ToList()
                });
            }
            return result;
        }

        public async Task<bool> ReadMessageAsync(int readerId, IEnumerable<int> messageIds)
        {
            var targetId = 0;
            var messages = await _chatMessageRepository.GetEntitiesAsync(m=>m.ReceiverId == readerId && !m.IsRead);
            messages.ForEach(item =>
            {
                if(messageIds.Contains(item.Id))
                {
                    targetId = item.SenderId;
                    item.IsRead = true;
                }
            });
            _chatMessageRepository.UpdateEntities(messages);
            var result = await _chatMessageRepository.UnitOfWork.SaveChangeAsync();
            if (result)
            {
                if (messageIds.Count() > 0 && readerId != targetId)
                {
                    await _hubContext.Clients.User($"{targetId}").SendAsync("ChatMessageRead",
                    new {
                        TargetId = readerId,
                        IsReadIds= messageIds
                    });
                }
                return true;
            }
            return false;
        }

        public async Task<bool> RefuseMessageAsync(int id, int userId)
        {
            var chatMessage = await _chatMessageRepository.GetEntityAsync(c => c.Id == id);
            if (chatMessage is null) throw new CustomException("该消息不存在");
            if (chatMessage.RefusherIds.Any(r => r == userId)) throw new CustomException("消息已屏蔽该用户");
            chatMessage.RefusherIds = chatMessage.RefusherIds.Append(userId);
            _chatMessageRepository.UpdateEntity(chatMessage);
            return await _chatMessageRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<bool> WithDrawMessageAsync(int id, int userId)
        {
            var chatMessage = await _chatMessageRepository.GetEntityAsync(c => c.Id == id);
            if (chatMessage is null) throw new CustomException("该消息不存在");
            if (DateTime.Now > chatMessage.CreateTime.AddMinutes(2)) throw new CustomException("撤回有效时间为两分钟");
            chatMessage.Type = ChatMessageTypes.撤回;
            _chatMessageRepository.UpdateEntity(chatMessage);
            var result = await _chatMessageRepository.UnitOfWork.SaveChangeAsync();
            if (result)
            {
                await _hubContext.Clients.User($"{chatMessage.ReceiverId}").SendAsync("SendMessage", new ChatMessageResponseModel
                {
                    TargetInfo = _mapper.Map<UserModel>(await _userRepository.GetEntityAsync(u => u.Id == chatMessage.SenderId)),
                    ChatMessages = new List<ChatMessageDetailModel>() {
                    _mapper.Map<ChatMessage,ChatMessageDetailModel>(chatMessage)
                }
                });
            }
            return result;
        }
    }
}
