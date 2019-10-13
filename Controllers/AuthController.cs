using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Witter.Data;
using Witter.Dtos;
using Witter.Models;

namespace Witter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly DataContext dataContext;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration, IMapper mapper, IUserRepository userRepository, DataContext dataContext)
        {
            this.authRepository = authRepository;
            this.configuration = configuration;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.dataContext = dataContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            if (await authRepository.UserExists(userForRegisterDto.Username))
            {
                return BadRequest("Username already exists");
            }

            User user = mapper.Map<User>(userForRegisterDto);
            await authRepository.Register(user, userForRegisterDto.Password);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForRegisterDto userForRegisterDto)
        {
            var user = await authRepository.Login(userForRegisterDto.Username, userForRegisterDto.Password);

            if(user == null)
            {
                return Unauthorized();
            }

            var role = "User";

            if (user.IsAdmin)
            {
                role = "Admin";
            }

            if(user.PermanentBan || user.Ban != null)
            {
                role = "Banned";
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userToReturn = mapper.Map<UserForReturnDto>(user);

            return Ok(new {
                token = tokenHandler.WriteToken(token),
                userToReturn
            });
        }

        [Authorize(Roles="User, Admin")]
        [HttpPut("password/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, UserForEditDto userForEditDto)
        {
            int loggedUserId;
            Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out loggedUserId);

            if(userId != loggedUserId)
            {
                return BadRequest("You cannot modify another user's profile.");
            }

            var user = await userRepository.GetUser(userId);

            user = await authRepository.ChangePassword(user, userForEditDto.Password);

            if(await dataContext.Commit())
            {
                return NoContent();
            }

            throw new Exception($"Changing password failed on save.");
        }
    }
}