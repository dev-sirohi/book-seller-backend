using System.Threading.Tasks;
using System.Collections.Generic;

namespace BSB.src.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto?> GetByIdAsync(Guid id);
        // Add more methods as needed
    }
}