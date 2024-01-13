﻿using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Community
{
    public record class PostCollect : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int PostId { get; set; }

        public int UserId { get; set; }
    }
}
