using Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Community
{
    public class PostModel
    {
        public int Id { get; set; }

        public required string Text { get; set; }

        public string Status { get; set; }

        public IEnumerable<string> ImgUrls { get; set; }

        public DateTime CreateTime { get; set; }

        public int PublisherId { get; set; }

        public int Comments { get; set; }

        public int Likes { get; set; }

        public int Collects { get; set; }

        public bool IsLike { get; set; }

        public bool IsCollect { get; set; }

        public UserModel Publisher { get; set; }
    }

    public class AddPostModel
    {
        public required string Text { get; set; }

        public IEnumerable<string> ImgUrls { get; set; }

        public int PublisherId { get; set; }
    }

    public class SetPostStatusModel
    {
        public int Id { get; set; }

        public int Status { get; set; }
    }

    public class PostIdModel
    {
        public int PostId { get; set; }
    }

    public class UserCommunityDataModel
    {
        public int AppraiseNum { get; set; }

        public int PostNum { get; set; }

        public int BePraiseNum { get; set; }

        public int CollectNum { get; set; }
    }
}
