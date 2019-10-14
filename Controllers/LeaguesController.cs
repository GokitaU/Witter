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
    [Authorize(Roles="User, Admin")]
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
        public async Task<IActionResult> GetLeagues()
        {
            var leagues = leagueRepository.GetLeagues();

            return Ok(leagues);
        }

        [HttpGet("{id}", Name ="GetLeague")]
        public async Task<IActionResult> GetLeague(int id)
        {
            var league = await leagueRepository.GetLeague(id);

            return Ok(league);
        }

        [HttpGet("{id}/rank")]
        public async Task<IActionResult> GetRanking(int id)
        {
            var users = leagueRepository.GetUsersByLeague(id);

            return Ok(users);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetLeaguesByUser(int userId)
        {
            var leagues = leagueRepository.GetLeaguesByUser(userId);

            return Ok(leagues);
        }

        [HttpPost("{id}/join")]
        public async Task<IActionResult> JoinLeague(int id)
        {
            int loggedUserId;
            Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out loggedUserId);

            if(await leagueRepository.Member(id, loggedUserId))
            {
                return BadRequest("You are already a member of this league.");
            }

            var league = await leagueRepository.GetLeague(id);

            if(league == null)
            {
                return BadRequest("League not found.");
            }

            var userInLeague = new UserInLeague
            {
                User = await userRepository.GetUser(loggedUserId),
                League = league
            };

            leagueRepository.Join(userInLeague);

            if(await dataContext.Commit())
            {
                return Ok();
            }

            return BadRequest("Something went wrong. Try again later.");
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLeague([FromForm]LeagueForCreateDto league)
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