using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly DataContext dataContext;

        public LeagueRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async void Create(League league)
        {
            await dataContext.Leagues.AddAsync(league);
        }

        public void Delete(League league)
        {
            dataContext.Leagues.Remove(league);
        }

        public async Task<League> GetLeague(int id)
        {
            return await dataContext.Leagues.Include(l => l.Admin).FirstOrDefaultAsync(l => l.Id == id);
        }

        public IEnumerable<League> GetLeagues()
        {
            return dataContext.Leagues.Include(l => l.Admin);
        }

        public IEnumerable<League> GetLeaguesByUser(int id)
        {
            return dataContext.UsersInLeagues.Where(x => x.UserId == id).Select(x => x.League).Include(l => l.Admin);
        }

        public IEnumerable<User> GetUsersByLeague(int id)
        {
            return dataContext.UsersInLeagues.Where(x => x.LeagueId == id).Select(x => x.User).OrderBy(y => y.Score);
        }

        public async void Join(UserInLeague userInLeague)
        {
            await dataContext.UsersInLeagues.AddAsync(userInLeague);
        }

        public async Task<bool> Member(int leagueId, int userId)
        {
            return await dataContext.UsersInLeagues.AnyAsync(x => x.LeagueId == leagueId && x.UserId == userId);
        }

        public async Task<bool> NameExists(string name)
        {
            var league = await dataContext.Leagues.FirstOrDefaultAsync(l => l.Name == name);

            return league == null ? false : true;
        }
    }
}
