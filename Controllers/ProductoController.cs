using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RETOAPI.Data;
using RETOAPI.DTOs;
using RETOAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RETOAPI.Controllers
{
    [ApiController]
    [Route("PRODUCT")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ProductoController:ControllerBase
    {
        private readonly AppDbContext _conexionDB;
        private readonly IMapper _mapper;
        public ProductoController(AppDbContext conexionDB, IMapper mapper)
        {
            _conexionDB = conexionDB;
            _mapper = mapper;
        }
        [HttpGet("ListAll")]
        public async Task<ActionResult> GetAllProducts()
        {
            try
            {
                var products = await _conexionDB.Products
                    .Include(p => p.CategoryProduct)
                    .ToListAsync();

                var productList = _mapper.Map<List<ProductList>>(products);

                return Ok(productList);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateProduct([FromBody] ProductCreate productCreate)
        {
            try
            {
                var product = _mapper.Map<Product>(productCreate);

                _conexionDB.Products.Add(product);
                await _conexionDB.SaveChangesAsync();

                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("Update")]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductUpdate productUpdate)
        {
            try
            {
                var product = await _conexionDB.Products.FindAsync(productUpdate.Id_Product);
                if (product == null)
                {
                    return NotFound();
                }

                _mapper.Map(productUpdate, product); 

                await _conexionDB.SaveChangesAsync();

                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _conexionDB.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                _conexionDB.Products.Remove(product);
                await _conexionDB.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error");
            }
        }
    }
        
       
}
