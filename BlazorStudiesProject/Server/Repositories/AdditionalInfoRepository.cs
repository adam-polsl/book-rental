using AutoMapper;
using BlazorStudiesProject.Server.Data;
using BlazorStudiesProject.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BlazorStudiesProject.Server.Repositories
{
    public class AdditionalInfoRepository : IAdditionalInfoRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public AdditionalInfoRepository(ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AdditionalInfoDto> GetAdditionalInfo(long id)
        {
            var additionalInfoEnitity = await _dbContext
                .AdditionalInfos
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<AdditionalInfoDto>(additionalInfoEnitity);
        }
    }
}
