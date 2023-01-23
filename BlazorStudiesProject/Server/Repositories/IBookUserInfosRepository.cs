using BlazorStudiesProject.Shared.Dtos;
using BlazorStudiesProject.Shared.Dtos.Book;

namespace BlazorStudiesProject.Server.Repositories
{
    public interface IBookUserInfosRepository
    {
        Task<BookUserInfoDto> GetInfo(long bookId, Guid userId);
        Task<IList<DisplayBookDto>> GetUserBooks(Guid userId);
        Task<ContentDto> GetContent(long bookId, Guid userId, int page);
    }
}
