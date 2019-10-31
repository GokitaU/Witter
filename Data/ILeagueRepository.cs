using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public interface ILeagueRepository
    {
        void Create(League league);
        void Delete(League league);
        void DeleteAllUsersInLeague(int leagueId);
        void DeleteUserInLeague(UserInLeague userInLeague);
        Task<bool> NameExists(string name);
        Task<bool> Member(int leagueId, int userId);
        Task<League> GetLeague(int id);
        void Join(UserInLeague userInLeague);
        IEnumerable<League> GetLeagues();
        IEnumerable<User> GetUsersByLeague(int id);
        IEnumerable<League> GetLeaguesByUser(int id);
        IEnumerable<League> GetLeaguesWithoutUser(int id);
        Task<int> CountUsers(int id);
        int GetPosition(int userId, int leagueId);
        Task<UserInLeague> GetUserInLeague(int userId, int leagueId);
    }
}
