﻿using AutoMapper;
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
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _conexionDB;
        private readonly IMapper _mapper;
        public ClientController(AppDbContext conexionDB, IMapper mapper)
        {
            _conexionDB = conexionDB;
            _mapper = mapper;
        }
        [HttpGet("ListAll")]
        public async Task<ActionResult> GetAllClient()
        {
            try
            {
                var users = await _conexionDB.Users
                    .Where(u => u.UserRols.Any(ur => ur.RolId == 2))
                    .ToListAsync();

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
        public async Task<ActionResult> CreateEmployee([FromBody] UserCreate clientcreate)
        {
            try
            {
                var client = _mapper.Map<Users>(clientcreate);

                _conexionDB.Users.Add(client);
                await _conexionDB.SaveChangesAsync();

                var userRole = new UserRole
                {
                    UserId = client.UserId,
                    RolId = clientcreate.Rolid,
                };

                _conexionDB.UserRoles.Add(userRole);
                await _conexionDB.SaveChangesAsync();

                return Ok(client);
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

                _mapper.Map(clientUpdate, employee);

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

                var userRoles = await _conexionDB.UserRoles.Where(ur => ur.UserId == id).ToListAsync();
                if (userRoles.Any())
                {
                    _conexionDB.UserRoles.RemoveRange(userRoles);
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

    }
}
