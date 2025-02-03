using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using System.Linq;
namespace BookStore.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreApiContext _context;
        public BookRepository(BookStoreApiContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Book>> GetBooks()
        //{
        //    return await _context.Books.ToListAsync();
        //}

        public async Task<IEnumerable<Book>> GetBooks(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.Books.AsQueryable();

            // Apply search filter if searchTerm is provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));
            }

            // Get total count for pagination metadata (if needed)
            int totalItems = await query.CountAsync();

            // Apply pagination
            var books = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return books;
        }

        public async Task<int> GetBooksCount(string searchTerm)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));
            }

            return await query.CountAsync();
        }


        public async Task<Book> GetBookById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book> AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
