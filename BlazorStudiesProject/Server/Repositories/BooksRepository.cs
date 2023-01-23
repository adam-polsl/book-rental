using AutoMapper;
using BlazorStudiesProject.Server.Data;
using BlazorStudiesProject.Server.Entities;
using BlazorStudiesProject.Shared.Dtos.Book;
using Microsoft.EntityFrameworkCore;


namespace BlazorStudiesProject.Server.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public BooksRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        //admin
        public async Task<long> Add(BookDto createBookDto)
        {
            var newEntity = _mapper.Map<BookEntity>(createBookDto);

            newEntity.AdditionalInfo = new AdditionalInfoEntity
            {
                Description = createBookDto.Description ?? "description not provided",
                Price = createBookDto.Price ?? 100000,
            };

            newEntity.Content = new ContentEntity
            {
                Text = createBookDto.Content ?? "notProvided"
            };

            await _dbContext.Books.AddAsync(newEntity);

            await _dbContext.SaveChangesAsync();
            return newEntity.Id;

        }

        //admin
        public async Task<BookDto> Delete(long id)
        {
            var book = _dbContext
                .Books
                .FirstOrDefault(book => book.Id == id);

            if (book == null)
            {
                return null;
            }

            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();

            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;

        }

        //admin
        public async Task<BookDto> Edit(long id, BookDto editBookDto)
        {
            var bookEntity = await _dbContext
                .Books
                .Include(book => book.Content)
                .Include(book => book.AdditionalInfo)
                .FirstOrDefaultAsync(book => book.Id == id);

            if (bookEntity == null)
            {
                throw new Exception($"book with id: {id} not found");
            }

            if (!string.IsNullOrEmpty(editBookDto.Title))
            {
                bookEntity.Title = editBookDto.Title;
            }

            if (!string.IsNullOrEmpty(editBookDto.Author))
            {
                bookEntity.Author = editBookDto.Author;
            }

            if (!string.IsNullOrEmpty(editBookDto.ImageUrl))
            {
                bookEntity.ImageUrl = editBookDto.ImageUrl;
            }

            if (!string.IsNullOrEmpty(editBookDto.Content))
            {
                bookEntity.Content.Text = editBookDto.Content;
            }

            if (!string.IsNullOrEmpty(editBookDto.Description))
            {
                bookEntity.AdditionalInfo.Description = editBookDto.Description;
            }

            if (editBookDto.Price is int price)
            {
                bookEntity.AdditionalInfo.Price = price;
            }



            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BookDto>(bookEntity);
        }

        //admin
        public async Task<BookDto> GetById(long id)
        {
            var bookEntity = await _dbContext.Books
                .Include(book => book.Content)
                .Include(book => book.AdditionalInfo)
                .Where(book => book.Id == id)
                .FirstOrDefaultAsync();

            if (bookEntity == null)
            {
                throw new Exception($"book with id: {id} not found");
            }

            return _mapper.Map<BookDto>(bookEntity);
        }

        public async Task<IList<DisplayBookDto>> GetAll()
        {
            var mappedBooks = await _mapper.ProjectTo<DisplayBookDto>(_dbContext.Books).ToListAsync();
            return mappedBooks;
        }
    }
}
