using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoBackupHelperApp.Services;
using MongoDB.Driver;
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
                await _service.UploadBackupAsync();
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
            // DI container
            serviceCollection.AddSingleton<IMongoDatabase>(cfg =>
            {
                var config = cfg.GetRequiredService<IOptions<AppConfig>>().Value;
                config.Validate(); // проверка конфигов
                IMongoClient client = new MongoClient(config.ConnectionString);
                return client.GetDatabase(config.DataBaseName);
            });
            serviceCollection.AddScoped<IMongoUploaderService,MongoUploaderService>();
            serviceCollection.AddScoped<IFileManagerService, FileManagerService>();


            var serviceProvider = serviceCollection.BuildServiceProvider();


            // присваиваем ,так как статик методы и по другому неоч красиво
            _service = serviceProvider.GetRequiredService<MongoUploaderService>();

        }
    }
}
