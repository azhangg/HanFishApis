using Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Entities.EntityConfigrations.Account
{
    internal class RoleEntityConfigration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.MenuIds)
                .HasConversion(v =>
                JsonConvert.SerializeObject(v), v =>
                JsonConvert.DeserializeObject<List<int>>(v) ?? new List<int>(),
                new ValueComparer<List<int>>((c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
        }
    }
}