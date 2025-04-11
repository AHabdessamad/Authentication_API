using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepo, IMapper mapper)
        {
            _bookRepository = bookRepo;
            _mapper = mapper;
        }

        public Book CreateBook(BookDto bookDto)
        {
            if (bookDto == null) return null;
            var bookMaped = _mapper.Map<Book>(bookDto);
            return  _bookRepository.CreateBook(bookMaped);

        }

        public bool DeleteBook(int id)
        {
            return _bookRepository.DeleteBook(id);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public Book GetBookById(int id)
        {
            return _bookRepository.GetBookById(id);
        }

        public Book UpdateBook(int id, BookDto bookDto)
        {
            var bookMaped = _mapper.Map<Book>(bookDto);
            return _bookRepository.UpdateBook(id, bookMaped);
            
        }

        public IEnumerable<Book> SearchByTitle(string title)
        {
            return _bookRepository.SearchByTitle(title);
        }
    }

}
