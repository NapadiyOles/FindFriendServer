using AutoMapper;
using FindFriend.Business.Interfaces;
using FindFriend.Business.Mapping;
using FindFriend.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using FindFriend.Data.Injections;

namespace FindFriend.Business.Injectinos
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessServices(this IServiceCollection services, string connection)
        {
            services.AddUnitOfWork(connection);

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAddService, AddService>();
            services.AddTransient<IUserService, UserService>();
        }

        public static void AddAutomapper(this IServiceCollection services, params Profile[] profiles)
        {
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();

                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            })));
        }
    }
}