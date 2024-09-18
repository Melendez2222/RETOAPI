using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RETOAPI.Data;
using RETOAPI.DTOs;
using RETOAPI.Models;

namespace RETOAPI.Controllers
{
    [ApiController]
    [Route("CATEGORY")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class CategoryProductController : ControllerBase
    {
        private readonly AppDbContext _conexionDB;
        public CategoryProductController(AppDbContext conexionDB)
        {
            _conexionDB = conexionDB;
        }
        [HttpGet("ListAll")]
        public async Task<ActionResult> GetAllCategoryProduct()
        {
            try
            {
                var Category = await (from c in _conexionDB.CategoryProducts 
                                      select new CategoryProductList { 
                                      CatProductId=c.CatProductId,
                                      CatProductName=c.CatProductName,
                                      CatProductActive=c.CatProductActive,
                                      }).ToListAsync();
                return Ok(Category);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
