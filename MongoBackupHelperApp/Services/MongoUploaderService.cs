using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoBackupHelperApp.Services
{
    internal class MongoUploaderService
    {
        private const int CHUNK_SIZE = 5;
        private readonly IFileManagerService _fileManagerService;
        private readonly IMongoDatabase _database;

        public MongoUploaderService(IFileManagerService fileManagerService, IMongoDatabase database = null)
        {
            _fileManagerService = fileManagerService;
            _database = database;
        }

        public async Task UploadBackupAsync()
        {
            //1 получили имена файлов
            var filesData = await _fileManagerService.GetUploadInfoAndData();
            if (filesData == null) //если каким то хреном он пустой то ничего не делаем
                throw new ArgumentNullException("No data to upload", nameof(filesData));

            // асинхроное выполнение с ожиданием завершения всех зададч
            var uploadTask = filesData.Select(x => UploadToCollection(x));
            await Task.WhenAll(uploadTask);

            // паралельное выпонение,не возвращает результат
            //await Parallel.ForEachAsync(filesData,
            //    new ParallelOptions { MaxDegreeOfParallelism = 10 },
            //    async (file, _) =>
            //     {
            //         await UploadToCollection(file);
            //     });
        }

        private async Task UploadToCollection(KeyValuePair<string, IEnumerable<BsonDocument>> uploadData)
        {
            var collection = _database.GetCollection<BsonDocument>(uploadData.Key);
            await collection.InsertManyAsync(uploadData.Value);

            // список где будут операции записи,которые выпоним одним запросом
            // эфективно если надо выполнить несколько разных операций
            //var bulkWrite = new List<WriteModel<BsonDocument>>();
            //foreach (var item in uploadData.Value)
            //{
            //    bulkWrite.Add(new InsertOneModel<BsonDocument>(item));
            //}
            //var random=new Random();
            //await Task.Delay(random.Next(10000));
            Console.WriteLine($"Upload to {uploadData.Key} done");
        }
    }
}
