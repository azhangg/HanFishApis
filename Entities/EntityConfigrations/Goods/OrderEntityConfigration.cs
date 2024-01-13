using Entities.Goods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EntityConfigrations.Goods
{
    internal class OrderEntityConfigration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.Good).WithMany(g => g.Orders).HasForeignKey(o => o.GoodId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.Address).WithMany(a => a.Orders).HasForeignKey(o => o.AddressId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
