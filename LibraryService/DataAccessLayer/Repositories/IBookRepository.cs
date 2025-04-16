using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public interface IBookRepository
    {
        public Task<Book> CreateBook(Book book);
        public Task<IEnumerable<Book>> GetAllBooks();
        public Task<bool> DeleteBook(int id);
        public Task<Book> GetBookById(int id);
        public Task<IEnumerable<Book>> SearchByTitle(string title);
        public Task<Book> UpdateBook(int id, Book book);
    }
}
