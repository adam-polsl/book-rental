using AutoMapper;
using BlazorStudiesProject.Server.Models;
using BlazorStudiesProject.Server.Repositories;
using BlazorStudiesProject.Shared.Dtos.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorStudiesProject.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMapper _mapper; //should be removed
        private readonly IBooksRepository _booksRepository;

        public BooksController(IMapper mapper, IBooksRepository booksRepository)
        {
            _mapper = mapper;
            _booksRepository = booksRepository;
        }

        [AllowAnonymous]//admin only
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> Get([FromRoute] long id)
        {
            return Ok(await _booksRepository.GetById(id));
        }

        [AllowAnonymous]
        [HttpGet("Display/{id}")]
        public async Task<ActionResult<DisplayBookDto>> GetDisplay(long id)
        {
            var results = await _booksRepository.GetAll();
            return Ok(results.FirstOrDefault(x => x.Id == id));
        }


        //collection
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IList<DisplayBookDto>>> GetAll()
        {
            return Ok(await _booksRepository.GetAll());
        }

        //Authorize group admin
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<ActionResult<BookDto>> Delete([FromRoute] long id)
        {
            return Ok(await _booksRepository.Delete(id));
        }

        //Authorize group admin
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<BookDto>> Create([FromBody] BookDto bookDto)
        {
            //var image = await new HttpClient().GetByteArrayAsync(bookDto.ImageUrl);

            // var wrappedImage = $"data:image/jpeg;base64,{Convert.ToBase64String(image)}";
            return Ok(await _booksRepository.Add(bookDto));
        }

        //Authorize group admin
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> Edit([FromRoute] long id, [FromBody] BookDto bookDto)
        {
            return Ok(await _booksRepository.Edit(id, bookDto));
        }

        [HttpGet("Duck")]
        public async Task<ActionResult<DukResponse>> GetDuck()
        {
            var httpClient = new HttpClient();
            var dukResponse = await httpClient.GetFromJsonAsync<DukResponse>("https://random-d.uk/api/v2/random");
            return Ok(dukResponse);
        }
    }
}
