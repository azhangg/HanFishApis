using Entities.Account;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Goods
{
    public record class Address : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public required string Name { get; set; }

        [MaxLength(11)]
        public required string ContactNum { get; set; }

        public required string DeliveryAddress { get; set; }

        public bool IsDefault { get; set; }

        public User User { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
