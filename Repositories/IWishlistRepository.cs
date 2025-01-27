using BookStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BookStore.Repositories
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<Wishlist>> GetWishlist(int userId);
        Task<Wishlist> AddToWishlist(int userId, int bookId);
        Task<bool> RemoveFromWishlist(int userId, int wishlistId);
    }
}
