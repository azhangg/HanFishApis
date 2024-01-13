using Entities.Account;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Community
{
    public record class PostComment : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int PostId { get; set; }

        public required string Comment { get; set; }

        public string? ImgUrl {  get; set; }

        public int UserId { get; set; }

        public int PId { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
