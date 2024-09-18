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
    [Route("EMPLOYEE")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class EmployeeController:ControllerBase
    {
        private readonly AppDbContext _conexionDB;
        public EmployeeController(AppDbContext conexionDB)
        {
            _conexionDB = conexionDB;
        }
        [HttpGet("ListAll")]
        public async Task<ActionResult> GetAllEmployee()
        {
            try
            {
                var Employee = await (from u in _conexionDB.Users
                                    join ur in _conexionDB.UserRoles on u.UserId equals ur.UserId
                                    join r in _conexionDB.Rols on ur.RolId equals r.RolId
                                    where r.RolId != 2
                                    select new UserList
                                    {
                                        UserId = u.UserId,
                                        UserRucDni = u.UserRucDni,
                                        UserName = u.UserName,
                                        UserAddress = u.UserAddress,
                                        UserEmail = u.UserEmail,
                                        UserPhone = u.UserPhone,
                                        UserActive = u.UserActive,
                                        CreatedAt = u.CreatedAt,

                                    }).ToListAsync();
                return Ok(Employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("Create")]
        public async Task<ActionResult> CreateEmployee([FromBody] UserCreate employecreate)
        {
            try
            {
                var Employee = new Users
                {
                    UserRucDni = employecreate.UserRucDni,
                    UserName = employecreate.UserName,
                    UserAddress = employecreate.UserAddress,
                    UserEmail = employecreate.UserEmail,
                    UserPhone = employecreate.UserPhone,
                    UserUsername = HashString(employecreate.UserUsername),
                    UserPassword = HashString(employecreate.UserPassword),

                };
                var userRole = new UserRole
                {
                    UserId = Employee.UserId,
                    RolId = employecreate.Rolid,
                };
                _conexionDB.Users.Add(Employee);
                await _conexionDB.SaveChangesAsync();
                _conexionDB.UserRoles.Add(userRole);
                await _conexionDB.SaveChangesAsync();
                return Ok(Employee);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("Update")]
        public async Task<ActionResult> UpdateEmployee([FromBody] UserUpdate employeeUpdate)
        {
            try
            {
                var employee = await _conexionDB.Users.FindAsync(employeeUpdate.UserId);
                if (employee == null)
                {
                    return NotFound("User not found");
                }

                employee.UserRucDni = employeeUpdate.UserRucDni;
                employee.UserName = employeeUpdate.UserName;
                employee.UserAddress = employeeUpdate.UserAddress;
                employee.UserEmail = employeeUpdate.UserEmail;
                employee.UserPhone = employeeUpdate.UserPhone;

                _conexionDB.Users.Update(employee);
                await _conexionDB.SaveChangesAsync();

                var userRole = await _conexionDB.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == employeeUpdate.UserId);
                if (userRole != null)
                {
                    userRole.RolId = employeeUpdate.Rolid;
                    _conexionDB.UserRoles.Update(userRole);
                }
                else
                {
                    var newUserRole = new UserRole
                    {
                        UserId = employeeUpdate.UserId,
                        RolId = employeeUpdate.Rolid
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
