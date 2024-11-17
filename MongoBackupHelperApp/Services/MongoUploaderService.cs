using Microsoft.Extensions.Options;
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
            var files=await _fileManagerService.GetUploadInfoAndData();
            var t=_database.ListCollections();
            Console.WriteLine();
        }

    }
}
