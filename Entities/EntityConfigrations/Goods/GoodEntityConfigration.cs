using Entities.Account;
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

namespace Entities.EntityConfigrations.Goods
{
    internal class GoodEntityConfigration : IEntityTypeConfiguration<Good>
    {
        public void Configure(EntityTypeBuilder<Good> builder)
        {
            builder.Property(g => g.Price).HasPrecision(18, 2);
            builder.HasOne(g => g.Category).WithMany(c => c.Goods).HasForeignKey(g => g.CategoryId);
            builder.HasOne(g => g.User).WithMany(u => u.Goods).HasForeignKey(g => g.UserId);
            builder.Property(r => r.ImgUrls)
                .HasConversion(v =>
                JsonConvert.SerializeObject(v), v =>
                JsonConvert.DeserializeObject<IEnumerable<string>>(v) ?? new List<string>(),
                new ValueComparer<IEnumerable<string>>((c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
            builder.Property(r => r.Tags)
                .HasConversion(v =>
                JsonConvert.SerializeObject(v), v =>
                JsonConvert.DeserializeObject<IEnumerable<string>>(v) ?? new List<string>(),
                new ValueComparer<IEnumerable<string>>((c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
        }
    }
}
