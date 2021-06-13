using System;
using AutoMapper;
using FindFriend.Business.Interfaces;
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

            services.AddTransient<IAuthService, IAuthService>();
            services.AddTransient<IAddService, AddService>();
            services.AddTransient<IUserService, UserService>();
        }

        public static void AddAutomapper(this IServiceCollection services,
            Action<IMapperConfigurationExpression> config)
        {
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(config)));
        }
    }
}