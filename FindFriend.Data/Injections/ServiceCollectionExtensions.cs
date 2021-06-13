using FindFriend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FindFriend.Data.Injections
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUnitOfWork(this IServiceCollection services, string connection)
        {
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(connection));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}