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
    public class UserRepository : IUserRepository
    {
        protected BlogContext _dbContext;
        protected IMongoCollection<User> _usersCollection;

        public UserRepository()
        {
            _dbContext = new BlogContext();
            _usersCollection = _dbContext.Users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmail(string email)
        {
            User userFound = null;

            var list = await _usersCollection.Find(user => user.Email == email).ToListAsync();

            if (list.Count > 1)
                throw new Exception("There are more than one account registered with the provided email address");
            else if (list.Count == 1)
            {
                userFound = list.FirstOrDefault();
            }

            return userFound;
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
    }
}
