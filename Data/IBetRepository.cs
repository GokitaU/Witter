using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Dtos;
using Witter.Helpers;
using Witter.Models;

namespace Witter.Data
{
    public interface IBetRepository
    {
        void Place(Bet bet);
        Task<Bet> GetBet(int BetId);
        Task<bool> BetPlaced(int userId, int matchId);
        Task<PagedList<Bet>> GetBetsByUser(int userId, BetParams betParams);
        Task<PagedList<Bet>> GetPastBetsByUser(int userId, BetParams betParams);
        IEnumerable<Bet> GetBetsByMatch(int matchId);
        Task<Bet> GetBetsByUserAndMatch(int userId, int matchId);
    }
}
