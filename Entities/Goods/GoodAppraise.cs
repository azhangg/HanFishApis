using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Goods
{
    public record class GoodAppraise : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int Rate { get; set; }

        public int GoodId { get; set;}

        public int UserId { get; set; }

        public int SellerId { get; set; }

        public string? Comment { get; set; }

        public string? CommentImgUrl { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
