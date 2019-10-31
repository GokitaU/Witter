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
    [Authorize(Roles = "User, Admin")]
    public class LeaguesController : ControllerBase
    {
        private readonly DataContext dataContext;
        private readonly ILeagueRepository leagueRepository;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public LeaguesController(DataContext dataContext, ILeagueRepository leagueRepository, IMapper mapper, IUserRepository userRepository)
        {
            this.dataContext = dataContext;
            this.leagueRepository = leagueRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLeagues()
        {
            var leagues = leagueRepository.GetLeagues();

            var leaguesToReturn = mapper.Map<IEnumerable<LeagueForListDto>>(leagues);

            foreach (var l in leagues.Zip(leaguesToReturn, Tuple.Create))
            {
                //temporary solution
                l.Item2.UserCount = await leagueRepository.CountUsers(l.Item1.Id);
            }

            return Ok(leaguesToReturn);
        }

        [HttpGet("{id}", Name = "GetLeague")]
        public async Task<IActionResult> GetLeague(int id)
        {
            var league = await leagueRepository.GetLeague(id);

            if(league == null)
            {
                return NotFound();
            }

            var leagueToReturn = mapper.Map<LeagueForListDto>(league);
            leagueToReturn.UserCount = await leagueRepository.CountUsers(league.Id);

            return Ok(leagueToReturn);
        }

        [HttpGet("{id}/rank")]
        public async Task<IActionResult> GetRanking(int id)
        {
            var users = leagueRepository.GetUsersByLeague(id);

            var usersToReturn = mapper.Map<IEnumerable<UserForReturnDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLeaguesByUser(int userId)
        {
            var leagues = leagueRepository.GetLeaguesByUser(userId);

            var leaguesToReturn = mapper.Map<IEnumerable<LeagueForListDto>>(leagues);

            foreach (var l in leagues.Zip(leaguesToReturn, Tuple.Create))
            {
                //temporary solution
                l.Item2.UserCount = await leagueRepository.CountUsers(l.Item1.Id);
                l.Item2.Position = leagueRepository.GetPosition(userId, l.Item1.Id);
            }

            leaguesToReturn = leaguesToReturn.OrderByDescending(l => l.UserCount);

            return Ok(leaguesToReturn);
        }

        [HttpGet("user/not/{userId}")]
        public async Task<IActionResult> GetLeaguesWithoutUser(int userId)
        {
            var leagues = leagueRepository.GetLeaguesWithoutUser(userId);

            var leaguesToReturn = mapper.Map<IEnumerable<LeagueForListDto>>(leagues);

            foreach (var l in leagues.Zip(leaguesToReturn, Tuple.Create))
            {
                //temporary solution
                l.Item2.UserCount = await leagueRepository.CountUsers(l.Item1.Id);
            }

            leaguesToReturn = leaguesToReturn.OrderByDescending(l => l.UserCount);

            return Ok(leaguesToReturn);
        }

        [HttpPost("{id}/join")]
        public async Task<IActionResult> JoinLeague(int id)
        {
            int loggedUserId;
            Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out loggedUserId);

            if (await leagueRepository.Member(id, loggedUserId))
            {
                return BadRequest("You are already a member of this league.");
            }

            var league = await leagueRepository.GetLeague(id);

            if (league == null)
            {
                return BadRequest("League not found.");
            }

            var userInLeague = new UserInLeague
            {
                User = await userRepository.GetUser(loggedUserId),
                League = league
            };

            leagueRepository.Join(userInLeague);

            if (await dataContext.Commit())
            {
                return Ok();
            }

            return BadRequest("Something went wrong. Try again later.");
        }

        [HttpPost("{id}/leave")]
        public async Task<IActionResult> LeaveLeague(int id)
        {
            int loggedUserId;
            Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out loggedUserId);

            var userInLeague = await leagueRepository.GetUserInLeague(loggedUserId, id);

            if(userInLeague == null)
            {
                return BadRequest("You are not a member of this league.");
            }

            leagueRepository.DeleteUserInLeague(userInLeague);

            if(await dataContext.Commit())
            {
                return Ok();
            }

            return BadRequest("Could not leave league");
        }

    [HttpPost("add")]
        public async Task<IActionResult> AddLeague(LeagueForCreateDto league)
        {
            if (await leagueRepository.NameExists(league.Name))
            {
                return BadRequest("League already exists. Choose different name.");
            }

            int loggedUserId;
            Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out loggedUserId);

            var leagueToCreate = mapper.Map<League>(league);
            leagueToCreate.Admin = await userRepository.GetUser(loggedUserId);

            leagueRepository.Create(leagueToCreate);

            var userInLeague = new UserInLeague
            {
                User = leagueToCreate.Admin,
                League = leagueToCreate
            };

            leagueRepository.Join(userInLeague);

            if (await dataContext.Commit())
            {
                return CreatedAtRoute("GetLeague", new { id = leagueToCreate.Id }, leagueToCreate);
            }

            return BadRequest("Could not add league");
        }

        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            int loggedUserId;
            Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out loggedUserId);

            var league = await leagueRepository.GetLeague(id);

            if(league.Admin.Id != loggedUserId)
            {
                return BadRequest("You are not an admin of this league!");
            }

            leagueRepository.Delete(league);

            if(await dataContext.Commit())
            {
                return Ok();
            }

            return BadRequest("Could not delete league");
        }
    }
}