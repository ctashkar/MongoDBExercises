using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M101DotNet.WebApp.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization; // BsonClassMap
using MongoDB.Bson.Serialization.Conventions;

namespace M101DotNet.WebApp.Repositories
{
    public class PostRepository : IPostRepository
    {
        protected BlogContext _dbContext;
        protected IMongoCollection<Post> _postCollection;

        public PostRepository()
        {
            _dbContext = new BlogContext();
            _postCollection = _dbContext.Posts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> AddNewUser(User user)
        {
            await _usersCollection.InsertOneAsync(user);

            return user;
        }

        public async Task<List<Post>> GetRecentPosts()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<Post> InsertNewPost(Post post)
        {
            await _postCollection.InsertOneAsync(post);

            return post;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Post> FindPost(ObjectId id)
        {
            Post postFound = null;

            var list = await _postCollection.Find(post => post.Id == id).ToListAsync();

            if (list.Count > 1)
                throw new Exception("There are more than one post registered with the provided post Id");
            else if (list.Count == 1)
            {
                postFound = list.FirstOrDefault();
            }

            return postFound;
        }

        public async Task<List<Post>> FindPostsByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> AddCommentToPost(Comment comment, ObjectId postId)
        {
            throw new NotImplementedException();
        }
    }
}
