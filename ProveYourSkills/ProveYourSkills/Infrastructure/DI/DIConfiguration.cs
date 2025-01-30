using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProveYourSkills.Core.Services;
using ProveYourSkills.Core.Services.Abstractions;
using ProveYourSkills.Infrastructure.Configuration;
using ProveYourSkills.Infrastructure.Http;
using ProveYourSkills.Infrastructure.Http.Abstractions;
using ProveYourSkills.UI.ViewModel;
using Serilog;

namespace ProveYourSkills.Infrastructure.DI;

public static class DIConfiguration
{
    public static void SetupServiceCollection(HostBuilderContext context, IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Information()
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });

        services.Configure<AppSettings>(context.Configuration.GetSection("AppSettings"));
        services.AddHttpClient();
        services.AddScoped<IUiComponentFactory, UiComponentFactory>();
        services.AddScoped<IGridCellBuilder, GridCellBuilder>();
        services.AddScoped<IRestApiClient, RestApiClient>();
        //services.AddScoped<IRestApiClient, RestApiProxy>();
        services.AddScoped<IPostApiClient, PostApiClient>();

        // Register Views
        services.AddSingleton<PostGridViewModel>();
        services.AddSingleton<MainWindow>();
    }
}
