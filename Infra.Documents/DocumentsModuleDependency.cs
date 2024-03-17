using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.Console;
using MongoDB.Driver;
using Domain.Ports.Services;
using Infra.Documents.Operations;

namespace Infra.Documents
{
    public static class DocumentsModuleDependency
    {
        public static IServiceCollection AddDocumentsService(this IServiceCollection services, IConfiguration configuration)
        {
            string mongoConnectionString = configuration.GetSection("TradeContext:ConnectionString").Value;
            string dbName = configuration.GetSection("TradeContext:DatabaseHangFireName").Value;
            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseConsole()
            .UseMongoStorage(mongoConnectionString, dbName, new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new CollectionMongoBackupStrategy()
                },
                Prefix = "trade.hangfire",
                CheckConnection = false
            })
        );

            // Configurar Hangfire para executar em segundo plano
            services.AddHangfireServer();

            services.AddTransient<IFileAtapter, FileAtapter>();
            return services;
        }
    }
}
