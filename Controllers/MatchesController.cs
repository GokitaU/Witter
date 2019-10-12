using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MatchesController : ControllerBase
    {
        private readonly IMatchRepository matchRepository;
        private readonly ITeamRepository teamRepository;
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public MatchesController(IMatchRepository matchRepository, ITeamRepository teamRepository, DataContext dataContext, IMapper mapper)
        {
            this.matchRepository = matchRepository;
            this.teamRepository = teamRepository;
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMatches()
        {
            var matches = matchRepository.GetMatches(false, false);

            return Ok(matches);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingMatches()
        {
            var matches = matchRepository.GetMatches(true, false);

            return Ok(matches);
        }

        [HttpGet("ended")]
        public async Task<IActionResult> GetEndedMatches()
        {
            var matches = matchRepository.GetMatches(false, true);

            return Ok(matches);
        }

        [HttpGet("{id}", Name="GetMatch")]
        public async Task<IActionResult> GetMatch(int id)
        {
            var match = await matchRepository.GetMatch(id);

            return Ok(match);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddMatch([FromForm]MatchForCreateDto match)
        {
            if(match.TeamAId == match.TeamBId)
            {
                return BadRequest("You picked the same team twice.");
            }

            if(await teamRepository.Exists(match.TeamAId) && await teamRepository.Exists(match.TeamBId))
            {
                var matchToCreate = mapper.Map<Match>(match);
                matchToCreate.TeamA = await teamRepository.GetTeam(match.TeamAId);
                matchToCreate.TeamB = await teamRepository.GetTeam(match.TeamBId);

                matchRepository.Add(matchToCreate);

                if (await dataContext.Commit())
                {
                    return CreatedAtRoute("GetMatch", new { id = matchToCreate.Id }, matchToCreate);
                }

            }

            return BadRequest("Could not add match.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await matchRepository.GetMatch(id);

            if(match == null)
            {
                return BadRequest("Match does not exist.");
            }

            matchRepository.Delete(match);

            if (await dataContext.Commit())
            {
                return Ok();
            }

            return BadRequest($"Could not delete match {id}");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMatch(int id, [FromForm]MatchForCreateDto match)
        {
            var matchToUpdate = await matchRepository.GetMatch(id);

            matchToUpdate.TeamA = await teamRepository.GetTeam(match.TeamAId);
            matchToUpdate.TeamB = await teamRepository.GetTeam(match.TeamBId);
            matchToUpdate.Date = match.Date;
            matchToUpdate.TeamAOdds = match.TeamAOdds;
            matchToUpdate.TeamBOdds = match.TeamBOdds;
            matchToUpdate.DrawOdds = match.DrawOdds;

            if(await dataContext.Commit())
            {
                return NoContent();
            }

            throw new Exception($"Updating match {id} failed on save.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("score/{id}")]
        public async Task<IActionResult> updateScore(int id, [FromForm]ScoreForUpdateDto score)
        {
            var match = await matchRepository.GetMatch(id);

            if(match == null)
            {
                return BadRequest("Match does not exist.");
            }

            if(match.Date >= DateTime.Now)
            {
                return BadRequest("You cannot assign score to a future match.");
            }

            var scoreToUpdate = new Score
            {
                MatchId = id,
                TeamAGoals = score.TeamAGoals,
                TeamBGoals = score.TeamBGoals
            };

            match.Score = scoreToUpdate;

            if(await dataContext.Commit())
            {
                return NoContent();
            }

            throw new Exception($"Updating score of match {id} failed on save.");
        }
    }
}