using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Goods
{
    public record GoodCategory : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }

        public IEnumerable<Good> Goods { get; set; }
    }
}
