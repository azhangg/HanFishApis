using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;

namespace Entities.Message
{
    public record class ChatMessage : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public required string Content { get; set; }

        public IEnumerable<int> RefusherIds { get; set; } = Enumerable.Empty<int>();

        public bool IsRead { get; set; }

        public ChatMessageTypes Type { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
