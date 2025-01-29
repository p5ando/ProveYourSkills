using Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProveYourSkills.Http;
using Serilog;
using System.Windows;

namespace ProveYourSkills
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Warning()
                .CreateLogger();

            AppHost = Host.CreateDefaultBuilder()
                .UseSerilog() // Use Serilog as the logging provider
                .ConfigureServices((context, services) =>
                {
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
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await AppHost.StopAsync();
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }
}
