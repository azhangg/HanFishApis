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
    internal class GoodCategoryEntityConfigration : IEntityTypeConfiguration<GoodCategory>
    {
        public void Configure(EntityTypeBuilder<GoodCategory> builder)
        {
        }
    }
}
