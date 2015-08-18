using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Models.Account;

namespace M101DotNet.WebApp.Services
{
    interface IUserService
    {
        Task<User> GetUserByEmail(string email);

        Task<User> RegisterNewUser(RegisterModel model);
    }
}
