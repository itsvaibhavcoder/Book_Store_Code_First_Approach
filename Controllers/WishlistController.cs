using BookStore.Models;
using BookStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            return Ok(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] Wishlist request)
        {
            var wishlistItem = await _wishlistRepository.AddToWishlist(request.UserId, request.BookId);
            return CreatedAtAction(nameof(GetWishlist), new { userId = wishlistItem.UserId }, wishlistItem);
        }

        [HttpDelete("{userId}/{wishlistId}")]
        public async Task<IActionResult> RemoveFromWishlist(int userId, int wishlistId)
        {
            var result = await _wishlistRepository.RemoveFromWishlist(userId, wishlistId);
            if (!result)
            {
                return NotFound("Wishlist item not found.");
            }
            return Ok("Wishlist item removed successfully.");
        }

    }
}
