using System;
using CDNMiddleware.Application.DataAccess;
using CDNMiddleware.Application.DataAccess.Interfaces;
using CDNMiddleware.Application.Services;
using CDNMiddleware.Application.Services.Interfaces;

namespace CDNMiddleware.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDataAccess(this IServiceCollection services)
        {
            return services.AddScoped<IUserDataAccess, UserDataAccess>()
                            ;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services.AddScoped<IUserService, UserService>()
                            ;
        }
    }
}

