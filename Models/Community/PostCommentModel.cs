using Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Community
{
    public class PostCommentModel
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public required string Comment { get; set; }

        public string? ImgUrl { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? AvatarUrl { get; set; }

        public int PId { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class AddPostCommentModel
    {
        public int PostId { get; set; }

        public required string Comment { get; set; }

        public string? ImgUrl { get; set; }

        public int UserId { get; set; }

        public int PId { get; set; } = 0;
    }

    public class PostCommentResponseModel
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public required string Comment { get; set; }

        public string? ImgUrl { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? AvatarUrl { get; set; }

        public int PId { get; set; }

        public DateTime CreateTime { get; set; }

        public IEnumerable<PostCommentResponseModel> Children { get; set; }
    }

    public class CommentPostModel
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public required string Comment { get; set; }

        public string? ImgUrl { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? AvatarUrl { get; set; }

        public int PId { get; set; }

        public DateTime CreateTime { get; set; }

        public PostModel Post { get; set; }

        public UserModel Puser { get; set; }
    }
}
