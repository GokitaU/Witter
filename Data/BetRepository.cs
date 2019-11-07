using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Dtos;
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

        public async Task<IEnumerable<BetForClientDto>> GetBetsByUser(int userId)
        {
            var bets = dataContext.Bets.Where(b => b.UserId == userId).OrderByDescending(b => b.Id);

            var betsToReturn = mapper.Map<IEnumerable<BetForClientDto>>(bets);

            foreach (var b in bets.Zip(betsToReturn, Tuple.Create))
            {
                //temporary solution
                b.Item2.Match = await matchRepository.GetMatch(b.Item1.MatchId);
            }

            betsToReturn = betsToReturn.OrderByDescending(b => b.Match.Date);

            return betsToReturn;
        }

        public async Task<Bet> GetBetsByUserAndMatch(int userId, int matchId)
        {
            return await dataContext.Bets.FirstOrDefaultAsync(b => (b.UserId == userId && b.MatchId == matchId));
        }

        public async Task<IEnumerable<BetForClientDto>> GetPastBetsByUser(int userId)
        {
            var bets = dataContext.Bets.Where(b => b.UserId == userId).OrderByDescending(b => b.Id);

            var betsToReturn = mapper.Map<IEnumerable<BetForClientDto>>(bets);

            foreach (var b in bets.Zip(betsToReturn, Tuple.Create))
            {
                //temporary solution
                b.Item2.Match = await matchRepository.GetMatch(b.Item1.MatchId);
            }

            betsToReturn = betsToReturn.Where(b => b.Match.Date < DateTime.Now).OrderByDescending(b => b.Match.Date);
            return betsToReturn;
        }

        public async void Place(Bet bet)
        {
            await dataContext.Bets.AddAsync(bet);
        }
    }
}
