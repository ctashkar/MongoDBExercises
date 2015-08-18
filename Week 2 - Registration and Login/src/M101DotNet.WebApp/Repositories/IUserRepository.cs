using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M101DotNet.WebApp.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace M101DotNet.WebApp.Repositories
{
    interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);

        Task<User> AddNewUser(User user);
    }
}
