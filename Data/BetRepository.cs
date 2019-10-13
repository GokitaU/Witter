using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public class BetRepository : IBetRepository
    {
        private readonly DataContext dataContext;

        public BetRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<bool> BetPlaced(int userId, int matchId)
        {
            return await dataContext.Bets.AnyAsync(b => (b.UserId == userId) && (b.MatchId == matchId));
        }

        public async Task<Bet> GetBet(int BetId)
        {
            return await dataContext.Bets.FirstOrDefaultAsync(b => b.Id == BetId);
        }

        public IEnumerable<Bet> GetBetsByMatch(int matchId)
        {
            return dataContext.Bets.Where(b => b.MatchId == matchId);
        }

        public IEnumerable<Bet> GetBetsByUser(int userId)
        {
            if(upcoming && ended)
            {
                return dataContext.Bets.Where(b => b.UserId == userId);
            }
        }

        public async void Place(Bet bet)
        {
            await dataContext.Bets.AddAsync(bet);
        }
    }
}
