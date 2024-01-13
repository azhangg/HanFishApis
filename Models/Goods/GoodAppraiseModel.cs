using Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Goods
{
    public class GoodAppraiseModel
    {
        public int Id { get; set; }

        public int Rate { get; set; }

        public int GoodId { get; set; }

        public int UserId { get; set; }

        public int SellerId { get; set; }

        public string? Comment { get; set; }

        public string? CommentImgUrl { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class GoodAppraiseResponseModel
    {
        public int Id { get; set; }

        public int Rate { get; set; }

        public int GoodId { get; set; }

        public int UserId { get; set; }

        public int SellerId { get; set; }

        public string? Comment { get; set; }

        public string? CommentImgUrl { get; set; }

        public DateTime CreateTime { get; set; }

        public UserModel User { get; set; }

        public GoodModel Good { get; set; }
    }

    public class AddGoodAppraiseModel
    {
        public int Rate { get; set; }

        public int GoodId { get; set; }

        public int UserId { get; set; }

        public int SellerId { get; set; }

        public string? Comment { get; set; }

        public string? CommentImgUrl { get; set; }
    }
}
