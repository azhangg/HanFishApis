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
    public record class Order : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public required string Code { get; set; }

        public int AddressId { get; set; }

        public int GoodId { get; set; }

        public int UserId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreateTime { get; set; }

        public Good Good { get; set; }

        public User User { get; set; }

        public Address Address { get; set; }
    }
}
