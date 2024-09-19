using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RETOAPI.Data;
using RETOAPI.DTOs;
using RETOAPI.Models;
using RETOAPI.Services;

namespace RETOAPI.Controllers
{
    public class UsersConrtroller:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _conexionDB;
        private readonly ServiceCredentials _serviceCredentials;
        public UsersConrtroller(AppDbContext conexionDB, IMapper mapper, ServiceCredentials serviceCredentials)
        {
            _conexionDB = conexionDB;
            _mapper = mapper;
            _serviceCredentials = serviceCredentials;
        }
        [HttpGet("ListAll")]
        public async Task<ActionResult> GetAllClient()
        {
            try
            {
                var users = await _conexionDB.Users
                    .Include(u => u.UserRols)
                    .ThenInclude(ur => ur.Rols).ToListAsync();

                var clientList = _mapper.Map<List<UserList>>(users);

                return Ok(clientList);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("Create")]
        public async Task<ActionResult> CreateEmployee([FromBody] UserCreate userCreate)
        {
            try
            {
                var Usuario = _mapper.Map<Users>(userCreate);

                _conexionDB.Users.Add(Usuario);
                await _conexionDB.SaveChangesAsync();

                var userRole = new UserRole
                {
                    UserId = Usuario.UserId,
                    RolId = userCreate.Rolid,
                };
                _conexionDB.UserRoles.Add(userRole);
                await _conexionDB.SaveChangesAsync();

                return Ok(Usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("Update")]
        public async Task<ActionResult> UpdateEmployee([FromBody] UserUpdate userUpdate)
        {
            try
            {
                var Usuario = await _conexionDB.Users.FindAsync(userUpdate.UserId);
                if (Usuario == null)
                {
                    return NotFound("User not found");
                }

                // Mapea los cambios del objeto UserUpdate al objeto Users existente
                _mapper.Map(userUpdate, Usuario);

                _conexionDB.Users.Update(Usuario);
                await _conexionDB.SaveChangesAsync();

                var userRole = await _conexionDB.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userUpdate.UserId);
                if (userRole != null)
                {
                    userRole.RolId = userUpdate.Rolid;
                    _conexionDB.UserRoles.Update(userRole);
                }
                else
                {
                    var newUserRole = new UserRole
                    {
                        UserId = userUpdate.UserId,
                        RolId = userUpdate.Rolid
                    };
                    _conexionDB.UserRoles.Add(newUserRole);
                }

                await _conexionDB.SaveChangesAsync();

                return Ok(Usuario);
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
                var employee = await _conexionDB.Users
                    .Include(u => u.UserRols)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (employee == null)
                {
                    return NotFound("User not found");
                }

                // Eliminar roles asociados
                _conexionDB.UserRoles.RemoveRange(employee.UserRols);

                // Eliminar el usuario
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
    }
}
