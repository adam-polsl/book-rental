using AutoMapper;
using BlazorStudiesProject.Server.Data;
using BlazorStudiesProject.Server.Entities;
using BlazorStudiesProject.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BlazorStudiesProject.Server.Repositories
{
    public class PurchasesRepository : IPurchasesRepository
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;


        public PurchasesRepository(ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Activate(string code, Guid userId)
        {
            var codeEntity = await _dbContext
                .Codes
                .FirstOrDefaultAsync(x => x.Code == code);

            if (codeEntity == null)
            {
                throw new Exception("invalid code");
            }

            _dbContext
                .Codes
                .Remove(codeEntity);

            await _dbContext
                .ActivePurchases
                .AddAsync(CreatePurchase(codeEntity.BookId, userId, codeEntity.MoneyPaid));

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddCode(CreateCodeDto codeDto)
        {
            var newEntity = _mapper.Map<CodeEntity>(codeDto);
            await _dbContext.Codes.AddAsync(newEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<CreateCodeDto>> GetAllCodes()
        {
            var codes = await _dbContext.Codes.ToListAsync();

            return _mapper.Map<IList<CreateCodeDto>>(codes);
        }

        private ActivePurchaseEntity CreatePurchase(long bookId, Guid userId, int moneyPaid)
        {
            var validUntilDate = DateTime.UtcNow.AddMonths(1);

            return new ActivePurchaseEntity
            {
                BookId = bookId,
                UserId = userId,
                MoneyPaid = moneyPaid,
                PurchaseDate = DateTime.UtcNow,
                ValidUntill = validUntilDate,
            };
        }
    }
}
