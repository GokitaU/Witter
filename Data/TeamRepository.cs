using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DataContext dataContext;

        public TeamRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async void Add(Team team)
        {
            await dataContext.AddAsync(team);
        }

        public void Delete(Team team)
        {
            dataContext.Remove(team);
        }

        public async Task<bool> Exists(int id)
        {
            return await dataContext.Teams.AnyAsync(t => t.Id == id);
        }

        public async Task<Team> GetTeam(int id)
        {
            return await dataContext.Teams.FirstOrDefaultAsync(t => t.Id == id);
        }

        public IEnumerable<Team> GetTeams()
        {
            return dataContext.Teams;
        }
    }
}
