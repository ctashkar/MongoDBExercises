using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Repositories;
using M101DotNet.WebApp.Models.Account;
using MongoDB.Bson;

namespace M101DotNet.WebApp.Services
{
    public class PostService : IPostService
    {
        protected PostRepository _postRepository;

        /// <summary>
        /// 
        /// </summary>
        public PostService()
        {
            _postRepository = new PostRepository();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> RegisterNewUser(RegisterModel model)
        {
            var newUser = new User() { Name = model.Name, Email = model.Email };

            newUser = await _userRepository.AddNewUser(newUser);

            return newUser;
        }

        public Task<List<Post>> GetRecentPosts()
        {
            throw new NotImplementedException();
        }

        public Task<Post> InsertNewPost(Post post)
        {
            var result = _postRepository.InsertNewPost(post);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Post> FindPost(ObjectId id)
        {
            var result = _postRepository.FindPost(id);

            return result;
        }

        public Task<List<Post>> FindPostsByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public Task<Post> AddCommentToPost(Comment comment, ObjectId postId)
        {
            throw new NotImplementedException();
        }
    }
}
