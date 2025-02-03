//using BookStore.Models;
//using BookStore.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace BookStore.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class WishlistController : ControllerBase
//    {
//        private readonly IWishlistRepository _wishlistRepository;

//        public WishlistController(IWishlistRepository wishlistRepository)
//        {
//            _wishlistRepository = wishlistRepository;
//        }

//        [HttpGet("{userId}")]
//        public async Task<IActionResult> GetWishlist(int userId)
//        {
//            var wishlist = await _wishlistRepository.GetWishlist(userId);
//            return Ok(wishlist);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddToWishlist([FromBody] Wishlist request)
//        {
//            var wishlistItem = await _wishlistRepository.AddToWishlist(request.UserId, request.BookId);
//            return CreatedAtAction(nameof(GetWishlist), new { userId = wishlistItem.UserId }, wishlistItem);
//        }

//        [HttpDelete("{userId}/{wishlistId}")]
//        public async Task<IActionResult> RemoveFromWishlist(int userId, int wishlistId)
//        {
//            var result = await _wishlistRepository.RemoveFromWishlist(userId, wishlistId);
//            if (!result)
//            {
//                return NotFound("Wishlist item not found.");
//            }
//            return Ok("Wishlist item removed successfully.");
//        }

//    }
//}


using BookStore.Models;
using BookStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistController(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishlist(int userId)
        {
            var wishlist = await _wishlistRepository.GetWishlist(userId);

            if (wishlist == null)
                throw new KeyNotFoundException($"No wishlist items found for user with ID {userId}.");  // 404 Not Found

            return Ok(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] Wishlist request)
        {
            if (request.UserId <= 0 || request.BookId <= 0)
                throw new ArgumentException("Invalid UserId or BookId provided.");  // 400 Bad Request

            var wishlistItem = await _wishlistRepository.AddToWishlist(request.UserId, request.BookId);

            if (wishlistItem == null)
                throw new InvalidOperationException("Failed to add item to wishlist.");  // 500 Internal Server Error

            return CreatedAtAction(nameof(GetWishlist), new { userId = wishlistItem.UserId }, wishlistItem);
        }

        [HttpDelete("{userId}/{wishlistId}")]
        public async Task<IActionResult> RemoveFromWishlist(int userId, int wishlistId)
        {
            var result = await _wishlistRepository.RemoveFromWishlist(userId, wishlistId);

            if (!result)
                throw new KeyNotFoundException($"Wishlist item with ID {wishlistId} not found for user {userId}.");  // 404 Not Found

            return Ok("Wishlist item removed successfully.");
        }
    }
}
