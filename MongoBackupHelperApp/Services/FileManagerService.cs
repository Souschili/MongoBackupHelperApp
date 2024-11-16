using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

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
            if (!Directory.Exists(_appConfig.Value.BackupFolder))
            {
                throw new InvalidOperationException($"Unable to find backup directory path {_appConfig.Value.BackupFolder}");
            }

            //// получаем список файлов,убираем полный путь ,если их нет пишем что ноль и выходим
            var files = new DirectoryInfo(_appConfig.Value.BackupFolder)
                .EnumerateFiles().ToList();

            if (!files.Any())
                throw new Exception($"Files to upload not found in {_appConfig.Value.BackupFolder}");

            GetUploadInfoAndData(files);

            return null;
        }

        public Dictionary<string, List<BsonDocument>> GetUploadInfoAndData(List<FileInfo> files)
        {
            Dictionary<string,List<BsonDocument>> uploadInfo= new Dictionary<string, List<BsonDocument>>();
            foreach (var item in files)
            {
                Console.WriteLine(item);
            }
            return uploadInfo;
        }

    }
}
