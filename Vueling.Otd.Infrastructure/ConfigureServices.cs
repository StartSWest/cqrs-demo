using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vueling.Otd.Application.CurrencyPair.Interfaces;
using Vueling.Otd.Application.Transaction.Interfaces;
using Vueling.Otd.Infrastructure.CurrencyPair.Services;

namespace Vueling.Otd.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Currency pair services
            services.AddTransient<ICurrencyPairService, CurrencyPairService>();
            services.AddTransient<ICurrencyPairStateService, CurrencyPairStateService>();

            // Transaction services
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<ITransactionStateService, TransactionStateService>();

            services.AddHttpClient("GatewayApiClient", (provider, client) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var baseAddress = configuration.GetRequiredSection("GatewayApi")["BaseUrl"];
                client.BaseAddress = new Uri(baseAddress);
            });

            return services;
        }
    }
}