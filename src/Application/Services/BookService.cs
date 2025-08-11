using Application.DTOs;
using Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BookService : IBookService
    {
        public Task<IEnumerable<BookDto>> GetAllAsync() => Task.FromResult<IEnumerable<BookDto>>(new List<BookDto>());
        public Task<BookDto?> GetByIdAsync(Guid id) => Task.FromResult<BookDto?>(null);
    }
}