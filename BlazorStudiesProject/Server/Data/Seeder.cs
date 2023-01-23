using BlazorStudiesProject.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorStudiesProject.Server.Data
{
    public class Seeder
    {
        private readonly ApplicationDbContext _dbContext;

        public Seeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddBook()
        {
            var now = DateTimeOffset.UtcNow.ToString("G");


            var newItem = new BookEntity()
            {
                Author = $"TenTego: {now}",
                ImageUrl = "imgurl",
                Title = "Title",
                AdditionalInfo = new AdditionalInfoEntity() { Description = "asd", Price = 1200 },
                Content = new ContentEntity() { Text = "asdasd" }
            };

            await _dbContext.Books.AddAsync(newItem);
            var xd = await _dbContext.SaveChangesAsync();
        }

        public async Task AddInactiveSub()
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync();
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.Contains("bb"));
            var random = new Random();
            var value = random.Next() % 20;
            var x = new ActivePurchaseEntity { Book = book, User = user, MoneyPaid = 724, PurchaseDate = new DateTime(2012, 1, 1), ValidUntill = new DateTime(2012, 2, 1 + value) };

            await _dbContext.ActivePurchases.AddAsync(x);

            var xd = await _dbContext.SaveChangesAsync();

        }


    }
}
