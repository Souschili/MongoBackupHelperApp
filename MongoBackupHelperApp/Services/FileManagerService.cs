using Microsoft.Extensions.Options;

namespace MongoBackupHelperApp.Services
{
    internal class FileManagerService
    {
        private readonly IOptions<AppConfig> _appConfig;

        public FileManagerService(IOptions<AppConfig> appConfig)
        {
            appConfig.Value.Validate();
            _appConfig = appConfig;
        }

        public IEnumerable<string> GetFiles()
        {
            // чекаем есть ли эта директория физически
            if(!Directory.Exists(_appConfig.Value.BackupFolder))
            {
                throw new InvalidOperationException($"Unable to find backup directory path {_appConfig.Value.BackupFolder}");
            }

            // получаем список файлов,убираем полный путь ,если их нет пишем что ноль и выходим
            var files=new DirectoryInfo(_appConfig.Value.BackupFolder)
                .EnumerateFiles()
                .Select(x=> x.Name);

            if (!files.Any())
                throw new Exception($"Files to upload not found in {_appConfig.Value.BackupFolder}");
            
           return files;
        }
    }
}
