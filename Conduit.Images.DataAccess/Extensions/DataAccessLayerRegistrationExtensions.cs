using System.Reflection;
using Conduit.Images.DataAccess.Articles;
using Conduit.Images.DataAccess.Images.Repositories;
using Conduit.Images.DataAccess.Images.Services;
using Conduit.Images.DataAccess.Migrations;
using Conduit.Images.Domain.Articles;
using Conduit.Images.Domain.Images.Services.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Conduit.Images.DataAccess.Extensions;

public static class DataAccessLayerRegistrationExtensions
{
    public static void RegisterDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        bool addMigrations)
    {
        services.AddScoped<IArticleConsumerRepository, ArticleConsumerRepository>()
            .AddScoped<IArticleReadRepository, ArticleReadRepository>()
            .AddScoped<DataAccessLayerInitializer, DataAccessLayerInitializer>()
            .AddScoped<IImageReadRepository, ImageReadRepository>()
            .AddScoped<IImageWriteRepository, ImageWriteRepository>()
            .AddSingleton<IImageStorageNameGenerator, ConfiguredImageStorageNameGenerator>()
            .AddSingleton<IImageUrlProvider, ConfiguredImageUrlProvider>()
            .AddSingleton<IImageStorage, LocalImageStorage>()
            .AddScoped<ConnectionProvider>();

        services.AddHealthChecks()
            .AddNpgSql(GetImageDatabase);

        services.Configure<ConfiguredImageUrlProvider.Options>(configuration.GetSection("ConfiguredImageUrlProvider").Bind);
        services.Configure<ConfiguredImageStorageNameGenerator.Options>(configuration.GetSection("ConfiguredImageStorageNameGenerator").Bind);
        services.Configure<ConnectionProvider.Options>(configuration.GetSection("ConnectionProvider").Bind);
        services.Configure<LocalImageStorage.Options>(configuration.GetSection("LocalImageStorage").Bind);

        if (addMigrations)
        {
            var connectionProviderOptions = new ConnectionProvider.Options();
            configuration.GetSection("ConnectionProvider").Bind(connectionProviderOptions);
            services.AddFluentMigratorCore()
                .ConfigureRunner(options => options.AddPostgres()
                .WithGlobalConnectionString(connectionProviderOptions.ImageDatabase)
                .ScanIn(typeof(Initial).Assembly).For.Migrations());
        }
    }

    private static string GetImageDatabase(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<IOptions<ConnectionProvider.Options>>().Value.ImageDatabase;
    }
}
