

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoBackupHelperApp.Config;
using System.Security.Authentication.ExtendedProtection;

namespace MongoBackupHelperApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                AppConfig();

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        private static void AppConfig()
        {

            var serviceCollection = new ServiceCollection();

            var builder= new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                // optional false — файл обязателен, и приложение завершится с ошибкой, если файл не найден.
                // reloadonchange true — конфигурация будет автоматически обновляться, если файл изменится.
                .AddJsonFile("options1.json",optional:false,reloadOnChange:true) 
                .Build();

            serviceCollection.Configure<AppConfig>(builder.GetRequiredSection("Options"));

            var serviceProvider= serviceCollection.BuildServiceProvider();

            // Пример получения конфигурации из DI контейнера через IOptions
            var appConfig = serviceProvider.GetRequiredService<IOptions<AppConfig>>().Value;

            Console.WriteLine($"Connection String: {appConfig.ConnectionString}");
            Console.WriteLine($"Backup Folder: {appConfig.BackupFolder}");
        }
    }
}
