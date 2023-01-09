using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Infrastructure.Data;
using Infrastructure.Identity.Entities.Users;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers.Identity
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, DatabaseContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        [HttpPost("addUser")]
        public async Task<ActionResult<AdminUser>> Register(UserRegisterDto user)
        {
            // check if the user with the same username exist
            var existingUser = await _context.AdminUsers.Where(u => u.Username == user.Username).FirstOrDefaultAsync();
            
            if (existingUser != null)
            {
                return BadRequest("User with this username exist.");
            }

            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
        
            AdminUser newUser = new AdminUser
            {
                Name = user.Name,
                Surname = user.Surname,
                Username = user.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RefreshToken = null
            };

            _context.AdminUsers.Add(newUser);
            await _context.SaveChangesAsync();


            return Ok(newUser);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {
            // check if the user with the same username exist
            var existingUser = await _context.AdminUsers.
                Where(u => u.Username == request.Username).
                Include(r => r.RefreshToken).
                FirstOrDefaultAsync();

            if (existingUser == null)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, existingUser.PasswordHash, existingUser.PasswordSalt))
            { 
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(existingUser);
            
            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);
            
            existingUser.TokenCreated = DateTime.Now;
            existingUser.TokenExpires = DateTime.Now.AddDays(1);
            
            if (existingUser.RefreshToken == null )
            {
                var userRefreshToken = new RefreshToken()
                {
                    AdminUserId = existingUser.Id,
                    Token = refreshToken.Token,
                    Expires = refreshToken.Expires,
                    AdminUser = existingUser
                };

                existingUser.RefreshToken = userRefreshToken;
                await _context.RefreshTokens.AddAsync(userRefreshToken);
                await _context.SaveChangesAsync();
            }

            if (existingUser.RefreshToken != null)
            {
                var refreshUserRefreshToken = await _context.RefreshTokens.Where(r => r.AdminUserId == existingUser.Id)
                                .FirstOrDefaultAsync();
                if (refreshUserRefreshToken != null)
                {
                    refreshUserRefreshToken.Token = refreshToken.Token;
                    refreshUserRefreshToken.Expires = refreshToken.Expires;
                }
                await _context.SaveChangesAsync();
            }
            
            await _context.SaveChangesAsync();
            
            return Ok(token);
        }
        
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken(UserRefreshTokenDto user)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var existingUser = await _context.AdminUsers.Where(u => u.Username == user.Username).Include(r => r.RefreshToken).FirstOrDefaultAsync();

            if (existingUser == null)
            {
                return Unauthorized("User not found.");
            }

            if (existingUser.RefreshToken == null)
            {
                return Unauthorized("User not authorized");
            }

            if (existingUser.RefreshToken.Token == null)
            {
                return Unauthorized("User not authorized");
            }

            if (!existingUser.RefreshToken.Token.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if(existingUser.RefreshToken.Expires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }
            
            string token = CreateToken(existingUser);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            existingUser.RefreshToken.Token = newRefreshToken.Token;
            existingUser.RefreshToken.Expires = newRefreshToken.Expires;

            await _context.SaveChangesAsync();
            
            return Ok(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        
        private string CreateToken(AdminUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
        
    }
    
}
