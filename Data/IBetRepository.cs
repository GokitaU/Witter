using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Dtos;
using Witter.Models;

namespace Witter.Data
{
    public interface IBetRepository
    {
        void Place(Bet bet);
        Task<Bet> GetBet(int BetId);
        Task<bool> BetPlaced(int userId, int matchId);
        Task<IEnumerable<BetForClientDto>> GetBetsByUser(int userId);
        Task<IEnumerable<BetForClientDto>> GetPastBetsByUser(int userId);
        IEnumerable<Bet> GetBetsByMatch(int matchId);
        Task<Bet> GetBetsByUserAndMatch(int userId, int matchId);
    }
}
