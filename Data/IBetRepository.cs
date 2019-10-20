using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public interface IBetRepository
    {
        void Place(Bet bet);
        Task<Bet> GetBet(int BetId);
        Task<bool> BetPlaced(int userId, int matchId);
        IEnumerable<Bet> GetBetsByUser(int userId);
        IEnumerable<Bet> GetBetsByMatch(int matchId);
        Task<Bet> GetBetsByUserAndMatch(int userId, int matchId);
    }
}
