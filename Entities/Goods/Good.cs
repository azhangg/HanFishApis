using Entities.Account;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;

namespace Entities.Goods
{
    public record class Good : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public required string Description { get; set; }

        public IEnumerable<string> ImgUrls { get; set; } = Enumerable.Empty<string>();

        public decimal Price { get; set; }

        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();

        public GoodStatus Status { get; set; }

        public DateTime CreateTime { get; set; }

        public int CategoryId { get; set; }

        public int UserId { get; set; }

        public GoodCategory Category { get; set; }

        public User User { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
