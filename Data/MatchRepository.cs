using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public class MatchRepository : IMatchRepository
    {
        private readonly DataContext dataContext;
        private readonly IBetRepository betRepository;

        public MatchRepository(DataContext dataContext, IBetRepository betRepository)
        {
            this.dataContext = dataContext;
            this.betRepository = betRepository;
        }

        public async void Add(Match match)
        {
            await dataContext.Matches.AddAsync(match);
        }

        public void Delete(Match match)
        {
            dataContext.Matches.Remove(match);
        }

        public async Task<Match> GetMatch(int id)
        {
            return await dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score).FirstOrDefaultAsync(m => m.Id == id);
        }

        public IEnumerable<Match> GetMatches(bool notStarted, bool ended)
        {
            if (notStarted)
            {
                return dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score).Where(m => m.Date > DateTime.Now);
            }

            if (ended)
            {
                return dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score).Where(m => m.Score != null);
            }

            return dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score);
        }

        public IEnumerable<Match> GetTeamsMatches(int teamId)
        {
            return dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score).Where(m => ((m.TeamA.Id == teamId) || (m.TeamB.Id == teamId)) && (m.Score != null));
        }
    }
}
