using BlazorStudiesProject.Server.Entities;
using BlazorStudiesProject.Server.Repositories;
using BlazorStudiesProject.Shared.Dtos;
using BlazorStudiesProject.Shared.Dtos.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorStudiesProject.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BookUserController : AppBaseController
    {
        private readonly IBookUserInfosRepository _bookUserInfosRepository;

        public BookUserController(UserManager<ApplicationUser> userManager,
            IBookUserInfosRepository bookUserInfosRepository) : base(userManager)
        {
            _bookUserInfosRepository = bookUserInfosRepository;
        }

        [AllowAnonymous]
        [HttpGet("UserBooks")]
        public async Task<IList<DisplayBookDto>> GetUserBooks()
        {
            var user = await GetLoggedUserEnity();

            return await _bookUserInfosRepository.GetUserBooks(user.Id);
        }

        [AllowAnonymous]
        [HttpGet("BookContent/{bookId}/{page?}")]
        public async Task<ActionResult<ContentDto>> GetContent(long bookId, int page = 1)
        {
            var user = await GetLoggedUserEnity();
            var result = await _bookUserInfosRepository.GetContent(bookId, user.Id, page);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{bookId}")]
        public async Task<ActionResult<BookUserInfoDto>> GetBookUserInfo(long bookId)
        {
            var user = await GetLoggedUserEnity();
            var result = await _bookUserInfosRepository.GetInfo(bookId, user.Id);
            return Ok(result);
        }
    }
}
