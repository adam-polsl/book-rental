using BlazorStudiesProject.Server.Repositories;
using BlazorStudiesProject.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorStudiesProject.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class AdditionalInfoController : ControllerBase
    {
        private readonly IAdditionalInfoRepository _additionalInfoRepository;
        public AdditionalInfoController(IAdditionalInfoRepository additionalInfoRepository)
        {
            _additionalInfoRepository = additionalInfoRepository;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<AdditionalInfoDto>> GetAdditionalInfo(long id)
        {
            var additionalInfo = await _additionalInfoRepository.GetAdditionalInfo(id);

            return Ok(additionalInfo);
        }
    }
}
