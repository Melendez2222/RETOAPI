using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RETOAPI.Data;
using RETOAPI.DTOs;
using RETOAPI.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RETOAPI.Controllers
{
    [ApiController]
    [Route("CLIENT")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ClientController:ControllerBase
    {
        private readonly AppDbContext _conexionDB;
        public ClientController(AppDbContext conexionDB)
        {
            _conexionDB = conexionDB;
        }
        [HttpGet("ListAll")]
        public async Task<ActionResult> GetAllClient()
        {
            try
            {
                var Client = await (from u in _conexionDB.Users
                                      join ur in _conexionDB.UserRoles on u.UserId equals ur.UserId
                                      join r in _conexionDB.Rols on ur.RolId equals r.RolId
                                      where r.RolId == 2
                                      select new UserList
                                      {
                                          UserId = u.UserId,
                                          UserRucDni=u.UserRucDni,
                                          UserName=u.UserName,
                                          UserAddress=u.UserAddress,
                                          UserEmail=u.UserEmail,
                                          UserPhone=u.UserPhone,
                                          UserActive=u.UserActive,
                                          CreatedAt=u.CreatedAt,

                                      }).ToListAsync();
                return Ok(Client);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("Create")]
        public async Task<ActionResult> CreateEmployee([FromBody] UserCreate clientcreate)
        {
            try
            {
                var Client = new Users
                {
                    UserRucDni = clientcreate.UserRucDni,
                    UserName = clientcreate.UserName,
                    UserAddress = clientcreate.UserAddress,
                    UserEmail = clientcreate.UserEmail,
                    UserPhone = clientcreate.UserPhone,
                    UserUsername = HashString(clientcreate.UserUsername),
                    UserPassword = HashString(clientcreate.UserPassword),

                };
                var userRole = new UserRole
                {
                    UserId = Client.UserId,
                    RolId = clientcreate.Rolid,
                };
                _conexionDB.Users.Add(Client);
                await _conexionDB.SaveChangesAsync();
                _conexionDB.UserRoles.Add(userRole);
                await _conexionDB.SaveChangesAsync();
                return Ok(Client);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("Update")]
        public async Task<ActionResult> UpdateEmployee([FromBody] UserUpdate clientUpdate)
        {
            try
            {
                var employee = await _conexionDB.Users.FindAsync(clientUpdate.UserId);
                if (employee == null)
                {
                    return NotFound("User not found");
                }

                employee.UserRucDni = clientUpdate.UserRucDni;
                employee.UserName = clientUpdate.UserName;
                employee.UserAddress = clientUpdate.UserAddress;
                employee.UserEmail = clientUpdate.UserEmail;
                employee.UserPhone = clientUpdate.UserPhone;

                _conexionDB.Users.Update(employee);
                await _conexionDB.SaveChangesAsync();

                var userRole = await _conexionDB.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == clientUpdate.UserId);
                if (userRole != null)
                {
                    userRole.RolId = clientUpdate.Rolid;
                    _conexionDB.UserRoles.Update(userRole);
                }
                else
                {
                    var newUserRole = new UserRole
                    {
                        UserId = clientUpdate.UserId,
                        RolId = clientUpdate.Rolid
                    };
                    _conexionDB.UserRoles.Add(newUserRole);
                }

                await _conexionDB.SaveChangesAsync();

                return Ok(employee);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _conexionDB.Users.FindAsync(id);
                if (employee == null)
                {
                    return NotFound("User not found");
                }

                var userRole = await _conexionDB.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == id);
                if (userRole != null)
                {
                    _conexionDB.UserRoles.Remove(userRole);
                }

                _conexionDB.Users.Remove(employee);
                await _conexionDB.SaveChangesAsync();

                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
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
