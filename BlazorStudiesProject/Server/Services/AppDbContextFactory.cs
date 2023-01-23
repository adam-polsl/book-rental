using BlazorStudiesProject.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorStudiesProject.Server.Services
{
    public class AppDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AppDbContextFactory(//IServiceProvider serviceProvider,
            IServiceScopeFactory serviceScopeFactory)
        {
            // _serviceProvider = serviceProvider;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public ApplicationDbContext CreateDbContext()
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var myScopedService = scope.ServiceProvider.GetService<ApplicationDbContext>();

                //myScopedService.DoSomething();
            }
            try
            {
                var a = _serviceProvider.GetRequiredService<ApplicationDbContext>();
                return a;

            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
