using System.Threading.Tasks;
using BookStore.Models;

namespace BookStore.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task RegisterUserAsync(User user);
        Task<bool> ValidateUserCredentialsAsync(string email, string password);
    }
}
