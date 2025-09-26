using BSB.src.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSB.src.Application.Services
{
    public class BookService : IBookService
    {
        public Task<IEnumerable<BookDto>> GetAllAsync() => Task.FromResult<IEnumerable<BookDto>>(new List<BookDto>());
        public Task<BookDto?> GetByIdAsync(Guid id) => Task.FromResult<BookDto?>(null);
    }
}