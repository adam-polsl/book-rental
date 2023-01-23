using BlazorStudiesProject.Shared.Dtos;

namespace BlazorStudiesProject.Server.Repositories
{
    public interface IAdditionalInfoRepository
    {
        Task<AdditionalInfoDto> GetAdditionalInfo(long id);
    }
}
