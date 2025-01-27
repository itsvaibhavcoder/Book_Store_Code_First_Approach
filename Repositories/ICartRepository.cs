using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Models;
namespace BookStore.Repositories
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItems>> GetUserCart(int userId);
        Task<CartItems> AddToCart(int userId, int bookId, int quantity);
        Task<CartItems> UpdateCartItem(int userId, int cartItemId, int quantity);
        Task<bool> RemoveFromCart(int userId, int cartItemId);
    }
}
