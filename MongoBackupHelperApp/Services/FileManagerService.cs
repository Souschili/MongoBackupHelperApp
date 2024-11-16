using Microsoft.Extensions.Options;
using MongoBackupHelperApp.Model;
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

        private IEnumerable<FileInfo> GetFiles()
        {
            // чекаем есть ли эта директория физически
            if (!Directory.Exists(_appConfig.Value.BackupFolder))
            {
                throw new InvalidOperationException($"Unable to find backup directory path {_appConfig.Value.BackupFolder}");
            }

            // получаем список файлов,убираем полный путь ,если их нет пишем что ноль и выходим
            var files = new DirectoryInfo(_appConfig.Value.BackupFolder)
                .EnumerateFiles();

            if (!files.Any())
                throw new Exception($"Files to upload not found in {_appConfig.Value.BackupFolder}");

            return files;
        }

        public async Task<Dictionary<string, IEnumerable<BsonDocument>>> GetUploadInfoAndData()
        {
            try
            {

                var files = GetFiles();
                var parseTasks = files.Select(async x => new UploadInfoModel
                {
                    CollectionName = ParseCollectionName(x),
                    Documents = await GetDataFromFile(x)
                });

                var parseResult = await Task.WhenAll(parseTasks);
                var dictionary = parseResult.ToDictionary(f => f.CollectionName, f => f.Documents ?? Enumerable.Empty<BsonDocument>());
                return dictionary;
            }
            catch (AggregateException ex)
            {
                foreach (var innerEx in ex.InnerExceptions)
                {
                    Console.WriteLine($"Task failed with error: {innerEx.Message}");
                    Console.WriteLine(innerEx.StackTrace);
                }
                return new Dictionary<string, IEnumerable<BsonDocument>>();
            }
        }

        private async Task<IEnumerable<BsonDocument>> GetDataFromFile(FileInfo file)
        {
            if (!file.Exists)
                throw new FileNotFoundException($"File not exitst {file.Name}", nameof(file));
            try
            {
                var jsonContent = await File.ReadAllTextAsync(file.FullName);
                List<BsonDocument> documents = BsonSerializer.Deserialize<List<BsonDocument>>(jsonContent);
                return documents;
            }
            catch (IOException ex)
            {
                throw new Exception($"Error reading file: {file.Name}", ex);
            }
            catch (BsonSerializationException ex)
            {
                throw new Exception($"Error deserializing BSON from file: {file.Name}", ex);
            }

        }

        private string ParseCollectionName(FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file), "FileInfo cannot be null.");

            if (file.Name.Count(c => c == '.') < 2)
                throw new ArgumentException($"Invalid file name format: {file.Name}", nameof(file));

            var start = file.Name.IndexOf(".") + 1;
            var end = file.Name.LastIndexOf(".");

            return file.Name[start..end];
        }

    }
}
