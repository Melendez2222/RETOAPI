using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RETOAPI.Data;
using RETOAPI.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RETOAPI.Controllers
{
    [ApiController]
    [Route("Auth")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _conexionDB;

        public AuthController(IConfiguration configuration, AppDbContext conexionDB)
        {
            _configuration = configuration;
            _conexionDB = conexionDB;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                // Hashing the input credentials
                var hashedUsuario = HashString(login.Username);
                var hashedPassword = HashString(login.Password);

                // Using LINQ to find the user
                var user = await _conexionDB.Users
                                            .Where(u => u.UserUsername == hashedUsuario && u.UserPassword == hashedPassword)
                                            .FirstOrDefaultAsync();

                if (user != null)
                {
                    // Creating claims for the JWT token
                    var claims = new[]
                    {   
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    // Generating the JWT token
                    var keyString = _configuration["Jwt:Key"];
                    if (string.IsNullOrEmpty(keyString))
                    {
                        return StatusCode(500, new { message = "JWT key is not configured properly." });
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                        signingCredentials: creds);

                    // Returning the token and user ID
                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), id = user.UserId });
                }

                // Returning unauthorized if user not found
                return Unauthorized();
            }
            catch (Exception ex)
            {
                // Logging the exception and returning a 500 status code
                return StatusCode(500, new { message = "Ocurrió un error al obtener el usuario", error = ex.Message });
            }
        }
        private string HashString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
