using Entities.Community;
using Entities.Goods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EntityConfigrations.Community
{
    internal class PostEntityConfigration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasOne(p => p.Publisher).WithMany(u => u.Posts).HasForeignKey(p => p.PublisherId);
            builder.Property(p => p.ImgUrls)
                .HasConversion(v =>
                JsonConvert.SerializeObject(v), v =>
                JsonConvert.DeserializeObject<IEnumerable<string>>(v) ?? new List<string>(),
                new ValueComparer<IEnumerable<string>>((c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
        }
    }
}
