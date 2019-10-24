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

        [HttpPut("ban/{id}")]
        public async Task<IActionResult> AddBan(int id, UserForBanDto userForBanDto)
        {
            var user = await userRepository.GetUser(id);

            if(userForBanDto.Ban == null && userForBanDto.PermanentBan == false)
            {
                return BadRequest("Something went wrong");
            }

            if (user.PermanentBan)
            {
                return BadRequest("User is already banned!");
            }

            user.PermanentBan = userForBanDto.PermanentBan;
            user.Ban = userForBanDto.Ban;

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

            if (user.Ban == null && user.PermanentBan == false)
            {
                return BadRequest("User is not banned!");
            }

            user.Ban = null;
            user.PermanentBan = false;

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

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await userRepository.GetUser(id);

            var userToReturn = mapper.Map<UserForAdminDto>(user);

            return Ok(userToReturn);
        }

        [AllowAnonymous]
        [HttpGet("users")]
        public async Task<IActionResult> GetUserList()
        {
            var users = userRepository.GetUsers();

            var usersToReturn = mapper.Map<IEnumerable<UserForAdminDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("users/admin")]
        public async Task<IActionResult> GetAdminUsersList()
        {
            var users = userRepository.GetAdminUsers();

            var usersToReturn = mapper.Map<IEnumerable<UserForAdminDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("users/banned")]
        public async Task<IActionResult> GetBannedUsersList()
        {
            var users = userRepository.GetBannedUsers();

            var usersToReturn = mapper.Map<IEnumerable<UserForAdminDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("users/notbanned")]
        public async Task<IActionResult> GetNotBannedUsersList()
        {
            var users = userRepository.GetNotBannedUsers();

            var usersToReturn = mapper.Map<IEnumerable<UserForAdminDto>>(users);

            return Ok(usersToReturn);
        }
    }
}