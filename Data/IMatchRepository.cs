using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public interface IMatchRepository
    {
        IEnumerable<Match> GetMatches(bool started, bool ended);
        IEnumerable<Match> GetTeamsMatches(int teamId);
        IEnumerable<Match> GetMatchesForAdmin();
        Task<Match> GetMatch(int id);
        Match GetMatchSync(int id);
        void Add(Match match);
        void Delete(Match match);
    }
}
