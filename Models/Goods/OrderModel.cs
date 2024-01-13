using Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Goods
{
    public class OrderModel
    {
        public int Id { get; set; }

        public required string Code { get; set; }

        public int AddressId { get; set; }

        public int GoodId { get; set; }

        public int UserId { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public GoodModel Good { get; set; }

        public UserModel User { get; set; }

        public AddressModel Address { get; set; }
    }

    public class AddOrderModel
    {
        public int GoodId { get; set; }

        public int UserId { get; set; }

        public int AddressId { get; set; }
    }

    public class UpdateOrderModel
    {
        public int Id { get; set; }

        public int AddressId { get; set; }
    }

    public class ModifyOrderStatusModel
    {
        public int Id { get; set; }

        public int Status { get; set; }
    }
}
