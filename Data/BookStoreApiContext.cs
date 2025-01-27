using Microsoft.EntityFrameworkCore;
using BookStore.Models;
namespace BookStore.Data
{
    public class BookStoreApiContext : DbContext
    {
        public BookStoreApiContext(DbContextOptions<BookStoreApiContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }

    }
}
