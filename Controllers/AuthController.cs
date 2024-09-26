using Microsoft.AspNetCore.Authorization;
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
using static RETOAPI.Services.ServiceCredentials;

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
                    var roleId = userRole?.idrelation ?? 2;
                    var roleName = roleId == 2 ? "" : "AdmPanel";

                    var token = _serviceCredentials.GetToken(user.UserName, roleId);

                    return Ok(new
                    {
                        token = token.Token,
                        expires = token.Expires,
                        id = user.UserId,
                        role = roleName
                    });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al obtener el usuario", error = ex.Message });
            }
        }
        [HttpGet("RefreshToken")]
        [Authorize]
        public async Task<IActionResult> tokenRefresh() { 
        
        }
        
    }
}
