using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public interface IUserRepository
    {
        Task<User> GetUser(int userId);
        void Delete(User user);
        IEnumerable<User> GetUsers();
        IEnumerable<User> GetBannedUsers();
        IEnumerable<User> GetNotBannedUsers();
        IEnumerable<User> GetAdminUsers();
    }
}
