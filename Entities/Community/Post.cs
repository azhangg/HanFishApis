using Entities.Account;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;

namespace Entities.Community
{
    public record class Post:BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public required string Text { get; set; }

        public PostStatus Status { get; set; }

        public IEnumerable<string> ImgUrls { get; set; }

        public DateTime CreateTime { get; set; }

        public int PublisherId { get; set; }

        public User Publisher { get; set; }
    }
}
