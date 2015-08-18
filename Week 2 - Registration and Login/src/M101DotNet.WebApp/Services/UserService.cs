using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Repositories;
using M101DotNet.WebApp.Models.Account;

namespace M101DotNet.WebApp.Services
{
    public class UserService : IUserService
    {
        protected UserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        public UserService()
        {
            _userRepository = new UserRepository();
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
    }
}
