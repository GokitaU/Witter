using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dataContext;

        public UserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Delete(User user)
        {
            dataContext.Remove(user);
        }

        public async Task<User> GetUser(int userId)
        {
            return await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return dataContext.Users;
        }
    }
}
