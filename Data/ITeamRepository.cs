using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public interface ITeamRepository
    {
        void Add(Team team);
        void Delete(Team team);
        Task<Team> GetTeam(int id);
        IEnumerable<Team> GetTeams();
        Task<bool> Exists(int id);
    }
}
