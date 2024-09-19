using AutoMapper;
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
        private readonly IMapper _mapper;
        public CategoryProductController(AppDbContext conexionDB, IMapper mapper)
        {
            _conexionDB = conexionDB;
            _mapper = mapper;
        }
        [HttpGet("ListAll")]
        public async Task<ActionResult> GetAllCategoryProduct()
        {
            try
            {
                var categories = await _conexionDB.CategoryProducts.ToListAsync();
                var categoryList = _mapper.Map<List<CategoryProductList>>(categories);
                return Ok(categoryList);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
