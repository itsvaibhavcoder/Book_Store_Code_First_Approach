using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using BCrypt.Net;
using BookStore.Models;
using BookStore.DTOs;
using BookStore.Repositories;


namespace BookStore.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        //{
        //    var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
        //    if (existingUser != null)
        //        return BadRequest("Email is already in use.");

        //    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        //    var user = new User
        //    {
        //        FirstName = userDto.FirstName,
        //        LastName = userDto.LastName,
        //        Email = userDto.Email,
        //        PasswordHash = hashedPassword
        //    };

        //    await _userRepository.RegisterUserAsync(user);

        //    return Ok(new { message = "User registered successfully!" });
        //}

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        //{
        //    var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
        //    if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        //        return Unauthorized("Invalid credentials.");

        //    var token = GenerateJwtToken(user);
        //    return Ok(new { token });
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existingUser != null)
                throw new ArgumentException("Email is already in use."); 

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PasswordHash = hashedPassword
            };

            await _userRepository.RegisterUserAsync(user);

            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials."); 

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }


        // Generate JWT Token
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
