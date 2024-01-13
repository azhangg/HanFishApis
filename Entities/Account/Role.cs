using Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Account
{
    public record Role : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(12)]
        public required string Name { get; set; }

        public List<int> MenuIds { get; set; } = new List<int>();

        public ICollection<User> Users { get; set; }
    }
}
