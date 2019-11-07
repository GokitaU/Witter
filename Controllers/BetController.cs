using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Witter.Data;
using Witter.Dtos;
using Witter.Models;

namespace Witter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IBetRepository betRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IUserRepository userRepository;
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public BetController(IBetRepository betRepository, IMatchRepository matchRepository, IUserRepository userRepository, DataContext dataContext, IMapper mapper)
        {
            this.betRepository = betRepository;
            this.matchRepository = matchRepository;
            this.userRepository = userRepository;
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("place")]
        public async Task<IActionResult> PlaceBet(BetForPlaceDto betForPlace)
        {
            int userId;
            Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId);

            var match = await matchRepository.GetMatch(betForPlace.MatchId);
            float odds;

            if (match.Date < DateTime.Now)
            {
                return BadRequest("You cannot bet on a past match.");
            }

            if(await betRepository.BetPlaced(userId, match.Id))
            {
                return BadRequest("You already placed a bet on this match!");
            }

            if (betForPlace.Prediction == 0)
            {
                odds = match.DrawOdds;
            }
            else if(betForPlace.Prediction == 1)
            {
                odds = match.TeamAOdds;
            }
            else if (betForPlace.Prediction == 2)
            {
                odds = match.TeamBOdds;
            }
            else
            {
                return BadRequest("Something went wrong. Try again.");
            }

            var bet = new Bet
            {
                MatchId = match.Id,
                Prediction = betForPlace.Prediction,
                UserId = userId,
                Odds = odds
            };

            betRepository.Place(bet);

            if(await dataContext.Commit())
            {
                return CreatedAtRoute("GetBet", new { id = bet.Id }, bet);
            }

            return BadRequest("Could not place bet.");
        }

        [HttpGet("{id}", Name = "GetBet")]
        public async Task<IActionResult> GetBet(int id)
        {
            var bet = await betRepository.GetBet(id);

            if(bet == null)
            {
                return NotFound();
            }

            return Ok(bet);
    }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBetsByUser(int userId)
        {
            var bets = await betRepository.GetBetsByUser(userId);

            return Ok(bets);
        }

        [HttpGet("user/{userId}/past")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPastBetsByUser(int userId)
        {
            var bets = await betRepository.GetPastBetsByUser(userId);

            return Ok(bets);
        }

        [HttpGet("user/match/{matchId}")]
        public async Task<IActionResult> GetBetsByUserAndMatch(int matchId)
        {
            int userId;
            Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId);

            var bet = await betRepository.GetBetsByUserAndMatch(userId, matchId);

            return Ok(bet);
        }

        [HttpGet("match/{matchId}")]
        public async Task<IActionResult> GetBetsByMatch(int matchId)
        {
            var bets = betRepository.GetBetsByMatch(matchId);

            return Ok(bets);
        }
    }
}