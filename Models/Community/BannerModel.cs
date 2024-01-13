using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Community
{
    public class BannerModel
    {
        public int Id { get; set; }

        public string ImgUrl { get; set; }

        public bool Apply { get; set; }

        public int Order { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class AddBannerModel
    {
        public string ImgUrl { get; set; }

        public bool Apply { get; set; }

        public int Order { get; set; }
    }

    public class UpdateBannerModel
    {
        public int Id { get; set; }

        public string ImgUrl { get; set; }

        public bool Apply { get; set; }

        public int Order { get; set; }
    }
}
