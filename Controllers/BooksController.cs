using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Repositories;
using System.Linq;
using System;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>>GetBooks()
        {
            var books = await _bookRepository.GetBooks();
            if (books == null)
            {
                throw new ArgumentException("No books found.");  // This triggers 400 BadRequest via middleware
            }
            return Ok(books);
            //return Ok(await _bookRepository.GetBooks());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookById(id) ?? throw new KeyNotFoundException($"Book with ID {id} not found.");
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            if (book == null) throw new ArgumentException("Invalid book details provided.");
            var newBook = await _bookRepository.AddBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> UpdateBook(int id, [FromBody] Book book)
        {
            //if (id != book.Id) return BadRequest();
            if (id != book.Id)
                throw new ArgumentException("Book ID mismatch.");
            // return Ok(await _bookRepository.UpdateBook(book));
            var updatedBook = await _bookRepository.UpdateBook(book);
            if (updatedBook == null)
                throw new KeyNotFoundException($"Book with ID {id} not found for update.");  // 404 if book doesn't exist

            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookRepository.DeleteBook(id);
            //if (!result) return NotFound();
            if (!result)
                throw new KeyNotFoundException($"Book with ID {id} not found for deletion.");
            return NoContent();
        }

    }
}
