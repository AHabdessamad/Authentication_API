using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class BookRepository : IBookRepository
    {
        public readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchByTitle(string title)
        {
            var matchedBooks = await _context.Books.Where(b => b.Title.Contains(title)).ToListAsync();
            return matchedBooks;
        }

        public async Task<Book> GetBookById(int id)
        { 
            var foundBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            return foundBook;
        }

        public async Task<Book> CreateBook(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateBook(int id, Book book)
        {
            var foundBook = await GetBookById(id);
            if (foundBook == null) return null;

            foundBook.Title = book.Title;
            foundBook.Author = book.Author;
            foundBook.PublishDate = book.PublishDate;
            foundBook.Price = book.Price;
            foundBook.ISBN = book.ISBN;
            foundBook.NbrOfCopy = book.NbrOfCopy;

            await _context.SaveChangesAsync();
            return foundBook;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await GetBookById(id);
            if (book == null) return false;
            _context.Remove(book);
            await _context.SaveChangesAsync();
            return true;

        }

        
    }
}
