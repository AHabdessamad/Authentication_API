using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public interface IBookRepository
    {
        public Book CreateBook(Book book);
        public IEnumerable<Book> GetAllBooks();
        public bool DeleteBook(int id);
        public Book GetBookById(int id);
        public IEnumerable<Book> SearchByTitle(string title);
        public Book UpdateBook(int id, Book book);
    }
}
