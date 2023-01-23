using AutoMapper;
using BlazorStudiesProject.Server.Data;
using BlazorStudiesProject.Server.Entities;
using Microsoft.EntityFrameworkCore;
namespace BlazorStudiesProject.Server.Services
{
    public class AppBackgroundWorker
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AppBackgroundWorker> _logger;
        private const int ScaningPeriodInSeconds = 5;
        private int ScaningPeriodInMs => ScaningPeriodInSeconds * 1000;

        public AppBackgroundWorker(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<AppBackgroundWorker> logger,
            IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _mapper = mapper;
            Task.Run(() => Start());
        }

        public async Task Start(CancellationToken cancellation = default)
        {

            while (true)
            {
                if (cancellation.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var myScopedService = scope.ServiceProvider.GetService<ApplicationDbContext>();
                        _ = DoIt(myScopedService);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }

                await Task.Delay(ScaningPeriodInMs, cancellation);
            }
        }

        public async Task DoIt(ApplicationDbContext dbContext)
        {
            var subscriptionsToArchive = await dbContext.ActivePurchases.Where(x => x.ValidUntill < DateTime.UtcNow).ToListAsync();
            dbContext.ActivePurchases.RemoveRange(subscriptionsToArchive);
            var subscriptionsToMove = _mapper.Map<IList<ArchivePurchaseEntity>>(subscriptionsToArchive);
            await dbContext.ArchivePurchases.AddRangeAsync(subscriptionsToMove);
            await dbContext.SaveChangesAsync();
        }
    }
}
