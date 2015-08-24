using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Models.Account;
using MongoDB.Bson;

namespace M101DotNet.WebApp.Services
{
    interface IPostService
    {
        Task<List<Post>> GetRecentPosts();

        Task<Post> InsertNewPost(Post post);

        Task<Post> FindPost(ObjectId id);

        Task<List<Post>> FindPostsByTag(string tag);

        Task<Post> AddCommentToPost(Comment comment, ObjectId postId);
    }
}
