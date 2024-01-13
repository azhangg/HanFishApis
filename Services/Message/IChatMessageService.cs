using Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Message
{
    public interface IChatMessageService
    {
        Task<ChatMessageResponseModel> GetChatMessagesAsync(int userId, int targetId, string datetime = "");

        Task<IEnumerable<ChatMessageResponseModel>> GetChatMessageUserUnReadAsync(int userId);

        Task<ChatMessageModel> AddChatMessageAsync(AddChatMessageModel model);

        Task<bool> DeleteChatMessageAsync(int id);

        Task<bool> RefuseMessageAsync(int id, int userId);

        Task<bool> WithDrawMessageAsync(int id, int userId);

        Task<bool> ReadMessageAsync(int readerId, IEnumerable<int> messageIds);
    }
}
