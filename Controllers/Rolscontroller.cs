using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RETOAPI.Data;
using RETOAPI.DTOs;

namespace RETOAPI.Controllers
{
    [ApiController]
    [Route("Rols")]
    [EnableCors("_myAllowSpecificOrigins")]
    [Authorize]
    public class Rolscontroller:ControllerBase
    {
        private readonly AppDbContext _conexionDB;
        public Rolscontroller(AppDbContext conexionDB)
        {
            _conexionDB = conexionDB;
        }
        [HttpGet("ListAll")]
        public async Task<ActionResult> GetAllRols()
        {
            try
            {
                var roles = await (from r in _conexionDB.Rols
                                      select new RolList
                                      {
                                          RolId=r.RolId,
                                          RolName=r.RolName,
                                          RolActive= r.RolActive,
                                      }).ToListAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
