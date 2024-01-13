using Models.Account;
using Models.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Message
{
    public class ChatMessageModel
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public required string Content { get; set; }

        public IEnumerable<int> RefusherIds { get; set; } = Enumerable.Empty<int>();

        public bool IsRead { get; set; }

        public int Type { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class ChatMessageDetailModel
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public required string Content { get; set; }

        public IEnumerable<int> RefusherIds { get; set; } = Enumerable.Empty<int>();

        public bool IsRead { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public DateTime CreateTime { get; set; }

        public GoodModel Good { get; set; }
    }

    public class ChatMessageResponseModel 
    {
        public UserModel TargetInfo { get; set; }

        public IEnumerable<ChatMessageDetailModel> ChatMessages { get; set; }
    }

    public class AddChatMessageModel
    {
        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public required string Content { get; set; }

        public int type { get; set; }
    }

    public class ReadMessageModel
    {
        public IEnumerable<int> MessageIds { get; set; }
    }
}
