using Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Goods
{
    public class AddressModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public required string Name { get; set; }

        [MaxLength(11)]
        public required string ContactNum { get; set; }

        public required string DeliveryAddress { get; set; }

        public bool IsDefault { get; set; }

        public UserModel User { get; set; }
    }

    public class AddAddressModel
    {
        public required string Name { get; set; }

        [MaxLength(11)]
        public required string ContactNum { get; set; }

        public required string DeliveryAddress { get; set; }
    }

    public class UpdateAddressModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        [MaxLength(11)]
        public string? ContactNum { get; set; }

        public string? DeliveryAddress { get; set; }
    }
}
