using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Community
{
    public record class Banner : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string ImgUrl { get; set; }

        public bool Apply { get; set; }

        public int Order { get; set; }

        public DateTime CreateTime { get; set; }   
    }
}
