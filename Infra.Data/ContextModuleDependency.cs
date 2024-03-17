using Domain.Ports.Repositories;
using Domain.Ports.Settings;
using Infra.Data.Repositories.Base;
using Infra.Data.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infra.Data
{
    public static class ContextModuleDependency
    {
        public static IServiceCollection AddTradeContextModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TradeContextSettings>(configuration.GetSection("TradeContext"));
            services.AddSingleton<ITradeContextSettings>(serviceProvider =>
                        serviceProvider.GetRequiredService<IOptions<TradeContextSettings>>().Value);

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            return services;
        }
    }
}
