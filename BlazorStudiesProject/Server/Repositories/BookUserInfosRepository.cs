using AutoMapper;
using BlazorStudiesProject.Server.Data;
using BlazorStudiesProject.Server.Entities;
using BlazorStudiesProject.Shared.Dtos;
using BlazorStudiesProject.Shared.Dtos.Book;
using Microsoft.EntityFrameworkCore;

namespace BlazorStudiesProject.Server.Repositories
{
    public class BookUserInfosRepository : IBookUserInfosRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public BookUserInfosRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _dbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<BookUserInfoDto> GetInfo(long bookId, Guid userId)
        {
            var bookUserInfo = await _dbContext
                .BookUserInfos
                .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);

            if (bookUserInfo == null)
            {
                bookUserInfo = new BookUserInfoEntity
                {
                    BookId = bookId,
                    UserId = userId,
                    CurrentPage = 1
                };
                await _dbContext.BookUserInfos.AddAsync(bookUserInfo);
                await _dbContext.SaveChangesAsync();
            }

            var dto = _mapper.Map<BookUserInfoDto>(bookUserInfo);

            var activePurchase = await _dbContext
                .ActivePurchases
                .FirstOrDefaultAsync(x => x.UserId == userId
                && x.BookId == bookId);

            if (activePurchase != null)
            {
                dto.ValidUntill = activePurchase.ValidUntill;
            }
            dto.Owned = activePurchase != null;


            return dto;
        }

        public async Task<ContentDto> GetContent(long bookId, Guid userId, int page)
        {
            var contentEntity = await _dbContext
                .ActivePurchases
                .Include(x => x.Book)
                .Include(x => x.Book.Content)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);

            if (contentEntity == null)
            {
                throw new Exception("content not avaliable");
            }

            await GetInfo(bookId, userId);

            await ChangeCurrentPage(userId, bookId, page);

            return _mapper.Map<ContentDto>(contentEntity.Book.Content);
        }

        public async Task<IList<DisplayBookDto>> GetUserBooks(Guid userId)
        {
            var books = await _dbContext.ActivePurchases
                .Include(x => x.Book)
                .Where(x => x.UserId == userId)
                .Select(x => x.Book)
                .ToListAsync();

            return _mapper.Map<IList<DisplayBookDto>>(books);
        }
        private async Task ChangeCurrentPage(Guid userId, long bookId, int newValue)
        {
            var bookUserInfo = await _dbContext
                .BookUserInfos
                .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);

            bookUserInfo.CurrentPage = newValue;
            await _dbContext.SaveChangesAsync();
        }

    }
}
