using BlazorStudiesProject.Shared.Dtos;

namespace BlazorStudiesProject.Server.Repositories
{
    public interface IPurchasesRepository
    {
        Task Activate(string code, Guid userId);
        Task AddCode(CreateCodeDto codeDto);
        Task<IList<CreateCodeDto>> GetAllCodes();
    }
}
