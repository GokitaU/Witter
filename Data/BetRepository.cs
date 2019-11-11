using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Dtos;
using Witter.Helpers;
using Witter.Models;

namespace Witter.Data
{
    public class BetRepository : IBetRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IMatchRepository matchRepository;

        public BetRepository(DataContext dataContext, IMapper mapper, IMatchRepository matchRepository)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.matchRepository = matchRepository;
        }

        public async Task<bool> BetPlaced(int userId, int matchId)
        {
            return await dataContext.Bets.AnyAsync(b => (b.UserId == userId) && (b.Match.Id == matchId));
        }

        public async Task<Bet> GetBet(int BetId)
        {
            return await dataContext.Bets.FirstOrDefaultAsync(b => b.Id == BetId);
        }

        public IEnumerable<Bet> GetBetsByMatch(int matchId)
        {
            return dataContext.Bets.Where(b => b.Match.Id == matchId);
        }

        public async Task<Bet> GetBetsByUserAndMatch(int userId, int matchId)
        {
            return await dataContext.Bets.FirstOrDefaultAsync(b => (b.UserId == userId && b.Match.Id == matchId));
        }

        public async Task<PagedList<Bet>> GetBetsByUser(int userId, BetParams betParams)
        {
            var bets = dataContext.Bets.Include(b => b.Match).Include(b => b.Match.TeamA).Include(b => b.Match.TeamB).Where(b => b.UserId == userId).OrderByDescending(b => b.Match.Date);

            return await PagedList<Bet>.CreateAsync(bets, betParams.PageNumber, betParams.PageSize);
        }

        public async Task<PagedList<Bet>> GetPastBetsByUser(int userId, BetParams betParams)
        {
            var bets = dataContext.Bets.Include(b => b.Match).Include(b => b.Match.TeamA).Include(b => b.Match.TeamB).Where(b => b.UserId == userId && b.Match.Date < DateTime.Now).OrderByDescending(b => b.Id);
            return await PagedList<Bet>.CreateAsync(bets, betParams.PageNumber, betParams.PageSize);
        }

        public async void Place(Bet bet)
        {
            await dataContext.Bets.AddAsync(bet);
        }
    }
}
