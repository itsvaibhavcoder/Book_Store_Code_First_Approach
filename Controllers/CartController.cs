//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using BookStore.Models;
//using BookStore.Repositories;
//using System;
//namespace BookStore.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CartController : ControllerBase
//    {
//        private readonly ICartRepository _cartRepository;

//        public CartController(ICartRepository cartRepository)
//        {
//            _cartRepository = cartRepository;
//        }

//        [HttpGet("{userId}")]
//        public async Task<ActionResult<IEnumerable<CartItems>>> GetUserCart(int userId)
//        {
//            return Ok(await _cartRepository.GetUserCart(userId));
//        }

//        [HttpPost]

//        public async Task<IActionResult> AddToCart([FromBody] CartItems request)
//        {
//            Console.WriteLine($"Received Data - UserId: {request.UserId}, BookId: {request.BookId}, Quantity: {request.Quantity}");

//            if (request.Quantity <= 0)
//            {
//                return BadRequest(new { message = "Quantity must be greater than zero." });
//            }

//            var cartItem = await _cartRepository.AddToCart(request.UserId, request.BookId, request.Quantity);

//            if (cartItem == null)
//            {
//                return BadRequest(new { message = "Failed to add item to cart." });
//            }

//            return Ok(cartItem);
//        }


//        [HttpPut("{userId}/{cartItemId}")]
//        public async Task<IActionResult> UpdateCartItem(int userId, int cartItemId, [FromBody] CartItems request)
//        {
//            if (request.Quantity <= 0)
//            {
//                return BadRequest("Quantity must be greater than 0.");
//            }

//            var result = await _cartRepository.UpdateCartItem(userId, cartItemId, request.Quantity);

//            if (result == null)
//            {
//                return NotFound("Cart item not found.");
//            }

//            return Ok("Cart item updated successfully.");
//        }

//        [HttpDelete("{userId}/{cartItemId}")]
//        public async Task<IActionResult> RemoveFromCart(int userId, int cartItemId)
//        {
//            var result = await _cartRepository.RemoveFromCart(userId, cartItemId);
//            if (!result) return NotFound();
//            return Ok("Cart item removed successfully..");
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Repositories;
using System;
using System.Linq;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<CartItems>>> GetUserCart(int userId)
        {
            var cartItems = await _cartRepository.GetUserCart(userId);

            if (cartItems == null)
                throw new KeyNotFoundException($"No items found in cart for user with ID {userId}.");

            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItems request)
        {
            Console.WriteLine($"Received Data - UserId: {request.UserId}, BookId: {request.BookId}, Quantity: {request.Quantity}");

            if (request.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");  // This triggers 400 Bad Request

            var cartItem = await _cartRepository.AddToCart(request.UserId, request.BookId, request.Quantity);

            if (cartItem == null)
                throw new InvalidOperationException("Failed to add item to cart.");  // Triggers 500 Internal Server Error

            return Ok(cartItem);
        }

        [HttpPut("{userId}/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int userId, int cartItemId, [FromBody] CartItems request)
        {
            if (request.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0.");  // 400 Bad Request

            var result = await _cartRepository.UpdateCartItem(userId, cartItemId, request.Quantity);

            if (result == null)
                throw new KeyNotFoundException($"Cart item with ID {cartItemId} not found for user {userId}.");  // 404 Not Found

            return Ok("Cart item updated successfully.");
        }

        [HttpDelete("{userId}/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int cartItemId)
        {
            var result = await _cartRepository.RemoveFromCart(userId, cartItemId);

            if (!result)
                throw new KeyNotFoundException($"Cart item with ID {cartItemId} not found for user {userId}.");  // 404 Not Found

            return Ok("Cart item removed successfully.");
        }
    }
}
