using Entities.Goods;
using Entities.Message;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EntityConfigrations.Message
{
    internal class ChatMessageEntityConfigration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.Property(m => m.RefusherIds)
                .HasConversion(v =>
                JsonConvert.SerializeObject(v), v =>
                JsonConvert.DeserializeObject<IEnumerable<int>>(v) ?? new List<int>(),
                new ValueComparer<IEnumerable<int>>((c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
        }
    }
}
