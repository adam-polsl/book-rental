using BlazorStudiesProject.Shared.Dtos.Book;

namespace BlazorStudiesProject.Server.Repositories
{
    public interface IBooksRepository
    {
        Task<BookDto> GetById(long id);
        Task<IList<DisplayBookDto>> GetAll();
        Task<long> Add(BookDto createBookDto);
        Task<BookDto> Edit(long id, BookDto editBookDto);
        Task<BookDto> Delete(long id);
    }
}
