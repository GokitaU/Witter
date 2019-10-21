using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository teamRepository;
        private readonly IMatchRepository matchRepository;
        private readonly DataContext dataContext;

        public TeamsController(ITeamRepository teamRepository, IMatchRepository matchRepository, DataContext dataContext)
        {
            this.teamRepository = teamRepository;
            this.matchRepository = matchRepository;
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            var teams = teamRepository.GetTeams();

            return Ok(teams);
        }

        [HttpGet("{id}", Name="GetTeam")]
        public async Task<IActionResult> GetTeam(int id)
        {
            var team = await teamRepository.GetTeam(id);

            if(team == null)
            {
                return NotFound();
            }

            var teamToReturn = new TeamForDetailedDto
            {
                Id = team.Id,
                Coach = team.Coach,
                Name = team.Name,
                PhotoUrl = team.PhotoUrl,
                Matches = matchRepository.GetTeamsMatches(id)
            };

            return Ok(teamToReturn);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddTeam([FromForm]Team team)
        {
            teamRepository.Add(team);

            if(await dataContext.Commit())
            {
                return CreatedAtRoute("GetTeam", new { id = team.Id }, team);
            }

            return BadRequest("Could not add team.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var teamToDelete = await teamRepository.GetTeam(id);

            if(teamToDelete == null)
            {
                return BadRequest("Team does not exist.");
            }

            teamRepository.Delete(teamToDelete);

            if(await dataContext.Commit())
            {
                return Ok();
            }

            return BadRequest("Could not delete team.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromForm]Team team)
        {
            var teamToEdit = await teamRepository.GetTeam(id);

            teamToEdit.Name = team.Name;
            teamToEdit.Coach = team.Coach;
            teamToEdit.PhotoUrl = team.PhotoUrl;

            if(await dataContext.Commit())
            {
                return NoContent();
            }

            throw new Exception($"Updating team {id} failed on save.");
        }
    }
}