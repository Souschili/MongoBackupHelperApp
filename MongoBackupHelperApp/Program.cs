using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MongoBackupHelperApp
{
    internal class Program
    {
        private static AppConfig _config;
        static async Task Main(string[] args)
        {
            try
            {
                AppConfig();
                Console.WriteLine($"Connection String: {_config.ConnectionString}");
                Console.WriteLine($"Backup Folder: {_config.BackupFolder}");
                Console.WriteLine($"Data Base: {_config.DataBaseName}");

                if (!Directory.Exists(_config.BackupFolder))
                {
                    throw new DirectoryNotFoundException($"Directory {_config.BackupFolder} not found");
                }

               // var files=Directory.GetFiles(_config.BackupFolder);
                var files=Directory.GetFiles(_config.BackupFolder,"*.json");
                var name=Path.GetFileName(files[0]);
                var first = name.IndexOf(".");
                var end = name.LastIndexOf(".");
                Console.WriteLine($"start:{first}---end:{end}");
                Console.WriteLine(name.Substring(9+1,22-9-1));
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

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Пример получения конфигурации из DI контейнера через IOptions
            var appConfig = serviceProvider.GetRequiredService<IOptions<AppConfig>>().Value;
            appConfig.Validate();

            _config = appConfig;
        }
    }
}
