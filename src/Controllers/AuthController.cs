using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_backend.DbContexts;
using web_backend.Entities;
using web_backend.Models.DTOs;

namespace web_backend.Controllers
{
    [ApiController]
    [Route("auth")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IDbContextFactory<CosmosContext> _contextFactory;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(
            IDbContextFactory<CosmosContext> contextFactory,
            IConfiguration configuration,
            IPasswordHasher<User> passwordHasher)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        // Allow anonymous because global fallback policy will otherwise require auth
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Email, name and password are required.");
            }

            await using var ctx = _contextFactory.CreateDbContext();

            var existing = await ctx.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (existing != null)
            {
                return Conflict("A user with that email already exists.");
            }

            var user = new User(dto.Email, dto.Name, string.Empty);
            var passwordHash = _passwordHasher.HashPassword(user, dto.Password);
            user.SetPasswordHash(passwordHash);

            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var userDto = new UserDto(user.Id, user.Email, user.Name, user.CreatedAt, user.UpdatedAt);
            return Ok(userDto);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            await using var ctx = _contextFactory.CreateDbContext();

            var user = await ctx.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verify == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid credentials.");
            }

            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                return StatusCode(500, "JWT signing key is not configured.");
            }

            var claims = new List<Claim>
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresInMinutes = 60;
            if (int.TryParse(_configuration["Jwt:ExpiryMinutes"], out var parsedMinutes))
            {
                expiresInMinutes = parsedMinutes;
            }

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            var result = new AuthResultDto(tokenString, tokenDescriptor.ValidTo, new UserDto(user.Id, user.Email, user.Name, user.CreatedAt, user.UpdatedAt));

            // update last login
            user.RegisterLogin();
            ctx.Users.Update(user);
            await ctx.SaveChangesAsync();

            return Ok(result);
        }
    }
}