using Microsoft.Extensions.Options;
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

        public MongoUploaderService(IFileManagerService fileManagerService)
        {
            _fileManagerService = fileManagerService;
        }

        public async Task UploadBackupAsync()
        {
            //1 получили имена файлов
            var files=await _fileManagerService.GetUploadInfoAndData();
            
            Console.WriteLine();
        }

    }
}
