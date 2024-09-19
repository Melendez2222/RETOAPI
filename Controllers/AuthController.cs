using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RETOAPI.Data;
using RETOAPI.DTOs;
using RETOAPI.Services;
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
        private readonly ServiceCredentials _serviceCredentials;

        public AuthController(IConfiguration configuration, AppDbContext conexionDB, ServiceCredentials serviceCredentials)
        {
            _configuration = configuration;
            _conexionDB = conexionDB;
            _serviceCredentials = serviceCredentials;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            
            try
            {
                var hashedUsuario = _serviceCredentials.HashString(login.Username);
                var hashedPassword = _serviceCredentials.HashString(login.Password);

                var user = await _conexionDB.Users
                                            .Include(u => u.UserRols)
                                            .ThenInclude(ur => ur.Rols)
                                            .Where(u => u.UserUsername == hashedUsuario && u.UserPassword == hashedPassword)
                                            .FirstOrDefaultAsync();

                if (user != null)
                {
                    var userRole = user.UserRols.FirstOrDefault();
                    var roleId = userRole?.RolId ?? 2;
                    var roleName = roleId == 2 ? "" : "AdmPanel";

                    var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, roleId.ToString())
            };

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

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), id = user.UserId, role = roleName });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al obtener el usuario", error = ex.Message });
            }
        }
        
    }
}
