using Entities.Base;
using Entities.Community;
using Entities.Goods;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Account
{
    public record User: BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        public required string Name { get; set; }

        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(50)]
        public required string LoginName { get; set; }

        [StringLength(200)]
        public required string Password { get; set; }

        [StringLength(200)]
        public string? AvatarUrl { get; set; }

        public DateTime CreateTime { get; set; }

        #region 角色表 
        public int RoleId { get; set; }

        public Role Role { get; set; }
        #endregion

        #region 物品表
        public IEnumerable<Good> Goods { get; set; }
        #endregion

        #region 帖子表
        public IEnumerable<Post> Posts { get; set; }
        #endregion

        #region 订单表
        public IEnumerable<Order> Orders { get; set; }
        #endregion

        #region 订单表
        public IEnumerable<Address> Address { get; set; }
        #endregion
    }
}
