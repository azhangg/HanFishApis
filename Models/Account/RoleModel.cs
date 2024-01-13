using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Account
{
    public class RoleModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<int> MenuIds { get; set; } = new List<int>();
    }

    public class AddRoleModel
    {
        public required string Name { get; set; }

        public List<int> MenuIds { get; set; }
    }

    public class UpdateRoleModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public List<int> MenuIds { get; set; }
    }
}
