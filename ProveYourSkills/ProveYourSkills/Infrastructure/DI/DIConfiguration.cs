using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProveYourSkills.Core.Http;
using ProveYourSkills.Infrastructure.Http;
using ProveYourSkills.UI.ViewModel;
using Serilog;

namespace ProveYourSkills.Infrastructure.DI
{
    public static class DIConfiguration
    {
        public static void SetupServiceCollection(HostBuilderContext context, IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Warning()
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            services.AddHttpClient();
            services.AddScoped<IRestApiClient, RestApiProxy>();
            services.AddScoped<IPostApiClient, PostApiClient>();

            // Register Views
            services.AddSingleton<PostGridViewModel>();
            services.AddSingleton<MainWindow>();
        }
    }
}
