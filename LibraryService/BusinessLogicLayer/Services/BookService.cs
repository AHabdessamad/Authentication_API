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

        public async Task<Book> CreateBook(BookDto bookDto)
        {
            if (bookDto == null) return null;
            var bookMaped = _mapper.Map<Book>(bookDto);
            return  await _bookRepository.CreateBook(bookMaped);

        }

        public async Task<bool> DeleteBook(int id)
        {
            return await _bookRepository.DeleteBook(id);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAllBooks();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _bookRepository.GetBookById(id);
        }

        public async Task<Book> UpdateBook(int id, BookDto bookDto)
        {
            var bookMaped = _mapper.Map<Book>(bookDto);
            return await _bookRepository.UpdateBook(id, bookMaped);
            
        }

        public async Task<IEnumerable<Book>> SearchByTitle(string title)
        {
            return await _bookRepository.SearchByTitle(title);
        }
    }

}
