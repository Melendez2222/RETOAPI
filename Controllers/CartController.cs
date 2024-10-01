using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RETOAPI.Data;
using RETOAPI.DTOs;
using RETOAPI.Models;
using RETOAPI.Services;

namespace RETOAPI.Controllers
{
    [ApiController]
    [Route("Cart")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class CartController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _conexionDB;
        private readonly IMapper _mapper;
        private readonly ServiceCredentials _serviceCredentials;
        public CartController(IConfiguration configuration, AppDbContext conexionDB, IMapper mapper, ServiceCredentials serviceCredentials)
        {
            _configuration = configuration;
            _conexionDB = conexionDB;
            _mapper = mapper;
            _serviceCredentials = serviceCredentials;
        }
        [HttpGet("CartUser/{id}")]
        public async Task<ActionResult> GetCartUser(int id) 
        {
            try
            {
                var Cart = await _conexionDB.CartUsers.FirstOrDefaultAsync(c => c.UserId == id);
                if (Cart==null)
                {
                    return NotFound();
                }
                return Ok(Cart);
            }
            catch (Exception ex) {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("CartItems")]
        public async Task<ActionResult> SetCartItems([FromBody] CartItemDetail cartItemDetail)
        {
            try
            {
                var user = await _conexionDB.Users.FirstOrDefaultAsync(u =>
                    u.UserUsername == _serviceCredentials.HashString(cartItemDetail.UserName) &&
                    u.UserPassword == _serviceCredentials.HashString(cartItemDetail.UserPassword));

                if (user == null)
                {
                    return Unauthorized("Invalid credentials");
                }

                int userId = user.UserId;

                var existingCartUser = await _conexionDB.CartUsers.FirstOrDefaultAsync(cu => cu.UserId == userId);
                if (existingCartUser == null)
                {
                    var newCartUser = _mapper.Map<CartUser>(cartItemDetail);
                    newCartUser.UserId = userId;
                    _conexionDB.CartUsers.Add(newCartUser);
                    await _conexionDB.SaveChangesAsync();
                    existingCartUser = newCartUser;
                }
                // Buscar o actualizar CartDetail
                var existingCartDetail = await _conexionDB.CartDetail.FirstOrDefaultAsync(cd => cd.IdCart == existingCartUser.IdCart && cd.ProductId == cartItemDetail.ProductId);
                if (existingCartDetail != null)
                {
                    existingCartDetail.Quantity += cartItemDetail.Quantity;
                }
                else
                {
                    var newCartDetail = _mapper.Map<CartDetail>(cartItemDetail);
                    newCartDetail.IdCart = existingCartUser.IdCart;
                    _conexionDB.CartDetail.Add(newCartDetail);
                }

                await _conexionDB.SaveChangesAsync();
                //var cartDetail = _mapper.Map<CartDetail>(cartItemDetail);
                //cartDetail.IdCart = existingCartUser.IdCart;

                //_conexionDB.CartDetail.Add(cartDetail);
                //await _conexionDB.SaveChangesAsync();
                //devolver objeto completo agregado
                return Ok(new { message = "Producto agregado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
    
}
