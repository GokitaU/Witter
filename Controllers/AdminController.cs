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

namespace Witter.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext dataContext;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public AdminController(DataContext dataContext, IUserRepository userRepository, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpPut("permban/{id}")]
        public async Task<IActionResult> AddPermBan(int id)
        {
            var user = await userRepository.GetUser(id);

            if (user.PermanentBan)
            {
                return BadRequest("User is already banned!");
            }

            user.PermanentBan = true;

            if (await dataContext.Commit())
            {
                return NoContent();
            }

            throw new Exception($"Banning user {id} failed on save.");
        }

        [HttpPut("unpermban/{id}")]
        public async Task<IActionResult> RemovePermBan(int id)
        {
            var user = await userRepository.GetUser(id);

            if (!user.PermanentBan)
            {
                return BadRequest("User is not banned!");
            }

            user.PermanentBan = false;

            if (await dataContext.Commit())
            {
                return NoContent();
            }

            throw new Exception($"Unbanning user {id} failed on save.");
        }

        [HttpPut("ban/{id}")]
        public async Task<IActionResult> AddBan(int id, [FromForm]DateTime date)
        {
            var user = await userRepository.GetUser(id);

            if (user.PermanentBan)
            {
                return BadRequest("User is already banned!");
            }

            user.Ban = date;

            if (await dataContext.Commit())
            {
                return NoContent();
            }

            throw new Exception($"Banning user {id} failed on save.");
        }

        [HttpPut("unban/{id}")]
        public async Task<IActionResult> RemoveBan(int id)
        {
            var user = await userRepository.GetUser(id);

            if (user.Ban == null)
            {
                return BadRequest("User is not banned!");
            }

            user.Ban = null;

            if (await dataContext.Commit())
            {
                return NoContent();
            }

            throw new Exception($"Unbanning user {id} failed on save.");
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await userRepository.GetUser(id);

            userRepository.Delete(user);

            if(await dataContext.Commit())
            {
                return Ok();
            }

            return BadRequest("Could not delete an user!");
        }

        [HttpPut("rights/{id}")]
        public async Task<IActionResult> AdminRights(int id)
        {
            var user = await userRepository.GetUser(id);

            user.IsAdmin = !user.IsAdmin;

            if(await dataContext.Commit())
            {
                return Ok();
            }

            return BadRequest($"Could not grant admin rights to the user {id}");
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersList()
        {
            var users = userRepository.GetUsers();

            var usersToReturn = mapper.Map<IEnumerable<UserForAdminDto>>(users);

            return Ok(usersToReturn);
        }
    }
}