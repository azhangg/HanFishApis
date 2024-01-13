using Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Goods
{
    public class GoodModel
    {
        public int Id { get; set; }

        public required string Description { get; set; }

        public IEnumerable<string> ImgUrls { get; set; } = Enumerable.Empty<string>();

        public decimal Price { get; set; }

        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();

        public string Status { get; set; }

        public DateTime CreateTime { get; set; }

        public int CategoryId { get; set; }

        public int UserId { get; set; }

        public GoodCategoryModel Category { get; set; }

        public UserModel User { get; set; }
    }

    public class AddGoodModel
    {
        public required string Description { get; set; }

        public IEnumerable<string> ImgUrls { get; set; } = Enumerable.Empty<string>();

        public decimal Price { get; set; }

        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();

        public int CategoryId { get; set; }

        public int UserId { get; set; }
    }

    public class UpdateGoodModel 
    {
        public int Id { get; set; }

        public decimal? Price { get; set; }

        public int? Status { get; set; }
    }
}
