using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Models;
namespace BookStore.Repositories
{
    public interface IBookRepository
    {
        //Task<IEnumerable<Book>> GetBooks();
        Task<IEnumerable<Book>> GetBooks(string searchTerm, int pageNumber, int pageSize);
        Task<int> GetBooksCount(string searchTerm);

        Task<Book> GetBookById(int id);
        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<bool> DeleteBook(int id);
    }
}
