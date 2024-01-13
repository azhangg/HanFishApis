using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Account
{
    public class UserModel
    {
        public int Id { get; set; }

        [StringLength(6)]
        public required string Name { get; set; }

        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(50)]
        public required string LoginName { get; set; }

        [JsonIgnore]
        [StringLength(200)]
        public string Password { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(200)]
        public string? AvatarUrl { get; set; }

        public int? RoleId { get; set; }
    }

    #region 注册用户
    public class RegisterUserModel 
    {
        [StringLength(6)]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(20)]
        public required string LoginName { get; set; }

        [StringLength(200)]
        public required string Password { get; set; }

        [StringLength(200)]
        public string? AvatarUrl { get; set; }

        public int? RoleId { get; set; }
    }
    #endregion

    #region 修改用户信息
    public class UpdateUserModel
    {
        public int Id { get; set; }

        [StringLength(20)]
        public required string Name { get; set; }

        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? AvatarUrl { get; set; }
    }
    #endregion

    #region 修改用户密码
    public class UpdateUserPasswordModel
    {
        public int Id { get; set; }

        [StringLength(200)]
        public required string Password { get; set; }
    }
    #endregion

    #region 修改用户角色
    public class UpdateUserRoleModel
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
    }
    #endregion


    public class UpdateClientUserModel
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? AvatarUrl { get; set; }
    }
}
