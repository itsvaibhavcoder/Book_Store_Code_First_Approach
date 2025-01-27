using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using System;

namespace BookStore.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly BookStoreApiContext _context;
        public CartRepository(BookStoreApiContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CartItems>> GetUserCart(int userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Book)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
        }

        public async Task<CartItems> AddToCart(int userId, int bookId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            var cartItem = await _context.CartItems
                .Include(c => c.Book) // Ensure Book entity is included
                .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItems { UserId = userId, BookId = bookId, Quantity = quantity };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            // Fetch again with included Book to ensure it's returned properly
            return await _context.CartItems
                .Include(c => c.Book) // Ensure Book is loaded in the response
                .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);
        }

        public async Task<CartItems> UpdateCartItem(int userId, int cartItemId, int quantity)
        {
           

            var cartItem = await _context.CartItems
        .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (cartItem == null)
            {
                return null;  // This will trigger "Cart item not found."
            }

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> RemoveFromCart(int userId, int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
