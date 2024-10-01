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
    public class CartController : ControllerBase
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
        [HttpPost("CartUser")]
        public async Task<ActionResult> GetCartUser([FromBody] SearchCartByUser searchCartByUser) 
        {
            try
            {
                var user = await _conexionDB.Users.FirstOrDefaultAsync(u =>
                     u.UserUsername == _serviceCredentials.HashString(searchCartByUser.Username) &&
                     u.UserPassword == _serviceCredentials.HashString(searchCartByUser.Password));
                if (user == null)
                {
                    return Unauthorized("Invalid credentials");
                }
                int userId = user.UserId;

                var cart = await _conexionDB.CartUsers.FirstOrDefaultAsync(c => c.UserId == user.UserId);
                if (cart == null)
                {
                    return Ok(); // No retornar nada si no existe el UserId en CartUser
                }
                // Buscar en CartDetail según CartId
                var cartDetails = await _conexionDB.CartDetail
                    .Where(cd => cd.IdCart == cart.IdCart)
                    .ToListAsync();

                if (cartDetails == null || !cartDetails.Any())
                {
                    return NotFound("No se encontraron detalles del carrito.");
                }

                // Mapear a DTO usando AutoMapper
                var cartDetailsDto = _mapper.Map<List<CartItemsList>>(cartDetails);

                // Devolver los detalles del carrito
                return Ok(cartDetailsDto);
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
                    existingCartDetail.Quantity += 1; // Incrementar la cantidad
                }
                else
                {
                    var newCartDetail = _mapper.Map<CartDetail>(cartItemDetail);
                    newCartDetail.IdCart = existingCartUser.IdCart;
                    _conexionDB.CartDetail.Add(newCartDetail);
                }

                await _conexionDB.SaveChangesAsync();
               
                return Ok(new { message = "Producto agregado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("DecreaseCartItem")]
        public async Task<ActionResult> DecreaseCartItem([FromBody] CartItemDetail cartItemDetail, [FromServices] IMapper mapper)
        {
            try
            {
                // Autenticar al usuario
                var user = await _conexionDB.Users.FirstOrDefaultAsync(u =>
                    u.UserUsername == _serviceCredentials.HashString(cartItemDetail.UserName) &&
                    u.UserPassword == _serviceCredentials.HashString(cartItemDetail.UserPassword));

                if (user == null)
                {
                    return Unauthorized("Invalid credentials");
                }

                int userId = user.UserId;

                // Buscar CartUser
                var existingCartUser = await _conexionDB.CartUsers.FirstOrDefaultAsync(cu => cu.UserId == userId);
                if (existingCartUser == null)
                {
                    return NotFound("Cart not found for the user");
                }

                // Buscar CartDetail
                var existingCartDetail = await _conexionDB.CartDetail.FirstOrDefaultAsync(cd => cd.IdCart == existingCartUser.IdCart && cd.ProductId == cartItemDetail.ProductId);
                if (existingCartDetail == null)
                {
                    return NotFound("Product not found in the cart");
                }

                // Decrementar la cantidad o eliminar el producto
                if (existingCartDetail.Quantity == 1)
                {
                    _conexionDB.CartDetail.Remove(existingCartDetail);
                }
                else
                {
                    existingCartDetail.Quantity -= 1;
                }

                await _conexionDB.SaveChangesAsync();

                return Ok(new { message = "Cantidad del producto actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("RemoveCartItem")]
        public async Task<ActionResult> RemoveCartItem([FromBody] CartItemDetail cartItemDetail)
        {
            try
            {
                // Autenticar al usuario
                var user = await _conexionDB.Users.FirstOrDefaultAsync(u =>
                    u.UserUsername == _serviceCredentials.HashString(cartItemDetail.UserName) &&
                    u.UserPassword == _serviceCredentials.HashString(cartItemDetail.UserPassword));

                if (user == null)
                {
                    return Unauthorized("Invalid credentials");
                }

                int userId = user.UserId;

                // Buscar CartUser
                var existingCartUser = await _conexionDB.CartUsers.FirstOrDefaultAsync(cu => cu.UserId == userId);
                if (existingCartUser == null)
                {
                    return NotFound("Cart not found for the user");
                }

                // Buscar CartDetail
                var existingCartDetail = await _conexionDB.CartDetail.FirstOrDefaultAsync(cd => cd.IdCart == existingCartUser.IdCart && cd.ProductId == cartItemDetail.ProductId);
                if (existingCartDetail == null)
                {
                    return NotFound("Product not found in the cart");
                }

                // Eliminar el producto del carrito
                _conexionDB.CartDetail.Remove(existingCartDetail);
                await _conexionDB.SaveChangesAsync();

                return Ok(new { message = "Producto eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
    
}
