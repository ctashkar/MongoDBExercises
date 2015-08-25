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
        #region Protected fields

        protected BlogContext _dbContext;
        protected IMongoCollection<Post> _postCollection;

        #endregion

        #region PostRepository() Constructor

        /// <summary>
        /// 
        /// </summary>
        public PostRepository()
        {
            _dbContext = new BlogContext();
            _postCollection = _dbContext.Posts;
        }

        #endregion

        #region GetRecentPosts

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Post>> GetRecentPosts()
        {
            var listOfRecentPosts = await _postCollection.Find(new BsonDocument())
                .Sort(Builders<Post>.Sort.Descending(x => x.CreatedAtUtc))
                    .Limit(10)
                        .ToListAsync();

            return listOfRecentPosts;
        }

        #endregion

        #region InsertNewPost

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

        #endregion
        
        #region FindPost

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

        #endregion

        #region FindPostsByTag

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task<List<Post>> FindPostsByTag(string tag)
        {
            var builder  = new FilterDefinitionBuilder<Post>();  // Builders<Post>.Filter.AnyIn(x => x.Tags, new List<string>() { tag });
            FilterDefinition<Post> filter = null;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                filter = builder.AnyIn(x => x.Tags, new List<string>() { tag });
            }
            else
            {
                filter = new BsonDocument();
            }

            var listOfRecentPosts = await _postCollection.Find(filter)  
                .Sort(Builders<Post>.Sort.Descending(x => x.CreatedAtUtc))
                    .ToListAsync();

            return listOfRecentPosts;
        }

        #endregion

        #region AddCommentToPost

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<Post> AddCommentToPost(Comment comment, ObjectId postId)
        {
            var filterDefinition = Builders<Post>.Filter.Eq(post => post.Id, postId);
            var updateDefinition = Builders<Post>.Update.Push(post => post.Comments, comment);
            var updateOptions = new FindOneAndUpdateOptions<Post>() { ReturnDocument = ReturnDocument.After };

            var result = await _postCollection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, updateOptions);

            return result;
        }

        #endregion

    }
}
