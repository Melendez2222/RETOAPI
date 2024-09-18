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
        public ProductoController(AppDbContext conexionDB)
        {
            _conexionDB = conexionDB;
        }
        [HttpGet("ListAll")]
        public async Task<ActionResult> GetAllProducts()
        {
            try
            {
                var products = await (from p in _conexionDB.Products
                                      join c in _conexionDB.CategoryProducts on p.CatProductId equals c.CatProductId
                                      select new ProductList { 
                                        id_Product=p.Id_Product,
                                        productCode=p.ProductCode,
                                        productName=p.ProductName,
                                        Category=c.CatProductName,
                                        Price=p.Price,
                                        Stock=p.Stock,
                                      }).ToListAsync();
                return Ok(products);
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
                var product = new Product
                {
                    ProductCode = productCreate.ProductCode,
                    ProductName = productCreate.ProductName,
                    CatProductId = productCreate.CatProductId,
                    Price = productCreate.Price,
                    Stock = productCreate.Stock,
                };

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
                product.ProductCode = productUpdate.ProductCode;
                product.ProductName = productUpdate.ProductName;
                product.CatProductId = productUpdate.CatProductId;
                product.Price = productUpdate.Price;
                product.Stock = productUpdate.Stock;

                await _conexionDB.SaveChangesAsync();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await (from p in _conexionDB.Products
                                     where p.Id_Product == id
                                     select p).FirstOrDefaultAsync();

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
                return StatusCode(500, "Internal server error");
            }
        }
    }
        
       
}
