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

        public MatchRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
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
                return dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score).Where(m => m.Date > DateTime.Now).OrderBy(m => m.Date);
            }

            if (ended)
            {
                return dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score).Where(m => m.Score != null).OrderBy(m => m.Date);
            }

            return dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score).OrderBy(m => m.Date);
        }

        public IEnumerable<Match> GetMatchesForAdmin()
        {
            return dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score).Where(m => m.Score == null || m.Date > DateTime.Now).OrderBy(m => m.Date);
        }

        public Match GetMatchSync(int id)
        {
            var match = dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score).FirstOrDefault(m => m.Id == id);
            return match;
        }

        public IEnumerable<Match> GetTeamsMatches(int teamId)
        {
            return dataContext.Matches.Include(m => m.TeamA).Include(m => m.TeamB).Include(m => m.Score).Where(m => ((m.TeamA.Id == teamId) || (m.TeamB.Id == teamId)) && (m.Score != null));
        }
    }
}
