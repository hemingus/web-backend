using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using web_backend.DbContexts;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using web_backend.DbContexts;
using web_backend.Models.DTOs;

namespace web_backend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IDbContextFactory<CosmosContext> _contextFactory;
        private readonly IConfiguration _configuration;

        public AuthController(IDbContextFactory<CosmosContext> contextFactory, IConfiguration configuration)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            await using var ctx = _contextFactory.CreateDbContext();

            var existing = await ctx.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (existing != null)
            {
                return Conflict("A user with that email already exists.");
            }

            var hasher = new PasswordHasher<object>();
            var passwordHash = hasher.HashPassword(null, dto.Password);

            var user = new Entities.User(dto.Email, dto.Name, passwordHash);

            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var userDto = new UserDto(user.Id, user.Email, user.Name, user.CreatedAt, user.UpdatedAt);
            return CreatedAtAction(null, userDto);
        }

        [HttpPost("login")]
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

            var hasher = new PasswordHasher<object>();
            var verify = hasher.VerifyHashedPassword(null, user.PasswordHash, dto.Password);
            if (verify == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid credentials.");
            }

            // generate JWT
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                // If JWT is not configured the server cannot issue tokens
                return StatusCode(500, "JWT signing key is not configured.");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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