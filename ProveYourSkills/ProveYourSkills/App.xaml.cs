using Microsoft.Extensions.DependencyInjection;
using ProveYourSkills.Models;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace ProveYourSkills
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            // setup di container
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // open window
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        // register service
        private void ConfigureServices(IServiceCollection services)
        {
            // Configure Logging
            services.AddLogging();
            // HTTP Client
            services.AddHttpClient(nameof(JsonPlaceholderClient), client =>
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddScoped<IJsonPlaceholderClient, JsonPlaceholderClient>();

            // Register Views
            services.AddSingleton<PostGridViewModel>();
            services.AddSingleton<MainWindow>();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            // Dispose of services if needed
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
