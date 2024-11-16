using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoBackupHelperApp.Services;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MongoBackupHelperApp
{
    internal class Program
    {
        private static MongoUploaderService _service;
        static async Task Main(string[] args)
        {
            try
            {
                AppConfig();
                _service.StartUpload();
               
               
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Configuration file not found {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Unable to parse config file {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled error {ex.Message}");
            }

        }

        private static void AppConfig()
        {

            var serviceCollection = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                // optional false — файл обязателен, и приложение завершится с ошибкой, если файл не найден.
                // reloadonchange true — конфигурация будет автоматически обновляться, если файл изменится.
                .AddJsonFile("options.json", optional: false, reloadOnChange: true)
                .Build();

            serviceCollection.Configure<AppConfig>(builder.GetRequiredSection("Options"));
            serviceCollection.AddScoped<MongoUploaderService>();
            serviceCollection.AddScoped<FileManagerService>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            _service=serviceProvider.GetRequiredService<MongoUploaderService>();
            
        }
    }
}
