using BookStore.Data;
using BookStore.Models;
using BookStore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly BookStoreApiContext _context;

        public WishlistRepository(BookStoreApiContext context)
        {
            _context = context;
        }

        // Get Wishlist for User
        public async Task<IEnumerable<Wishlist>> GetWishlist(int userId)
        {
            return await _context.Wishlist
                .Where(w => w.UserId == userId)
                .Include(w => w.Book)
                .ToListAsync();
        }

        // Add to Wishlist
        public async Task<Wishlist> AddToWishlist(int userId, int bookId)
        {
            var wishlistItem = new Wishlist { 
                UserId = userId, 
                BookId = bookId 
            };

            _context.Wishlist.Add(wishlistItem);
            await _context.SaveChangesAsync();
            var addedWishlistItem = await _context.Wishlist
        .Include(w => w.User)  
        .Include(w => w.Book) 
        .FirstOrDefaultAsync(w => w.Id == wishlistItem.Id);
            return wishlistItem;
        }

        // Remove from Wishlist
        public async Task<bool> RemoveFromWishlist(int userId, int wishlistId)
        {
            var wishlistItem = await _context.Wishlist
                .FirstOrDefaultAsync(w => w.Id == wishlistId && w.UserId == userId);

            if (wishlistItem == null)
                return false;

            _context.Wishlist.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
