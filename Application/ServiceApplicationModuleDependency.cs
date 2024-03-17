using Application.Services;
using Domain.Ports.Repositories;
using Domain.Ports.Services;
using Domain.Ports.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceApplicationModuleDependency
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IFileService, FileService>();

            services.AddScoped(typeof(IExcelService<>), typeof(ExcelService<>));

            return services;
        }
    }
}
