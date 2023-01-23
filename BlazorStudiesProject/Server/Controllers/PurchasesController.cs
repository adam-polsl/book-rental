using BlazorStudiesProject.Server.Entities;
using BlazorStudiesProject.Server.Repositories;
using BlazorStudiesProject.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorStudiesProject.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PurchasesController : AppBaseController
    {
        private readonly IPurchasesRepository _purchasesRepository;
        private readonly IBookUserInfosRepository _bookUserInfosRepository;

        public PurchasesController(UserManager<ApplicationUser> userManager,
            IPurchasesRepository purchasesRepository,
            IBookUserInfosRepository bookUserInfosRepository) : base(userManager)
        {
            _purchasesRepository = purchasesRepository;
            _bookUserInfosRepository = bookUserInfosRepository;
        }

        [AllowAnonymous]
        [HttpPost("Activate")]
        public async Task<IActionResult> ActivateBook([FromBody] CodeDto code)
        {
            var user = await GetLoggedUserEnity();

            await _purchasesRepository.Activate(code.Code, user.Id);
            return Ok();
        }

        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        [HttpPost("Code")]
        public async Task<IActionResult> AddCode([FromBody] CreateCodeDto createCodeDto)
        {
            await _purchasesRepository.AddCode(createCodeDto);
            return Ok();
        }
        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        [HttpGet("Code")]
        public async Task<ActionResult<IList<CreateCodeDto>>> GetCodes()
        {
            var result = await _purchasesRepository.GetAllCodes();
            return Ok(result);
        }
    }
}
