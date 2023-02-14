using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Vueling.Otd.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddApplicationLoggingBehaviour();

            return services;
        }
    }
}