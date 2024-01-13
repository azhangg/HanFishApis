using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.System
{
    public class MenuModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Title { get; set; }

        public bool Hidden { get; set; }

        public int Order { get; set; }

        public int? PId { get; set; }

        public string? Icon { get; set; }

        public string? Redirect { get; set; }

        public bool? Affix { get; set; }

        public bool? CachePage { get; set; }

        public bool? AlwaysShow { get; set; }

        public bool? LeaveRmCachePage { get; set; }

        public bool? CloseTabRmCache { get; set; }

        public string? CreateTime { get; set; }

        public List<MenuModel> Children { get; set; }
    }

    public class AddMenuModel 
    {
        public required string Name { get; set; }

        public required string Title { get; set; }

        public bool Hidden { get; set; }

        public int Order { get; set; }

        public int? PId { get; set; }

        public string? Icon { get; set; }

        public string? Redirect { get; set; }

        public bool? Affix { get; set; }

        public bool? CachePage { get; set; }

        public bool? AlwaysShow { get; set; }

        public bool? LeaveRmCachePage { get; set; }

        public bool? CloseTabRmCache { get; set; }
    }

    public class UpdateMenuModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Title { get; set; }

        public bool Hidden { get; set; }

        public int Order { get; set; }

        public int? PId { get; set; }

        public string? Icon { get; set; }

        public string? Redirect { get; set; }

        public bool? Affix { get; set; }

        public bool? CachePage { get; set; }

        public bool? AlwaysShow { get; set; }

        public bool? LeaveRmCachePage { get; set; }

        public bool? CloseTabRmCache { get; set; }
    }
}
