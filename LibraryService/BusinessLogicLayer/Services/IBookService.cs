using DataAccessLayer.Entities;


namespace BusinessLogicLayer.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(int id);
        Task<IEnumerable<Book>> SearchByTitle(string title);
        Task<Book> CreateBook(BookDto bookDto);
        Task<Book> UpdateBook(int id, BookDto book);
        Task<bool> DeleteBook(int id);
    }
}
